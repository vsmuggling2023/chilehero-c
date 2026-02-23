using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;

namespace chilehero_c
{
    public partial class Form3 : Form
    {
        private GradientPanel bg;
        private readonly string _connectionString;

        private readonly Color _yellow = ColorTranslator.FromHtml("#F2C200");
        private readonly Color _yellowFx = ColorTranslator.FromHtml("#FFD54A");

        private readonly Font _btnFont = new Font("Oxanium", 9.5f, FontStyle.Bold);

        // ✅ DOS progress bars
        private GradientProgressBar _pbLoading;   // arriba: cargar canciones
        private GradientProgressBar _pbDownload;  // abajo: descargas

        // HTTP + cookies (Drive puede requerir confirmación)
        private readonly CookieContainer _cookies = new CookieContainer();
        private readonly HttpClient _http;

        private const int ROW_HEIGHT = 44;
        private const int BTN_MIN_W = 150;
        private const int BTN_MIN_H = 30;

        public Form3(string connectionString)
        {
            InitializeComponent();
            _connectionString = connectionString;

            ClearUnexpectedBackgroundImages();
            BuildBackground();
            SetupProgressBars(); // ✅ ahora crea 2 barras

            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                CookieContainer = _cookies,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            _http = new HttpClient(handler);
            _http.Timeout = TimeSpan.FromMinutes(10);

            this.Load += Form3_Load;
            this.FormClosed += (s, e) => _http.Dispose();
        }

        private async void Form3_Load(object sender, EventArgs e)
        {
            await CargarCancionesAsync();
        }

        // ---------------------------------------------------------
        //  PROGRESS BARS (CUSTOM) - 2 BARRAS
        // ---------------------------------------------------------
        private void SetupProgressBars()
        {
            // ===== Arriba: Cargando Canciones (queda arriba como pediste) =====
            _pbLoading = new GradientProgressBar
            {
                Width = 360,
                Height = 22,

                CornerRadius = 12,
                BorderSize = 2,
                BorderColor = _yellow,
                BackFill = ColorTranslator.FromHtml("#101018"),

                StartColor = ColorTranslator.FromHtml("#D12A2A"),
                EndColor = ColorTranslator.FromHtml("#2A45D1"),

                ShowText = true,
                TextColor = Color.Gainsboro,
                TextOverride = "Cargando...",

                Indeterminate = true,
                MarqueeSpeed = 18,
                MarqueeStep = 10,
                MarqueeWidth = 60,

                Visible = false
            };

            _pbLoading.Parent = bg;
            _pbLoading.Top = 12;
            _pbLoading.Left = (ClientSize.Width - _pbLoading.Width) / 2;
            _pbLoading.Anchor = AnchorStyles.Top;
            _pbLoading.BringToFront();

            // ===== Abajo: Descargas (más grande y abajo como pediste) =====
            _pbDownload = new GradientProgressBar
            {
                Width = 520,  // ✅ MÁS GRANDE
                Height = 28,  // ✅ MÁS ALTA

                CornerRadius = 14,
                BorderSize = 2,
                BorderColor = _yellow,
                BackFill = ColorTranslator.FromHtml("#101018"),

                StartColor = ColorTranslator.FromHtml("#D12A2A"),
                EndColor = ColorTranslator.FromHtml("#2A45D1"),

                ShowText = true,
                TextColor = Color.Gainsboro,
                TextOverride = "0%",

                Indeterminate = false,
                Visible = false
            };

            _pbDownload.Parent = bg;
            PositionDownloadBarBottom();
            _pbDownload.Anchor = AnchorStyles.Bottom;
            _pbDownload.BringToFront();

            // Reposicionar al cambiar tamaño
            this.Resize += (s, e) =>
            {
                if (_pbLoading != null)
                    _pbLoading.Left = (ClientSize.Width - _pbLoading.Width) / 2;

                if (_pbDownload != null)
                    PositionDownloadBarBottom();
            };
        }

        private void PositionDownloadBarBottom()
        {
            if (_pbDownload == null) return;

            int marginBottom = 14;
            _pbDownload.Left = (ClientSize.Width - _pbDownload.Width) / 2;
            _pbDownload.Top = ClientSize.Height - _pbDownload.Height - marginBottom;

            // por si el grid lo tapa
            _pbDownload.BringToFront();
        }

        private void ShowLoading(string text)
        {
            if (_pbLoading != null)
            {
                _pbLoading.TextOverride = string.IsNullOrWhiteSpace(text) ? "Cargando..." : text;
                _pbLoading.Indeterminate = true;
                _pbLoading.Visible = true;
                _pbLoading.BringToFront();
            }

            if (dataGridView1 != null) dataGridView1.Enabled = false;
        }

        private void HideLoading()
        {
            if (_pbLoading != null)
            {
                _pbLoading.Indeterminate = false;
                _pbLoading.Visible = false;
            }

            if (dataGridView1 != null) dataGridView1.Enabled = true;
        }

        private void ShowDownloadProgress()
        {
            if (_pbDownload == null) return;

            PositionDownloadBarBottom();

            _pbDownload.Indeterminate = false;
            _pbDownload.Minimum = 0;
            _pbDownload.Maximum = 100;
            _pbDownload.Value = 0;
            _pbDownload.TextOverride = "0%";
            _pbDownload.Visible = true;
            _pbDownload.BringToFront();

            if (dataGridView1 != null) dataGridView1.Enabled = false;
        }

        private void UpdateDownloadProgress(int percent, string label)
        {
            if (_pbDownload == null) return;

            percent = Math.Max(0, Math.Min(100, percent));
            _pbDownload.Value = percent;
            _pbDownload.TextOverride = string.IsNullOrWhiteSpace(label) ? (percent + "%") : label;
        }

        private void HideDownloadProgress()
        {
            if (_pbDownload != null)
            {
                _pbDownload.Visible = false;
                _pbDownload.Indeterminate = false;
            }

            if (dataGridView1 != null) dataGridView1.Enabled = true;
        }

        // ---------------------------------------------------------
        //  ANTI-LOGO
        // ---------------------------------------------------------
        private void ClearUnexpectedBackgroundImages()
        {
            this.BackgroundImage = null;
            this.BackgroundImageLayout = ImageLayout.None;

            if (dataGridView1 != null)
            {
                dataGridView1.BackgroundImage = null;
                dataGridView1.BackgroundImageLayout = ImageLayout.None;
            }
        }

        // ---------------------------------------------------------
        //  CARGA (ASYNC)
        // ---------------------------------------------------------
        private async Task CargarCancionesAsync()
        {
            ShowLoading("Cargando canciones...");

            try
            {
                DataTable dt = await Task.Run(() => FetchCanciones());

                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = dt;

                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.MultiSelect = false;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                ApplyGridStyle();
                SetupDescargaButtonColumn();
                ForceRowHeights(ROW_HEIGHT);

                dataGridView1.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las canciones: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                HideLoading();
            }
        }

        private DataTable FetchCanciones()
        {
            const string sql = @"
                SELECT 
                    a.nombre AS Artista,
                    c.cancion AS Canción,
                    c.album  AS Álbum,
                    c.year   AS Año,
                    c.descarga AS Descarga
                FROM cancioneschilehero3 c
                INNER JOIN artistas a ON a.id = c.id_artista
                ORDER BY a.nombre, c.cancion;
            ";

            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(sql, cn))
            using (var da = new MySqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                cn.Open();
                da.Fill(dt);
                return dt;
            }
        }

        // ---------------------------------------------------------
        //  GRID STYLE
        // ---------------------------------------------------------
        private void ApplyGridStyle()
        {
            dataGridView1.RowHeadersVisible = false;

            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.BackgroundColor = ColorTranslator.FromHtml("#01030A");
            dataGridView1.BorderStyle = BorderStyle.None;

            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#1A1A1A");
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = _yellow;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Oxanium", 11f, FontStyle.Bold);
            dataGridView1.ColumnHeadersHeight = 38;

            dataGridView1.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#101018");
            dataGridView1.DefaultCellStyle.ForeColor = Color.Gainsboro;
            dataGridView1.DefaultCellStyle.Font = new Font("Oxanium", 10f, FontStyle.Regular);

            dataGridView1.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#2A45D1");
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;

            dataGridView1.GridColor = ColorTranslator.FromHtml("#202024");
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = true;

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;

            dataGridView1.RowTemplate.Height = ROW_HEIGHT;
        }

        private void ForceRowHeights(int height)
        {
            dataGridView1.SuspendLayout();
            try
            {
                foreach (DataGridViewRow r in dataGridView1.Rows)
                {
                    if (!r.IsNewRow) r.Height = height;
                }
            }
            finally
            {
                dataGridView1.ResumeLayout();
            }
        }

        // ---------------------------------------------------------
        //  DESCARGA COMO BOTÓN + PRÓXIMAMENTE
        // ---------------------------------------------------------
        private void SetupDescargaButtonColumn()
        {
            if (dataGridView1.Columns.Count == 0) return;
            if (!dataGridView1.Columns.Contains("Descarga")) return;

            int idx = dataGridView1.Columns["Descarga"].Index;
            dataGridView1.Columns.Remove("Descarga");

            var colBtn = new DataGridViewButtonColumn
            {
                Name = "Descarga",
                HeaderText = "Descarga",
                DataPropertyName = "Descarga",
                UseColumnTextForButtonValue = false,
                FlatStyle = FlatStyle.Flat,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 16,
                MinimumWidth = 180,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };

            dataGridView1.Columns.Insert(idx, colBtn);

            dataGridView1.Columns["Descarga"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns["Descarga"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView1.CellFormatting -= DataGridView1_CellFormatting_Descarga;
            dataGridView1.CellFormatting += DataGridView1_CellFormatting_Descarga;

            dataGridView1.CellPainting -= DataGridView1_CellPainting_Descarga;
            dataGridView1.CellPainting += DataGridView1_CellPainting_Descarga;

            dataGridView1.CellContentClick -= DataGridView1_CellContentClick_Descarga;
            dataGridView1.CellContentClick += DataGridView1_CellContentClick_Descarga;
        }

        private void DataGridView1_CellFormatting_Descarga(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dataGridView1.Columns[e.ColumnIndex].Name != "Descarga") return;

            string raw = Convert.ToString(e.Value);
            bool hasUrl = !string.IsNullOrWhiteSpace(raw);

            e.Value = hasUrl ? "Descargar" : "Próximamente";
            e.FormattingApplied = true;
        }

        private async void DataGridView1_CellContentClick_Descarga(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dataGridView1.Columns[e.ColumnIndex].Name != "Descarga") return;

            string url = Convert.ToString(((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem)?["Descarga"]);

            if (string.IsNullOrWhiteSpace(url))
            {
                CustomMessageBox.Warning("Esta descarga aún no está disponible.\n\nPróximamente.", "Próximamente", this);
                return;
            }

            await DescargarDesdeGoogleDriveAsync(url);
        }

        private void DataGridView1_CellPainting_Descarga(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dataGridView1.Columns[e.ColumnIndex].Name != "Descarga") return;

            e.Handled = true;
            e.PaintBackground(e.CellBounds, true);

            string text = Convert.ToString(e.FormattedValue) ?? "";
            bool isProx = text.Trim().Equals("Próximamente", StringComparison.OrdinalIgnoreCase);

            int padX = 10;
            int padY = 6;

            int w = Math.Max(BTN_MIN_W, e.CellBounds.Width - (padX * 2));
            int h = Math.Max(BTN_MIN_H, e.CellBounds.Height - (padY * 2));

            Rectangle rect = new Rectangle(
                e.CellBounds.X + (e.CellBounds.Width - w) / 2,
                e.CellBounds.Y + (e.CellBounds.Height - h) / 2,
                w,
                h
            );

            int radius = 10;
            int borderSize = 2;

            Color start = isProx ? ColorTranslator.FromHtml("#2A2A2A") : ColorTranslator.FromHtml("#D12A2A");
            Color end = isProx ? ColorTranslator.FromHtml("#1A1A1A") : ColorTranslator.FromHtml("#2A45D1");

            Color border = isProx ? ColorUtils.Darken(_yellow, 0.25f) : _yellow;
            Color textColor = isProx ? Color.Gainsboro : Color.White;

            using (GraphicsPath path = RoundedRect(rect, radius))
            using (LinearGradientBrush brush = new LinearGradientBrush(rect, start, end, LinearGradientMode.ForwardDiagonal))
            using (Pen pen = new Pen(border, borderSize))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                e.Graphics.FillPath(brush, path);
                pen.Alignment = PenAlignment.Inset;
                e.Graphics.DrawPath(pen, path);

                TextRenderer.DrawText(
                    e.Graphics,
                    text,
                    _btnFont,
                    rect,
                    textColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis
                );
            }

            e.Paint(e.CellBounds, DataGridViewPaintParts.Border);
        }

        private GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int maxRadius = Math.Min(bounds.Width, bounds.Height) / 2;
            if (radius > maxRadius) radius = maxRadius;

            int d = radius * 2;
            GraphicsPath path = new GraphicsPath();

            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            return path;
        }

        // ---------------------------------------------------------
        //  DESCARGA DIRECTA GOOGLE DRIVE
        // ---------------------------------------------------------
        private async Task DescargarDesdeGoogleDriveAsync(string driveLink)
        {
            try
            {
                string fileId = ExtractDriveFileId(driveLink);
                if (string.IsNullOrWhiteSpace(fileId))
                {
                    CustomMessageBox.Error("Link de Drive inválido.\n\nNo pude extraer el ID.", "Error", this);
                    return;
                }

                string directUrl = "https://drive.google.com/uc?export=download&id=" + fileId;

                string folder = ChooseFolder();
                if (string.IsNullOrWhiteSpace(folder)) return;

                string tempPath = Path.Combine(folder, "descarga_" + fileId + ".tmp");

                ShowDownloadProgress();
                UpdateDownloadProgress(0, "0%");

                var result = await DownloadDriveWithConfirmAsync(directUrl, tempPath);

                HideDownloadProgress();

                if (!result.Success)
                {
                    CustomMessageBox.Error("No se pudo descargar.\n\n" + result.ErrorMessage, "Descarga fallida", this);
                    return;
                }

                string finalName = string.IsNullOrWhiteSpace(result.FileName) ? ("archivo_" + fileId) : result.FileName;
                string finalPath = EnsureUniquePath(Path.Combine(folder, finalName));

                File.Move(tempPath, finalPath);

                CustomMessageBox.Success("Descarga completada.\n\nGuardado en:\n" + finalPath, "Éxito", this);
            }
            catch (Exception ex)
            {
                HideDownloadProgress();
                CustomMessageBox.Error("Error al descargar.\n\nDetalle: " + ex.Message, "Error", this);
            }
        }

        private string ChooseFolder()
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Selecciona una carpeta para guardar la descarga";
                return (dlg.ShowDialog(this) == DialogResult.OK) ? dlg.SelectedPath : null;
            }
        }

        private string EnsureUniquePath(string path)
        {
            if (!File.Exists(path)) return path;

            string dir = Path.GetDirectoryName(path);
            string name = Path.GetFileNameWithoutExtension(path);
            string ext = Path.GetExtension(path);

            for (int i = 1; i < 9999; i++)
            {
                string candidate = Path.Combine(dir, name + " (" + i + ")" + ext);
                if (!File.Exists(candidate)) return candidate;
            }

            return path;
        }

        private string ExtractDriveFileId(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return null;

            var m1 = Regex.Match(url, @"\/file\/d\/([a-zA-Z0-9_-]+)");
            if (m1.Success) return m1.Groups[1].Value;

            var m2 = Regex.Match(url, @"[?&]id=([a-zA-Z0-9_-]+)");
            if (m2.Success) return m2.Groups[1].Value;

            return null;
        }

        private async Task<(bool Success, string FileName, string ErrorMessage)> DownloadDriveWithConfirmAsync(string url, string destPath)
        {
            string currentUrl = url;

            for (int attempt = 1; attempt <= 3; attempt++)
            {
                using (var resp = await _http.GetAsync(currentUrl, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (IsFileResponse(resp))
                    {
                        string fileName = TryGetFileName(resp) ?? "descarga.bin";
                        await SaveResponseToFileWithProgressAsync(resp, destPath);
                        return (true, fileName, null);
                    }

                    string html = await resp.Content.ReadAsStringAsync();

                    // Sin Contains( , StringComparison) -> IndexOf
                    if (html.IndexOf("accounts.google.com", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        html.IndexOf("Sign in", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return (false, null, "El archivo parece restringido (requiere login/permisos).");
                    }

                    string next = ExtractConfirmDownloadUrlFromHtml(html);
                    if (!string.IsNullOrWhiteSpace(next))
                    {
                        if (next.StartsWith("/"))
                            currentUrl = "https://drive.google.com" + next.Replace("&amp;", "&");
                        else
                            currentUrl = next.Replace("&amp;", "&");

                        continue;
                    }

                    return (false, null, "Drive devolvió una página de confirmación que no pude interpretar.");
                }
            }

            return (false, null, "No se pudo completar la descarga (demasiados intentos).");
        }

        private bool IsFileResponse(HttpResponseMessage resp)
        {
            if (resp.Content.Headers.ContentDisposition != null) return true;

            var mt = resp.Content.Headers.ContentType != null ? resp.Content.Headers.ContentType.MediaType : "";
            if (mt == null) mt = "";

            if (mt.IndexOf("text/html", StringComparison.OrdinalIgnoreCase) < 0) return true;
            return false;
        }

        private string TryGetFileName(HttpResponseMessage resp)
        {
            var cd = resp.Content.Headers.ContentDisposition;
            if (cd == null) return null;

            string name = cd.FileNameStar ?? cd.FileName;
            if (string.IsNullOrWhiteSpace(name)) return null;

            name = name.Trim().Trim('"');
            return SanitizeFileName(name);
        }

        private string SanitizeFileName(string name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name;
        }

        private async Task SaveResponseToFileWithProgressAsync(HttpResponseMessage resp, string path)
        {
            long? total = resp.Content.Headers.ContentLength;

            using (var input = await resp.Content.ReadAsStreamAsync())
            using (var output = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                byte[] buffer = new byte[81920];
                long readTotal = 0;
                int read;

                while ((read = await input.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await output.WriteAsync(buffer, 0, read);
                    readTotal += read;

                    if (total.HasValue && total.Value > 0)
                    {
                        int percent = (int)(readTotal * 100L / total.Value);
                        UpdateDownloadProgress(percent, null);
                    }
                    else
                    {
                        UpdateDownloadProgress(0, "Descargando...");
                    }
                }

                UpdateDownloadProgress(100, "100%");
            }
        }

        private string ExtractConfirmDownloadUrlFromHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return null;

            var mHref = Regex.Match(html, "href=\"(?<u>/uc\\?[^\\\"]+)\"", RegexOptions.IgnoreCase);
            if (mHref.Success) return WebUtility.HtmlDecode(mHref.Groups["u"].Value);

            if (html.IndexOf("download-form", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                var sb = new StringBuilder("https://drive.usercontent.google.com/download");
                bool first = true;

                foreach (Match m in Regex.Matches(html, "<input[^>]*type=\"hidden\"[^>]*>", RegexOptions.IgnoreCase))
                {
                    var name = Regex.Match(m.Value, "name=\"(?<n>[^\"]+)\"", RegexOptions.IgnoreCase).Groups["n"].Value;
                    var value = Regex.Match(m.Value, "value=\"(?<v>[^\"]*)\"", RegexOptions.IgnoreCase).Groups["v"].Value;

                    if (string.IsNullOrEmpty(name)) continue;

                    sb.Append(first ? "?" : "&")
                      .Append(WebUtility.UrlEncode(name))
                      .Append("=")
                      .Append(WebUtility.UrlEncode(WebUtility.HtmlDecode(value)));

                    first = false;
                }

                if (!first) return sb.ToString();
            }

            return null;
        }

        // ---------------------------------------------------------
        //  BACKGROUND
        // ---------------------------------------------------------
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

            if (dataGridView1 != null)
            {
                var oldBounds = dataGridView1.Bounds;
                dataGridView1.Parent = bg;
                dataGridView1.Bounds = oldBounds;
                dataGridView1.BringToFront();
            }

            bg.Invalidate();
        }
    }
}