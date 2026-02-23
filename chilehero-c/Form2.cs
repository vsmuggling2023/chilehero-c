using MySqlConnector;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace chilehero_c
{
    public partial class Form2 : Form
    {
        private GradientPanel bg;

        private readonly string _connectionString;
        private readonly string _rol;
        private readonly string _nombre;

        private DataTable _dtCanciones;
        private readonly BindingSource _bsCanciones = new BindingSource();
        private bool _cerrandoPorLogout = false;

        private readonly Color _inputBg = ColorTranslator.FromHtml("#202024");
        private readonly Color _inputText = Color.Gainsboro;
        private readonly Color _yellow = ColorTranslator.FromHtml("#F2C200");
        private readonly Color _yellowFx = ColorTranslator.FromHtml("#FFD54A");

        public Form2(string connectionString = "", string rol = "", string nombre = "")
        {
            InitializeComponent();

            _connectionString = connectionString;
            _rol = string.IsNullOrWhiteSpace(rol) ? "Usuario" : rol;
            _nombre = string.IsNullOrWhiteSpace(nombre) ? "Usuario" : nombre;

            ReplaceMenuButtonsStyle();
            ReplaceAdminButtonStyle();

            ApplyRoleVisibility();

            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Sizable;
            BackColor = Color.Black;

            BuildBackground();
            MoveExistingControlsToLayout();

            lb_bienvenido.Text = "Bienvenido " + _nombre;
            lb_rol.Text = "Tu rol es: " + _rol;
        }

        private void ApplyRoleVisibility()
        {
            bool esAdminODueno =
                string.Equals(_rol, "Administrador", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(_rol, "Dueño", StringComparison.OrdinalIgnoreCase);

            btn_accederadmin.Visible = esAdminODueno;
        }

        private void BuildBackground()
        {
            bg = new GradientPanel
            {
                Dock = DockStyle.Fill,
                StartColor = ColorTranslator.FromHtml("#01030A"),
                EndColor = ColorTranslator.FromHtml("#040719"),
                Mode = LinearGradientMode.BackwardDiagonal
            };

            Controls.Add(bg);
            bg.SendToBack();
            bg.Invalidate();
        }

        private void MoveExistingControlsToLayout()
        {
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Parent = bg;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            if (lb_bienvenido != null)
            {
                lb_bienvenido.BackColor = Color.Transparent;
                lb_bienvenido.ForeColor = Color.Yellow;
            }

            if (lb_rol != null)
            {
                lb_rol.BackColor = Color.Transparent;
                lb_rol.ForeColor = Color.Yellow;
            }
        }

        private void ReplaceMenuButtonsStyle()
        {
            ReplaceWithGradientButtonLikeLogin(ref btn_canciones, btn_canciones_Click);
            ReplaceWithGradientButtonLikeLogin(ref btn_packs, btn_packs_Click);
            ReplaceWithGradientButtonLikeLogin(ref btn_subirpuntajes, btn_subirpuntajes_Click);
            ReplaceWithGradientButtonLikeLogin(ref btn_vertuspuntajes, btn_vertuspuntajes_Click);
        }

        private void ReplaceWithGradientButtonLikeLogin(ref Button btn, EventHandler onClick)
        {
            if (btn == null) return;
            if (btn is GradientButton) return;

            var old = btn;

            var gb = new GradientButton
            {
                Name = old.Name,
                Text = string.IsNullOrWhiteSpace(old.Text) ? old.Name : old.Text,

                Font = old.Font ?? new Font("Oxanium", 12f, FontStyle.Bold),
                ForeColor = Color.White,

                Size = old.Size,
                Location = old.Location,
                Dock = old.Dock,
                Anchor = old.Anchor,
                Margin = old.Margin,
                Padding = old.Padding,

                StartColor = ColorTranslator.FromHtml("#D12A2A"),
                EndColor = ColorTranslator.FromHtml("#2A45D1"),
                Mode = LinearGradientMode.ForwardDiagonal,
                BorderColor = _yellow,
                BorderHoverColor = _yellowFx,
                BorderRadius = 12,
                BorderSize = 2,
                GlowOnHover = true,
                GlowSize = 8
            };

            gb.Click += onClick;

            var parent = old.Parent;
            if (parent != null)
            {
                int index = parent.Controls.GetChildIndex(old);
                parent.Controls.Remove(old);
                parent.Controls.Add(gb);
                parent.Controls.SetChildIndex(gb, index);
            }

            btn = gb;
        }

        private void ReplaceAdminButtonStyle()
        {
            ReplaceWithYellowPhotoStyle(ref btn_accederadmin, btn_accederadmin_Click);
        }

        private void ReplaceWithYellowPhotoStyle(ref Button btn, EventHandler onClick)
        {
            if (btn == null) return;
            if (btn is GradientButton) return;

            var old = btn;

            Color top = ColorTranslator.FromHtml("#F2C200");
            Color bottom = ColorTranslator.FromHtml("#C98A00");
            Color border = ColorTranslator.FromHtml("#F2C200");
            Color hover = ColorTranslator.FromHtml("#FFD54A");

            var gb = new GradientButton
            {
                Name = old.Name,
                Text = string.IsNullOrWhiteSpace(old.Text) ? "ACCEDER" : old.Text,

                Font = old.Font ?? new Font("Oxanium", 12f, FontStyle.Bold),
                ForeColor = Color.Black,

                Size = old.Size,
                Location = old.Location,
                Dock = old.Dock,
                Anchor = old.Anchor,
                Margin = old.Margin,
                Padding = old.Padding,
                StartColor = top,
                EndColor = bottom,
                Mode = LinearGradientMode.Vertical,

                BorderColor = border,
                BorderHoverColor = hover,
                BorderRadius = 10,
                BorderSize = 2,

                GlowOnHover = true,
                GlowSize = 6
            };

            gb.Click += onClick;

            var parent = old.Parent;
            if (parent != null)
            {
                int index = parent.Controls.GetChildIndex(old);
                parent.Controls.Remove(old);
                parent.Controls.Add(gb);
                parent.Controls.SetChildIndex(gb, index);
            }

            btn = gb;
        }
        private void btn_canciones_Click(object sender, EventArgs e)
        {
            var f3 = new Form3(_connectionString);
            f3.Show();
        }

        private void btn_packs_Click(object sender, EventArgs e)
        {
            CustomMessageBox.Info("Sección Packs\n\n(Solo diseño por ahora)", "En construcción", this);
        }

        private void btn_subirpuntajes_Click(object sender, EventArgs e)
        {
            var f4 = new Form4(_connectionString);
            f4.Show();
        }

        private void btn_vertuspuntajes_Click(object sender, EventArgs e)
        {
            CustomMessageBox.Info("Ver tus Puntajes\n\n(Solo diseño por ahora)", "En construcción", this);
        }

        private void btn_accederadmin_Click(object sender, EventArgs e)
        {
            CustomMessageBox.Info("Acceso Admin\n\n(Solo diseño por ahora)", "En construcción", this);
        }
    }
}