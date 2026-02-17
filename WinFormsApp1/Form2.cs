using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;

namespace WinFormsApp1
{
    public partial class Form2 : Form
    {
        private readonly string _connectionString;
        private readonly string _rol;

        private DataTable _dtCanciones;
        private readonly BindingSource _bsCanciones = new BindingSource();

        private bool _cerrandoPorLogout = false;

        private bool _descargando = false;

        private System.Windows.Forms.Timer _clearProgressTimer;

        public Form2(string connectionString, string rol)
        {
            InitializeComponent();

            _connectionString = connectionString;
            _rol = string.IsNullOrWhiteSpace(rol) ? "Usuario" : rol;

            StartPosition = FormStartPosition.CenterScreen;

            Load += Form2_Load;

            acercaDeToolStripMenuItem.Click += AcercaDeToolStripMenuItem_Click;
            cerrarSesiónToolStripMenuItem.Click += CerrarSesiónToolStripMenuItem_Click;
            salirToolStripMenuItem.Click += SalirToolStripMenuItem_Click;

            usuariosToolStripMenuItem.Click += UsuariosToolStripMenuItem_Click;
            baneadosToolStripMenuItem.Click += BaneadosToolStripMenuItem_Click;
            cancionesToolStripMenuItem.Click += CancionesToolStripMenuItem_Click;

            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;

            btn_buscar.Click += btn_buscar_Click;
            txt_buscar.KeyDown += txt_buscar_KeyDown;

            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
            progressBar1.Style = ProgressBarStyle.Blocks;
            lb_progress.Text = "";
            lb_progress.Visible = true;

            lb_busqueda.Text = "Búsqueda:";
            ConfigurarLayoutResponsive_Form2();
        }

        private void ConfigurarLayoutResponsive_Form2()
        {
            this.AutoScaleMode = AutoScaleMode.Font;
            this.MinimumSize = new System.Drawing.Size(900, 600);

            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            txt_buscar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btn_buscar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lb_busqueda.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            progressBar1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lb_progress.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Text = $"Lista de Canciones | Chile Hero - Rol: {_rol}";

            ConfigurarPermisosGestion();

            CargarCanciones();
        }

        private void ConfigurarPermisosGestion()
        {
            bool esAdminODueno =
                _rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase) ||
                _rol.Equals("Dueño", StringComparison.OrdinalIgnoreCase);

            gestiónToolStripMenuItem.Visible = esAdminODueno;
        }

        private bool TienePermisoGestion()
        {
            return _rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase)
                || _rol.Equals("Dueño", StringComparison.OrdinalIgnoreCase);
        }

        private void UsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!TienePermisoGestion())
            {
                MessageBox.Show("No tienes permisos para acceder a Gestión.", "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var f = new Form3(_connectionString, _rol, soloBaneados: false);
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog(this);
        }

        private void BaneadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!TienePermisoGestion())
            {
                MessageBox.Show("No tienes permisos para acceder a Gestión.", "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var f = new Form3(_connectionString, _rol, soloBaneados: true);
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog(this);
        }

        private void CancionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CargarCanciones();
        }

        private void CargarCanciones()
        {
            const string sql = @"
                SELECT 
                    c.id AS id,
                    a.nombre AS Artista,
                    c.cancion AS Canción,
                    c.album  AS Álbum,
                    c.year   AS Año,
                    c.descarga AS Descarga
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

                    _dtCanciones = dt;
                    _bsCanciones.DataSource = _dtCanciones;

                    dataGridView1.AutoGenerateColumns = true;
                    dataGridView1.DataSource = _bsCanciones;

                    // Convertir Descarga a LinkColumn
                    ConfigurarColumnaDescargaComoLink();

                    // Config grid
                    dataGridView1.ReadOnly = true;
                    dataGridView1.AllowUserToAddRows = false;
                    dataGridView1.AllowUserToDeleteRows = false;
                    dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dataGridView1.MultiSelect = false;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    lb_busqueda.Text = $"Búsqueda: (Total: {_bsCanciones.Count})";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las canciones: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColumnaDescargaComoLink()
        {
            if (!dataGridView1.Columns.Contains("Descarga"))
                return;

            if (dataGridView1.Columns["Descarga"] is DataGridViewLinkColumn)
                return;

            int idx = dataGridView1.Columns["Descarga"].Index;
            dataGridView1.Columns.Remove("Descarga");

            var linkCol = new DataGridViewLinkColumn
            {
                Name = "Descarga",
                HeaderText = "Descarga",
                DataPropertyName = "Descarga",
                LinkBehavior = LinkBehavior.HoverUnderline,
                TrackVisitedState = true
            };

            dataGridView1.Columns.Insert(idx, linkCol);
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dataGridView1.Columns[e.ColumnIndex].Name != "Descarga") return;

            var raw = e.Value?.ToString();

            if (string.IsNullOrWhiteSpace(raw))
            {
                e.Value = "Próximamente";
                e.FormattingApplied = true;

                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewLinkCell linkCell)
                {
                    linkCell.LinkColor = Color.Gray;
                    linkCell.ActiveLinkColor = Color.Gray;
                    linkCell.VisitedLinkColor = Color.Gray;
                }
            }
            else
            {
                e.Value = "Descargar";
                e.FormattingApplied = true;

                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewLinkCell linkCell)
                {
                    linkCell.LinkColor = Color.Blue;
                    linkCell.ActiveLinkColor = Color.Red;
                    linkCell.VisitedLinkColor = Color.Purple;
                }
            }
        }

        private void btn_buscar_Click(object sender, EventArgs e)
        {
            AplicarFiltro(txt_buscar.Text);
        }

        private void txt_buscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                AplicarFiltro(txt_buscar.Text);
            }
        }

        private void AplicarFiltro(string texto)
        {
            if (_dtCanciones == null) return;

            texto = (texto ?? "").Trim();

            if (texto.Length == 0)
            {
                _bsCanciones.RemoveFilter();
                lb_busqueda.Text = $"Búsqueda: (Total: {_bsCanciones.Count})";
                return;
            }

            string safe = EscapeRowFilterValue(texto);

            _bsCanciones.Filter =
                $"[Artista] LIKE '%{safe}%'" +
                $" OR [Canción] LIKE '%{safe}%'" +
                $" OR [Álbum] LIKE '%{safe}%'" +
                $" OR CONVERT([Año], 'System.String') LIKE '%{safe}%'" +
                $" OR [Descarga] LIKE '%{safe}%'";

            lb_busqueda.Text = $"Búsqueda: {texto} ({_bsCanciones.Count} resultados)";
        }

        private static string EscapeRowFilterValue(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value
                .Replace("'", "''")
                .Replace("[", "[[]")
                .Replace("]", "[]]");
        }

        private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dataGridView1.Columns[e.ColumnIndex].Name != "Descarga") return;
            if (_descargando) return;

            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is not DataRowView rowView)
                return;

            var driveLink = rowView["Descarga"]?.ToString();

            if (string.IsNullOrWhiteSpace(driveLink))
            {
                MessageBox.Show("Próximamente 🙂", "Descarga",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _descargando = true;
            dataGridView1.Enabled = false;
            btn_buscar.Enabled = false;
            txt_buscar.Enabled = false;

            try
            {
                await DescargarDriveConNombreYProgresoAsync(driveLink.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al descargar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LimpiarProgresoUI();
            }
            finally
            {
                dataGridView1.Enabled = true;
                btn_buscar.Enabled = true;
                txt_buscar.Enabled = true;
                _descargando = false;
            }
        }

        private async Task DescargarDriveConNombreYProgresoAsync(string driveLinkOrId)
        {
            var fileId = ExtraerGoogleDriveFileId(driveLinkOrId);
            if (string.IsNullOrWhiteSpace(fileId))
                throw new Exception("No se pudo detectar el ID del archivo de Google Drive.");

            var directUrl = $"https://drive.google.com/uc?export=download&id={fileId}";

            var cookies = new CookieContainer();
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                UseCookies = true,
                CookieContainer = cookies,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            using var http = new HttpClient(handler);
            http.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; WinFormsApp)");

            using var resp1 = await http.GetAsync(directUrl, HttpCompletionOption.ResponseHeadersRead);

            if (EsRespuestaArchivo(resp1))
            {
                var suggestedName = ExtraerNombreDesdeHeaders(resp1) ?? "descarga";
                await GuardarConDialogoYProgresoAsync(resp1, suggestedName);
                return;
            }

            var html = await resp1.Content.ReadAsStringAsync();
            var token = ExtraerConfirmToken(html);

            if (string.IsNullOrWhiteSpace(token))
                throw new Exception("Google Drive pidió confirmación o permisos. Verifica que el archivo sea público (Cualquiera con el enlace).");

            var confirmUrl = AñadirConfirmToken(directUrl, token);

            using var resp2 = await http.GetAsync(confirmUrl, HttpCompletionOption.ResponseHeadersRead);

            if (!resp2.IsSuccessStatusCode)
                throw new Exception($"No se pudo descargar. Estado: {(int)resp2.StatusCode} {resp2.ReasonPhrase}");

            if (!EsRespuestaArchivo(resp2))
                throw new Exception("No se recibió el archivo (posible permiso denegado o link inválido).");

            var suggestedName2 = ExtraerNombreDesdeHeaders(resp2) ?? "descarga";
            await GuardarConDialogoYProgresoAsync(resp2, suggestedName2);
        }

        private async Task GuardarConDialogoYProgresoAsync(HttpResponseMessage resp, string suggestedName)
        {
            suggestedName = LimpiarNombreArchivo(suggestedName);

            using var sfd = new SaveFileDialog
            {
                Title = "Guardar archivo",
                FileName = suggestedName,
                Filter = "Todos los archivos (*.*)|*.*"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            long? totalBytes = resp.Content.Headers.ContentLength;

            PrepararProgresoUI();

            await using var input = await resp.Content.ReadAsStreamAsync();
            await using var output = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write, FileShare.None);

            var buffer = new byte[81920];
            long totalRead = 0;
            int read;

            while ((read = await input.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                await output.WriteAsync(buffer, 0, read);
                totalRead += read;

                ActualizarProgresoUI(totalBytes, totalRead);
            }

            MostrarCompletadoYLimpiarLuego(5000);
        }

        private void PrepararProgresoUI()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(PrepararProgresoUI));
                return;
            }

            DetenerTimerLimpieza();

            lb_progress.Visible = true;
            lb_progress.Text = "Descargando...";
            progressBar1.Style = ProgressBarStyle.Blocks;
            progressBar1.Value = 0;
        }

        private void ActualizarProgresoUI(long? totalBytes, long totalRead)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => ActualizarProgresoUI(totalBytes, totalRead)));
                return;
            }

            if (totalBytes.HasValue && totalBytes.Value > 0)
            {
                int percent = (int)Math.Min(100, (totalRead * 100L) / totalBytes.Value);
                progressBar1.Style = ProgressBarStyle.Blocks;
                progressBar1.Value = percent;
                lb_progress.Text = $"Descargando... {percent}%";
            }
            else
            {
                progressBar1.Style = ProgressBarStyle.Marquee;
                lb_progress.Text = "Descargando...";
            }
        }

        private void MostrarCompletadoYLimpiarLuego(int milisegundos)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => MostrarCompletadoYLimpiarLuego(milisegundos)));
                return;
            }

            progressBar1.Style = ProgressBarStyle.Blocks;
            progressBar1.Value = 100;
            lb_progress.Visible = true;
            lb_progress.Text = "✅ Descarga completada";

            DetenerTimerLimpieza();

            _clearProgressTimer = new System.Windows.Forms.Timer();
            _clearProgressTimer.Interval = milisegundos;
            _clearProgressTimer.Tick += (s, e) =>
            {
                _clearProgressTimer.Stop();
                _clearProgressTimer.Dispose();
                _clearProgressTimer = null;

                LimpiarProgresoUI();
            };
            _clearProgressTimer.Start();
        }

        private void LimpiarProgresoUI()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(LimpiarProgresoUI));
                return;
            }

            progressBar1.Style = ProgressBarStyle.Blocks;
            progressBar1.Value = 0;
            lb_progress.Text = "";
            lb_progress.Visible = true;
        }

        private void DetenerTimerLimpieza()
        {
            _clearProgressTimer?.Stop();
            _clearProgressTimer?.Dispose();
            _clearProgressTimer = null;
        }

        private static bool EsRespuestaArchivo(HttpResponseMessage resp)
        {
            var ct = resp.Content.Headers.ContentType?.MediaType?.ToLowerInvariant();
            var cd = resp.Content.Headers.ContentDisposition?.DispositionType?.ToLowerInvariant();

            if (cd == "attachment") return true;
            if (ct == null) return false;
            if (ct.Contains("text/html")) return false;

            return true;
        }

        private static string ExtraerNombreDesdeHeaders(HttpResponseMessage resp)
        {
            var cd = resp.Content.Headers.ContentDisposition;

            var nameStar = cd?.FileNameStar;
            if (!string.IsNullOrWhiteSpace(nameStar))
                return nameStar;

            var name = cd?.FileName;
            if (!string.IsNullOrWhiteSpace(name))
                return name;

            return null;
        }

        private static string LimpiarNombreArchivo(string name)
        {
            name = (name ?? "descarga").Trim().Trim('"');

            try { name = Uri.UnescapeDataString(name); } catch { }

            foreach (var c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');

            return string.IsNullOrWhiteSpace(name) ? "descarga" : name;
        }

        private static string ExtraerConfirmToken(string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return null;

            var m = Regex.Match(html, @"confirm=([0-9A-Za-z_]+)", RegexOptions.IgnoreCase);
            if (m.Success) return m.Groups[1].Value;

            var m2 = Regex.Match(html, @"download_warning[^=]*=([0-9A-Za-z_]+)", RegexOptions.IgnoreCase);
            if (m2.Success) return m2.Groups[1].Value;

            return null;
        }

        private static string AñadirConfirmToken(string url, string token)
        {
            if (url.Contains("confirm=")) return url;
            return url.Contains("?") ? $"{url}&confirm={token}" : $"{url}?confirm={token}";
        }

        private static string ExtraerGoogleDriveFileId(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;

            if (!input.Contains("/") && !input.Contains("?") && input.Length >= 10)
                return input.Trim();

            var m1 = Regex.Match(input, @"drive\.google\.com\/file\/d\/([^\/\?]+)", RegexOptions.IgnoreCase);
            if (m1.Success) return m1.Groups[1].Value;

            var m2 = Regex.Match(input, @"[?&]id=([^&]+)", RegexOptions.IgnoreCase);
            if (m2.Success) return m2.Groups[1].Value;

            return null;
        }

        private void AcercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Chile Hero 3 v1.0\nRol actual: {_rol}", "Acerca de",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _cerrandoPorLogout = true;

            var login = new Form1();
            login.Show();
            Close();
        }

        private void SalirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            DetenerTimerLimpieza();

            if (!_cerrandoPorLogout)
                Application.Exit();
        }
    }
}