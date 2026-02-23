using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace chilehero_c
{
    public class GradientProgressBar : Control
    {
        private int _minimum = 0;
        private int _maximum = 100;
        private int _value = 0;

        private bool _indeterminate = false;
        private int _marqueeSpeed = 18;      // ms por tick (menor = más rápido)
        private int _marqueeStep = 8;        // px por tick
        private int _marqueeWidth = 45;      // ancho del “bloque” en indeterminado
        private int _marqueeOffset = 0;
        private Timer _timer;

        // Estilo visual
        private int _cornerRadius = 12;
        private int _borderSize = 2;

        private Color _backFill = ColorTranslator.FromHtml("#101018");
        private Color _borderColor = ColorTranslator.FromHtml("#F2C200");

        private Color _startColor = ColorTranslator.FromHtml("#D12A2A"); // como btn_iniciasesion
        private Color _endColor = ColorTranslator.FromHtml("#2A45D1");

        private bool _showText = true;
        private string _textOverride = "";  // si lo dejas vacío usa porcentaje o “Cargando...”
        private Color _textColor = Color.Gainsboro;

        public GradientProgressBar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw, true);

            DoubleBuffered = true;
            Size = new Size(280, 18);
            Font = new Font("Oxanium", 9f, FontStyle.Bold);
        }

        // -----------------------
        //  PROPIEDADES
        // -----------------------
        [Category("Behavior")]
        public int Minimum
        {
            get => _minimum;
            set
            {
                _minimum = value;
                if (_maximum < _minimum) _maximum = _minimum;
                if (_value < _minimum) _value = _minimum;
                Invalidate();
            }
        }

        [Category("Behavior")]
        public int Maximum
        {
            get => _maximum;
            set
            {
                _maximum = Math.Max(value, _minimum);
                if (_value > _maximum) _value = _maximum;
                Invalidate();
            }
        }

        [Category("Behavior")]
        public int Value
        {
            get => _value;
            set
            {
                _value = Math.Max(_minimum, Math.Min(_maximum, value));
                Invalidate();
            }
        }

        [Category("Behavior")]
        public bool Indeterminate
        {
            get => _indeterminate;
            set
            {
                _indeterminate = value;
                ConfigureTimer();
                Invalidate();
            }
        }

        [Category("Behavior")]
        public int MarqueeSpeed
        {
            get => _marqueeSpeed;
            set
            {
                _marqueeSpeed = Math.Max(5, value);
                if (_timer != null) _timer.Interval = _marqueeSpeed;
            }
        }

        [Category("Behavior")]
        public int MarqueeStep
        {
            get => _marqueeStep;
            set => _marqueeStep = Math.Max(1, value);
        }

        [Category("Behavior")]
        public int MarqueeWidth
        {
            get => _marqueeWidth;
            set => _marqueeWidth = Math.Max(10, value);
        }

        [Category("Appearance")]
        public int CornerRadius
        {
            get => _cornerRadius;
            set
            {
                _cornerRadius = Math.Max(0, value);
                Invalidate();
            }
        }

        [Category("Appearance")]
        public int BorderSize
        {
            get => _borderSize;
            set
            {
                _borderSize = Math.Max(0, value);
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color BackFill
        {
            get => _backFill;
            set { _backFill = value; Invalidate(); }
        }

        [Category("Appearance")]
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        public Color StartColor
        {
            get => _startColor;
            set { _startColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        public Color EndColor
        {
            get => _endColor;
            set { _endColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        public bool ShowText
        {
            get => _showText;
            set { _showText = value; Invalidate(); }
        }

        [Category("Appearance")]
        public string TextOverride
        {
            get => _textOverride;
            set { _textOverride = value ?? ""; Invalidate(); }
        }

        [Category("Appearance")]
        public Color TextColor
        {
            get => _textColor;
            set { _textColor = value; Invalidate(); }
        }

        // -----------------------
        //  TIMER (MARQUEE)
        // -----------------------
        private void ConfigureTimer()
        {
            if (_indeterminate)
            {
                if (_timer == null)
                {
                    _timer = new Timer();
                    _timer.Interval = _marqueeSpeed;
                    _timer.Tick += (s, e) =>
                    {
                        _marqueeOffset += _marqueeStep;
                        if (_marqueeOffset > Width + _marqueeWidth) _marqueeOffset = -_marqueeWidth;
                        Invalidate();
                    };
                }
                _timer.Start();
            }
            else
            {
                _timer?.Stop();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer?.Stop();
                _timer?.Dispose();
                _timer = null;
            }
            base.Dispose(disposing);
        }

        // -----------------------
        //  DIBUJO
        // -----------------------
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Rectangle rect = ClientRectangle;
            if (rect.Width <= 0 || rect.Height <= 0) return;

            // margen para borde
            int half = _borderSize / 2;
            rect.Inflate(-half, -half);

            int radius = Math.Max(0, _cornerRadius - half);

            using (GraphicsPath bgPath = RoundedRect(rect, radius))
            {
                // Fondo
                using (SolidBrush backBrush = new SolidBrush(_backFill))
                    e.Graphics.FillPath(backBrush, bgPath);

                // Progreso
                Rectangle fillRect = rect;
                fillRect.Inflate(-_borderSize, -_borderSize);

                if (_indeterminate)
                {
                    // bloque animado
                    int blockX = fillRect.X + _marqueeOffset;
                    Rectangle block = new Rectangle(blockX, fillRect.Y, _marqueeWidth, fillRect.Height);

                    // recortar dentro del rect
                    Rectangle clipped = Rectangle.Intersect(block, fillRect);
                    if (clipped.Width > 0)
                    {
                        using (GraphicsPath fillPath = RoundedRect(fillRect, Math.Max(0, radius - 2)))
                        using (Region clipRegion = new Region(fillPath))
                        {
                            e.Graphics.SetClip(clipRegion, CombineMode.Replace);
                            using (var brush = new LinearGradientBrush(clipped, _startColor, _endColor, LinearGradientMode.ForwardDiagonal))
                                e.Graphics.FillRectangle(brush, clipped);
                            e.Graphics.ResetClip();
                        }
                    }
                }
                else
                {
                    float range = Math.Max(1, _maximum - _minimum);
                    float percent = (_value - _minimum) / range;
                    percent = Math.Max(0f, Math.Min(1f, percent));

                    int fillWidth = (int)Math.Round(fillRect.Width * percent);
                    if (fillWidth > 0)
                    {
                        Rectangle progress = new Rectangle(fillRect.X, fillRect.Y, fillWidth, fillRect.Height);

                        using (GraphicsPath fillPath = RoundedRect(fillRect, Math.Max(0, radius - 2)))
                        using (Region clipRegion = new Region(fillPath))
                        {
                            e.Graphics.SetClip(clipRegion, CombineMode.Replace);
                            using (var brush = new LinearGradientBrush(progress, _startColor, _endColor, LinearGradientMode.ForwardDiagonal))
                                e.Graphics.FillRectangle(brush, progress);
                            e.Graphics.ResetClip();
                        }
                    }
                }

                // Borde
                if (_borderSize > 0)
                {
                    using (Pen p = new Pen(_borderColor, _borderSize))
                    {
                        p.Alignment = PenAlignment.Inset;
                        e.Graphics.DrawPath(p, bgPath);
                    }
                }

                // Texto
                if (_showText)
                {
                    string text;
                    if (!string.IsNullOrWhiteSpace(_textOverride))
                    {
                        text = _textOverride;
                    }
                    else if (_indeterminate)
                    {
                        text = "Cargando...";
                    }
                    else
                    {
                        float range = Math.Max(1, _maximum - _minimum);
                        float percent = (_value - _minimum) / range;
                        percent = Math.Max(0f, Math.Min(1f, percent));
                        text = $"{(int)Math.Round(percent * 100)}%";
                    }

                    TextRenderer.DrawText(
                        e.Graphics,
                        text,
                        Font,
                        ClientRectangle,
                        _textColor,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding
                    );
                }
            }
        }

        private GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int r = Math.Max(0, radius);
            int d = r * 2;

            GraphicsPath path = new GraphicsPath();
            if (r == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}