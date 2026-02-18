using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace chilehero_c
{
    public class CustomMessageBoxForm : Form
    {
        private readonly GradientPanel _bg;
        private readonly Label _lbTitle;
        private readonly Label _lbMessage;

        private readonly GradientButton _btnOk;
        private readonly GradientButton _btnCancel;

        private readonly Panel _content;
        private readonly Panel _footer;

        private readonly Color _yellow = ColorTranslator.FromHtml("#F2C200");
        private readonly Color _yellowFx = ColorTranslator.FromHtml("#FFD54A");
        private readonly Color _text = Color.Gainsboro;

        // Ajustes visuales
        private int _cornerRadius = 10;
        private int _borderSize = 3;

        public CustomMessageBoxForm(string message, string title, CustomMessageBoxType type, bool showCancel)
        {
            // Render
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true;

            // Ventana
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.Black;
            ShowInTaskbar = false;
            TopMost = true;

            // Tamaño
            ClientSize = new Size(520, 220);

            // Deja espacio real para el borde
            Padding = new Padding(_borderSize + 2);

            // Fondo (dentro del padding)
            _bg = new GradientPanel
            {
                Dock = DockStyle.Fill,
                StartColor = ColorTranslator.FromHtml("#01030A"),
                EndColor = ColorTranslator.FromHtml("#040719"),
                Mode = LinearGradientMode.BackwardDiagonal
            };
            Controls.Add(_bg);

            // Footer (agregar primero no es obligatorio, pero ayuda a layout estable)
            _footer = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                BackColor = Color.Transparent,
                Padding = new Padding(18, 0, 18, 16)
            };
            _bg.Controls.Add(_footer);

            // Contenido
            _content = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(18, 16, 18, 10)
            };
            _bg.Controls.Add(_content);

            // ====== MENSAJE (✅ CAMBIO: se agrega ANTES del título) ======
            _lbMessage = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopLeft,
                BackColor = Color.Transparent,

                // ✅ CAMBIO: asegura visibilidad SIEMPRE
                ForeColor = _text,

                // ✅ CAMBIO: mejor render de texto y wrap
                UseCompatibleTextRendering = true,

                Font = new Font("Oxanium", 10.5f, FontStyle.Regular),

                // ✅ CAMBIO: texto con espacios normales + wrap
                Text = (message ?? string.Empty).Trim()
            };

            // ✅ CAMBIO: permite multilínea correctamente (wrap)
            // (para AutoSize=false, el wrap depende del tamaño real del label)
            _lbMessage.Padding = new Padding(0, 6, 0, 0);

            _content.Controls.Add(_lbMessage);

            // ====== TÍTULO ======
            _lbTitle = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 34,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent,
                ForeColor = _yellow,
                UseCompatibleTextRendering = true,
                Font = new Font("Oxanium", 13f, FontStyle.Bold),
                Text = string.IsNullOrWhiteSpace(title) ? "Mensaje" : title.Trim()
            };
            _content.Controls.Add(_lbTitle);

            // ✅ CAMBIO: Fuerza el orden correcto de Dock (Top arriba, Fill abajo)
            // En WinForms el orden afecta MUCHO el layout con Dock.
            _content.Controls.SetChildIndex(_lbTitle, 0);   // arriba
            _content.Controls.SetChildIndex(_lbMessage, 1); // debajo
            _lbTitle.BringToFront();
            _footer.BringToFront();

            // Botón OK
            _btnOk = new GradientButton
            {
                Text = "OK",
                Width = 140,
                Height = 42,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                StartColor = GetButtonStart(type),
                EndColor = GetButtonEnd(type),
                Mode = LinearGradientMode.ForwardDiagonal,
                BorderColor = _yellow,
                BorderHoverColor = _yellowFx,
                BorderRadius = 12,
                BorderSize = 2,
                GlowOnHover = true,
                GlowSize = 8,
                Font = new Font("Oxanium", 11f, FontStyle.Bold),
                ForeColor = Color.White
            };
            _btnOk.Click += (s, e) => { DialogResult = DialogResult.OK; Close(); };

            // Botón Cancel
            _btnCancel = new GradientButton
            {
                Text = "CANCELAR",
                Width = 160,
                Height = 42,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                StartColor = ColorTranslator.FromHtml("#2A2A2A"),
                EndColor = ColorTranslator.FromHtml("#1A1A1A"),
                Mode = LinearGradientMode.ForwardDiagonal,
                BorderColor = _yellow,
                BorderHoverColor = _yellowFx,
                BorderRadius = 12,
                BorderSize = 2,
                GlowOnHover = true,
                GlowSize = 8,
                Font = new Font("Oxanium", 11f, FontStyle.Bold),
                ForeColor = Color.Gainsboro
            };
            _btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            // Botones alineados derecha
            FlowLayoutPanel buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true,
                BackColor = Color.Transparent,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            if (showCancel) buttons.Controls.Add(_btnCancel);
            buttons.Controls.Add(_btnOk);
            _footer.Controls.Add(buttons);

            AcceptButton = _btnOk;
            CancelButton = showCancel ? (IButtonControl)_btnCancel : (IButtonControl)_btnOk;

            // Drag
            _bg.MouseDown += DragForm;
            _lbTitle.MouseDown += DragForm;

            // Redondeo
            ApplyRoundedRegion();

            // ✅ CAMBIO: forzar layout al mostrarse para evitar “texto vacío”
            Shown += (s, e) =>
            {
                ApplyRoundedRegion();

                // fuerza layout final
                _content.PerformLayout();
                _lbMessage.Invalidate();
                Invalidate();
            };
        }

        private Color GetButtonStart(CustomMessageBoxType type)
        {
            switch (type)
            {
                case CustomMessageBoxType.Error: return ColorTranslator.FromHtml("#D12A2A");
                case CustomMessageBoxType.Warning: return ColorTranslator.FromHtml("#C98A00");
                case CustomMessageBoxType.Success: return ColorTranslator.FromHtml("#1FAE5B");
                default: return ColorTranslator.FromHtml("#2A45D1");
            }
        }

        private Color GetButtonEnd(CustomMessageBoxType type)
        {
            switch (type)
            {
                case CustomMessageBoxType.Error: return ColorTranslator.FromHtml("#2A45D1");
                case CustomMessageBoxType.Warning: return ColorTranslator.FromHtml("#2A45D1");
                case CustomMessageBoxType.Success: return ColorTranslator.FromHtml("#2A45D1");
                default: return ColorTranslator.FromHtml("#D12A2A");
            }
        }

        private void ApplyRoundedRegion()
        {
            Rectangle bounds = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);

            using (GraphicsPath path = RoundedRect(bounds, _cornerRadius))
            {
                Region = new Region(path);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            ApplyRoundedRegion();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

            int half = _borderSize / 2;

            Rectangle rect = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            rect.Inflate(-half, -half);

            int radius = Math.Max(2, _cornerRadius - half);

            using (GraphicsPath path = RoundedRect(rect, radius))
            using (Pen pen = new Pen(_yellow, _borderSize))
            {
                pen.Alignment = PenAlignment.Inset;
                e.Graphics.DrawPath(pen, path);
            }
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

        private void DragForm(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            NativeMethods.ReleaseCapture();
            NativeMethods.SendMessage(Handle, 0xA1, new IntPtr(0x2), IntPtr.Zero);
        }

        private static class NativeMethods
        {
            [DllImport("user32.dll")]
            public static extern bool ReleaseCapture();

            [DllImport("user32.dll")]
            public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        }
    }
}