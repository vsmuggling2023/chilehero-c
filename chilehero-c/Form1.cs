using System;
using System.Drawing;
using System.Drawing.Drawing2D; // ✅ agregado
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using MySqlConnector;

namespace chilehero_c
{
    public partial class Form1 : Form
    {
        private GradientPanel bg;

        // Layout responsive
        private Panel container;
        private TableLayoutPanel layout;

        // Bordes para tus textboxes existentes
        private RoundedBorderPanel correoBorder;
        private RoundedBorderPanel passBorder;

        // Colores
        private readonly Color _inputBg = ColorTranslator.FromHtml("#202024");
        private readonly Color _inputText = Color.Gainsboro;
        private readonly Color _yellow = ColorTranslator.FromHtml("#F2C200");
        private readonly Color _yellowFx = ColorTranslator.FromHtml("#FFD54A");

        public Form1()
        {
            InitializeComponent();

            // ✅ AQUI: aplicar estilo al botón (sin tocar el Designer)
            ReplaceLoginButtonStyle();

            // ✅ Que el form abra centrado
            StartPosition = FormStartPosition.CenterScreen;

            // ✅ No maximizar
            MaximizeBox = false;

            // OPCIONAL: si además NO quieres resize arrastrando:
            // FormBorderStyle = FormBorderStyle.FixedSingle;

            BuildBackground();
            BuildResponsiveLayout();
            MoveExistingControlsToLayout();
            WrapExistingTextBoxesWithYellowBorder();

            // ✅ Centrar cuando ya hay tamaños reales
            Shown += (s, e) => CenterLayout();
            Resize += (s, e) => CenterLayout();
        }

        /// <summary>
        /// Reemplaza el btn_iniciasesion estándar por GradientButton manteniendo:
        /// - Parent (el layout lo asignas después, no importa)
        /// - evento Click (login)
        /// - nombre, texto, tamaño, márgenes, dock, anchor
        /// </summary>
        private void ReplaceLoginButtonStyle()
        {
            // Si el botón ya fue reemplazado, no hagas nada
            if (btn_iniciasesion is GradientButton) return;

            var old = btn_iniciasesion;

            // Crear el nuevo botón estilizado
            var gb = new GradientButton
            {
                Name = old.Name,
                Text = string.IsNullOrWhiteSpace(old.Text) ? "INGRESAR" : old.Text,

                Font = old.Font ?? new Font("Oxanium", 12f, FontStyle.Bold),
                ForeColor = Color.White,

                // Copiar layout básico (igual lo vuelves a setear en MoveExistingControlsToLayout)
                Size = old.Size,
                Location = old.Location,
                Dock = old.Dock,
                Anchor = old.Anchor,
                Margin = old.Margin,
                Padding = old.Padding,

                // ✅ Colores del estilo del botón (ajústalos si quieres)
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

            // ✅ Mantener el click del login
            gb.Click += btn_iniciasesion_Click;

            // Reemplazo en el contenedor original (si todavía tiene parent en este punto)
            var parent = old.Parent;
            if (parent != null)
            {
                int index = parent.Controls.GetChildIndex(old);
                parent.Controls.Remove(old);
                parent.Controls.Add(gb);
                parent.Controls.SetChildIndex(gb, index);
            }

            // ✅ Cambiar referencia (esto es CLAVE)
            btn_iniciasesion = gb;
        }

        // 1) Fondo gradiente
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
        }

        // 2) Layout responsive: contenedor + TableLayout centrado
        private void BuildResponsiveLayout()
        {
            container = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };
            bg.Controls.Add(container);

            layout = new TableLayoutPanel
            {
                ColumnCount = 1,
                RowCount = 7, // ✅ 7 filas: logo, titulo, label correo, input correo, label pass, input pass, botón
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.Transparent
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            for (int i = 0; i < 7; i++)
                layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            container.Controls.Add(layout);

            container.Resize += (s, e) => CenterLayout();
        }

        // Centrar y ajustar ancho del bloque
        private void CenterLayout()
        {
            if (container == null || layout == null) return;

            int targetWidth = (int)(container.ClientSize.Width * 0.70);
            targetWidth = Math.Max(320, Math.Min(520, targetWidth));
            layout.Width = targetWidth;

            layout.Left = (container.ClientSize.Width - layout.Width) / 2;
            layout.Top = (container.ClientSize.Height - layout.Height) / 2;
        }

        // 3) Tomar controles existentes y meterlos al layout
        private void MoveExistingControlsToLayout()
        {
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Parent = bg;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            lb_titulo.BackColor = Color.Transparent;
            lb_correo.BackColor = Color.Transparent;
            lb_pass.BackColor = Color.Transparent;

            lb_titulo.ForeColor = Color.Yellow;
            lb_correo.ForeColor = Color.Yellow;
            lb_pass.ForeColor = Color.Yellow;

            lb_titulo.AutoSize = true;
            lb_correo.AutoSize = true;
            lb_pass.AutoSize = true;

            pictureBox1.Margin = new Padding(0, 0, 0, 16);
            lb_titulo.Margin = new Padding(0, 0, 0, 22);
            lb_correo.Margin = new Padding(0, 0, 0, 8);
            lb_pass.Margin = new Padding(0, 18, 0, 8);

            // Agregar al layout
            layout.Controls.Add(pictureBox1, 0, 0);
            layout.Controls.Add(lb_titulo, 0, 1);
            layout.Controls.Add(lb_correo, 0, 2);
            layout.Controls.Add(lb_pass, 0, 4);

            pictureBox1.Anchor = AnchorStyles.None;

            // ✅ Botón en la fila 6 (debajo del pass)
            btn_iniciasesion.Parent = layout;
            btn_iniciasesion.Dock = DockStyle.Top;
            btn_iniciasesion.Height = 45;
            btn_iniciasesion.Margin = new Padding(0, 18, 0, 0);
            btn_iniciasesion.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            layout.Controls.Add(btn_iniciasesion, 0, 6);
        }

        // 4) Envolver txt_correo y txt_pass con borde amarillo
        private void WrapExistingTextBoxesWithYellowBorder()
        {
            correoBorder = CreateBorderForTextBox(txt_correo);
            passBorder = CreateBorderForTextBox(txt_pass);

            layout.Controls.Add(correoBorder, 0, 3);
            layout.Controls.Add(passBorder, 0, 5);

            correoBorder.Dock = DockStyle.Top;
            passBorder.Dock = DockStyle.Top;

            correoBorder.Height = 48;
            passBorder.Height = 48;

            txt_pass.UseSystemPasswordChar = true;

            txt_correo.Dock = DockStyle.Fill;
            txt_pass.Dock = DockStyle.Fill;
        }

        private RoundedBorderPanel CreateBorderForTextBox(TextBox tb)
        {
            var border = new RoundedBorderPanel
            {
                BackColor = Color.Transparent,
                FillColor = _inputBg,
                BorderColor = _yellow,
                BorderFocusColor = _yellowFx,
                BorderRadius = 12,
                BorderSize = 2,
                Padding = new Padding(12, 10, 12, 10),
                Margin = new Padding(0)
            };

            tb.BorderStyle = BorderStyle.None;
            tb.Multiline = true;
            tb.ScrollBars = ScrollBars.None;

            tb.BackColor = _inputBg;
            tb.ForeColor = _inputText;
            tb.Font = new Font("Oxanium", 10f, FontStyle.Bold);

            tb.Parent = border;
            tb.Dock = DockStyle.Fill;

            tb.Enter += (s, e) =>
            {
                border.SetFocused(true);
                tb.BackColor = _inputBg;
            };

            tb.Leave += (s, e) =>
            {
                border.SetFocused(false);
                tb.BackColor = _inputBg;
            };

            tb.TextChanged += (s, e) =>
            {
                if (tb.BackColor != _inputBg)
                    tb.BackColor = _inputBg;
            };

            return border;
        }

        // ========= HELPERS PARA EVITAR DBNull =========

        private static string DbNullToNull(object value)
        {
            return (value == null || value == DBNull.Value) ? null : Convert.ToString(value);
        }

        private static string ComputeSha256Hash(string raw)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(raw);
                var hash = sha.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        // ========= LOGIN =========

        private void btn_iniciasesion_Click(object sender, EventArgs e)
        {
            var usuario = txt_correo.Text.Trim();
            var pass = txt_pass.Text;

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Ingrese usuario y contraseña.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var connectionString = "Server=server.001webhospedaje.com;Database=tdrmgkza_chilehero;Uid=tdrmgkza_chilehero;Pwd=181730366u;";

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();


                    const string sql = @"
                        SELECT 
                            u.nombre,
                            u.contrasena, 
                            u.rol, 
                            b.razon, 
                            b.baneado_por
                        FROM usuarios u
                        LEFT JOIN Baneados b ON b.correo = u.correo
                        WHERE u.correo = @user
                        LIMIT 1;
                    ";


                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@user", usuario);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show("Usuario o contraseña incorrectos.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            var dbHash = DbNullToNull(reader["contrasena"]);
                            var rolRaw = DbNullToNull(reader["rol"]);
                            var rol = string.IsNullOrWhiteSpace(rolRaw) ? "Usuario" : rolRaw;

                            var nombre = DbNullToNull(reader["nombre"]);
                            if (string.IsNullOrWhiteSpace(nombre))
                                nombre = usuario;

                            var razon = DbNullToNull(reader["razon"]);
                            var baneadoPor = DbNullToNull(reader["baneado_por"]);

                            if (string.IsNullOrWhiteSpace(dbHash))
                            {
                                MessageBox.Show("Usuario o contraseña incorrectos.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            var enteredHash = ComputeSha256Hash(pass);

                            if (!string.Equals(dbHash, enteredHash, StringComparison.OrdinalIgnoreCase))
                            {
                                MessageBox.Show("Usuario o contraseña incorrectos.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            bool estaBaneado =
                                string.Equals(rol, "Baneado", StringComparison.OrdinalIgnoreCase)
                                || !string.IsNullOrWhiteSpace(razon)
                                || !string.IsNullOrWhiteSpace(baneadoPor);

                            if (estaBaneado)
                            {
                                string razonMsg = string.IsNullOrWhiteSpace(razon) ? "No especificada" : razon;
                                string baneadoPorMsg = string.IsNullOrWhiteSpace(baneadoPor) ? "No especificado" : baneadoPor;

                                MessageBox.Show(
                                    "Tu cuenta está BANEADA.\n\n" +
                                    "Razón: " + razonMsg + "\n" +
                                    "Baneado por: " + baneadoPorMsg + "\n" +
                                    "Contacta al administrador para revisar tu cuenta\n" +
                                    "chilehero2023@gmail.com",
                                    "Acceso denegado",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error
                                );

                                txt_pass.Clear();
                                txt_pass.Focus();
                                return;
                            }

                            MessageBox.Show("Inicio de sesión correcto. Rol: " + rol, "Éxito",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            var f2 = new Form2(connectionString, rol, nombre);
                            f2.Show();
                            this.Hide();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar o ejecutar consulta: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
