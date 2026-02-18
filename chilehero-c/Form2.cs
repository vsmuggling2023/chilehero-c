using MySqlConnector;
using System;
using System.Data;
using System.Drawing;
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

        // Colores
        private readonly Color _inputBg = ColorTranslator.FromHtml("#202024");
        private readonly Color _inputText = Color.Gainsboro;
        private readonly Color _yellow = ColorTranslator.FromHtml("#F2C200");
        private readonly Color _yellowFx = ColorTranslator.FromHtml("#FFD54A");

        // ✅ CONSTRUCTOR ÚNICO
        public Form2(string connectionString = "", string rol = "", string nombre = "")
        {
            // NO usar Designer
            InitializeComponent();

            _connectionString = connectionString;
            _rol = string.IsNullOrWhiteSpace(rol) ? "Usuario" : rol;
            _nombre = string.IsNullOrWhiteSpace(nombre) ? "Usuario" : nombre;

            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Sizable;
            BackColor = Color.Black;

            BuildBackground();
            MoveExistingControlsToLayout();

            lb_bienvenido.Text = "Bienvenido " + _nombre;
            lb_rol.Text = "Tu rol es: "+ _rol;
        }

        private void BuildBackground()
        {
            bg = new GradientPanel
            {
                Dock = DockStyle.Fill,
                StartColor = ColorTranslator.FromHtml("#01030A"),
                EndColor = ColorTranslator.FromHtml("#040719"),
                Mode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal
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
        }
    }
}
