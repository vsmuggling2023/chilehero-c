using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using MySqlConnector;

namespace chilehero_c
{
    public partial class Form4 : Form
    {
        private GradientPanel bg;
        private readonly string _connectionString;

        private readonly Color _yellow = ColorTranslator.FromHtml("#F2C200");
        private readonly Color _yellowFx = ColorTranslator.FromHtml("#FFD54A");

        private string _capturaBase64 = null;

        public Form4(string connectionString = "")
        {
            InitializeComponent();

            _connectionString = connectionString;

            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Sizable;
            BackColor = Color.Black;

            BuildBackground();
            MoveExistingControlsToLayout();

            ReplaceButtonsStyle();
            SetupPictureBox();
            SetupInstrumentos();

            this.Load += Form4_Load;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            CargarCancionesEnCombo();
        }

        // =========================================================
        //  BACKGROUND
        // =========================================================
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

        // =========================================================
        //  LABELS (amarillo + transparente)
        // =========================================================
        private void MoveExistingControlsToLayout()
        {
            ApplyLabelStyle(lb_cancion);
            ApplyLabelStyle(lb_instrumento);
            ApplyLabelStyle(lb_puntaje);
            ApplyLabelStyle(lb_porcentaje);
            ApplyLabelStyle(lb_youtube);
            ApplyLabelStyle(lb_captura);
        }

        private void ApplyLabelStyle(Label lb)
        {
            if (lb == null) return;
            lb.BackColor = Color.Transparent;
            lb.ForeColor = Color.Yellow;
            lb.AutoSize = true;
        }

        // =========================================================
        //  BOTONES: mismo estilo que Form2 (canciones)
        // =========================================================
        private void ReplaceButtonsStyle()
        {
            ReplaceWithGradientButtonLikeMenu(ref btn_subircaptura, btn_subircaptura_Click);
            ReplaceWithGradientButtonLikeMenu(ref btn_enviarpuntaje, btn_enviarpuntaje_Click);
        }

        private void ReplaceWithGradientButtonLikeMenu(ref Button btn, EventHandler onClick)
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

        // =========================================================
        //  COMBO CANCIONES: "Artista - Canción"
        // =========================================================
        private void CargarCancionesEnCombo()
        {
            if (cb_cancion == null) return;

            const string sql = @"
                SELECT 
                    c.id AS id,
                    CONCAT(a.nombre, ' - ', c.cancion) AS display_text
                FROM cancioneschilehero3 c
                INNER JOIN artistas a ON a.id = c.id_artista
                ORDER BY a.nombre, c.cancion;
            ";

            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand(sql, cn))
                using (var da = new MySqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    cn.Open();
                    da.Fill(dt);

                    cb_cancion.DisplayMember = "display_text";
                    cb_cancion.ValueMember = "id";
                    cb_cancion.DataSource = dt;
                    cb_cancion.DropDownStyle = ComboBoxStyle.DropDownList;
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Error(
                    "No se pudieron cargar las canciones.\n\nDetalle: " + ex.Message,
                    "Error",
                    this
                );
            }
        }

        // =========================================================
        //  INSTRUMENTOS
        // =========================================================
        private void SetupInstrumentos()
        {
            if (cb_instrumento == null) return;

            cb_instrumento.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_instrumento.Items.Clear();
            cb_instrumento.Items.Add("Guitarra");
            cb_instrumento.Items.Add("Bajo");
            cb_instrumento.Items.Add("Batería");
            cb_instrumento.Items.Add("Vocal");

            if (cb_instrumento.Items.Count > 0)
                cb_instrumento.SelectedIndex = 0;
        }

        // =========================================================
        //  CAPTURA
        // =========================================================
        private void SetupPictureBox()
        {
            if (pb_captura == null) return;

            pb_captura.BackColor = ColorTranslator.FromHtml("#101018");
            pb_captura.SizeMode = PictureBoxSizeMode.Zoom;
            pb_captura.BorderStyle = BorderStyle.FixedSingle;
        }

        private void btn_subircaptura_Click(object sender, EventArgs e)
        {
            if (openFileDialog1 == null)
            {
                CustomMessageBox.Error(
                    "No se encontró el OpenFileDialog.\n\nAgrega uno al formulario (openFileDialog1).",
                    "Falta componente",
                    this
                );
                return;
            }

            openFileDialog1.Title = "Selecciona tu captura";
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog(this) != DialogResult.OK)
                return;

            string path = openFileDialog1.FileName;

            try
            {
                // Mostrar sin bloquear archivo
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var img = Image.FromStream(fs))
                {
                    var old = pb_captura.Image;
                    pb_captura.Image = new Bitmap(img);
                    old?.Dispose();
                }

                // Guardar Base64
                byte[] bytes = File.ReadAllBytes(path);
                _capturaBase64 = Convert.ToBase64String(bytes);

                CustomMessageBox.Success("Captura cargada correctamente.", "Captura", this);
            }
            catch (Exception ex)
            {
                _capturaBase64 = null;
                CustomMessageBox.Error(
                    "No se pudo cargar la captura.\n\nDetalle: " + ex.Message,
                    "Error",
                    this
                );
            }
        }

        // =========================================================
        //  ENVIAR PUNTAJE (INSERT ranking con id_usuario)
        // =========================================================
        private void btn_enviarpuntaje_Click(object sender, EventArgs e)
        {
            // ✅ asegurar sesión
            if (Session.UserId <= 0)
            {
                CustomMessageBox.Error(
                    "No se detectó sesión de usuario.\n\nVuelve a iniciar sesión.",
                    "Sesión inválida",
                    this
                );
                return;
            }

            if (cb_cancion == null || cb_cancion.SelectedValue == null)
            {
                CustomMessageBox.Warning("Selecciona una canción.", "Faltan datos", this);
                return;
            }

            int idCancion;
            if (!int.TryParse(Convert.ToString(cb_cancion.SelectedValue), out idCancion))
            {
                CustomMessageBox.Error("No se pudo determinar el ID de la canción.", "Error", this);
                return;
            }

            string instrumento = cb_instrumento != null ? Convert.ToString(cb_instrumento.SelectedItem) : null;
            if (string.IsNullOrWhiteSpace(instrumento))
            {
                CustomMessageBox.Warning("Selecciona un instrumento.", "Faltan datos", this);
                return;
            }

            int puntaje;
            if (txt_puntaje == null || !int.TryParse(txt_puntaje.Text.Trim(), out puntaje) || puntaje < 0)
            {
                CustomMessageBox.Warning("Ingresa un puntaje válido (número entero).", "Dato inválido", this);
                return;
            }

            decimal porcentaje;
            string ptxt = (txt_porcentaje != null) ? txt_porcentaje.Text.Trim() : "";
            ptxt = ptxt.Replace(',', '.');

            if (!decimal.TryParse(ptxt, NumberStyles.Number, CultureInfo.InvariantCulture, out porcentaje))
            {
                CustomMessageBox.Warning("Ingresa un porcentaje válido (ej: 98.50).", "Dato inválido", this);
                return;
            }

            if (porcentaje < 0m || porcentaje > 100m)
            {
                CustomMessageBox.Warning("El porcentaje debe estar entre 0 y 100.", "Dato inválido", this);
                return;
            }

            string youtube = txt_youtube != null ? txt_youtube.Text.Trim() : null;
            if (string.IsNullOrWhiteSpace(youtube)) youtube = null;

            InsertarRanking(Session.UserId, idCancion, puntaje, porcentaje, instrumento, _capturaBase64, youtube);
        }

        private void InsertarRanking(int idUsuario, int idCancion, int puntaje, decimal porcentaje,
                                     string instrumento, string capturaBase64, string youtube)
        {
            const string sql = @"
                INSERT INTO ranking
                    (id_usuario, id_cancion, puntuacion, porcentaje, instrumento, fecha_puntuacion, captura, link_youtube)
                VALUES
                    (@id_usuario, @id_cancion, @puntuacion, @porcentaje, @instrumento, @fecha, @captura, @youtube);
            ";

            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@id_usuario", idUsuario);
                    cmd.Parameters.AddWithValue("@id_cancion", idCancion);
                    cmd.Parameters.AddWithValue("@puntuacion", puntaje);
                    cmd.Parameters.AddWithValue("@porcentaje", porcentaje);
                    cmd.Parameters.AddWithValue("@instrumento", instrumento);
                    cmd.Parameters.AddWithValue("@fecha", DateTime.Now);

                    if (string.IsNullOrWhiteSpace(capturaBase64))
                        cmd.Parameters.AddWithValue("@captura", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@captura", capturaBase64);

                    if (string.IsNullOrWhiteSpace(youtube))
                        cmd.Parameters.AddWithValue("@youtube", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@youtube", youtube);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }

                CustomMessageBox.Success(
                    "Puntaje enviado correctamente.\n\nQuedó en estado: En revisión",
                    "Éxito",
                    this
                );
            }
            catch (Exception ex)
            {
                CustomMessageBox.Error(
                    "No se pudo enviar el puntaje.\n\nDetalle: " + ex.Message,
                    "Error",
                    this
                );
            }
        }
    }
}