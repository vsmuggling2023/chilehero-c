using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace chilehero_c
{
    public class GradientButton : Button
    {
        public Color StartColor { get; set; } = ColorTranslator.FromHtml("#D12A2A");
        public Color EndColor { get; set; } = ColorTranslator.FromHtml("#2A45D1");
        public LinearGradientMode Mode { get; set; } = LinearGradientMode.Horizontal;

        public Color BorderColor { get; set; } = ColorTranslator.FromHtml("#F2C200");
        public Color BorderHoverColor { get; set; } = ColorTranslator.FromHtml("#FFD54A");
        public int BorderRadius { get; set; } = 12;
        public int BorderSize { get; set; } = 2;

        public bool GlowOnHover { get; set; } = true;
        public int GlowSize { get; set; } = 7;

        public Image LeftIcon { get; set; }
        public Image RightIcon { get; set; }
        public int IconSize { get; set; } = 18;
        public int IconSpacing { get; set; } = 10;

        private bool _hover;
        private bool _focus;

        public GradientButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            DoubleBuffered = true;

            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = Color.Transparent;

            ForeColor = Color.White;
            Cursor = Cursors.Hand;

            TextAlign = ContentAlignment.MiddleCenter;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _hover = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hover = false;
            Invalidate();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            _focus = true;
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            _focus = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(Parent != null ? Parent.BackColor : Color.Black);

            Rectangle rect = ClientRectangle;
            rect.Inflate(-1, -1);

            using (GraphicsPath path = RoundedRect(rect, BorderRadius))
            {
                using (var brush = new LinearGradientBrush(rect, StartColor, EndColor, Mode))
                {
                    e.Graphics.FillPath(brush, path);
                }

                bool fx = GlowOnHover && (_hover || _focus);
                if (fx)
                {
                    Color glowColor = BorderHoverColor;
                    for (int i = 1; i <= GlowSize; i++)
                    {
                        int alpha = Math.Max(6, 40 - i * 4);
                        using (var glowPen = new Pen(Color.FromArgb(alpha, glowColor), BorderSize + i))
                        {
                            glowPen.Alignment = PenAlignment.Center;
                            e.Graphics.DrawPath(glowPen, path);
                        }
                    }
                }

                Color b = (_hover || _focus) ? BorderHoverColor : BorderColor;
                using (var pen = new Pen(b, BorderSize))
                {
                    pen.Alignment = PenAlignment.Inset;
                    e.Graphics.DrawPath(pen, path);
                }

                DrawContent(e.Graphics, rect);
            }
        }

        private void DrawContent(Graphics g, Rectangle rect)
        {
            Size textSize = TextRenderer.MeasureText(Text, Font);

            bool hasLeft = LeftIcon != null;
            bool hasRight = RightIcon != null;

            int contentWidth = textSize.Width;
            if (hasLeft) contentWidth += IconSize + IconSpacing;
            if (hasRight) contentWidth += IconSize + IconSpacing;

            int x = rect.X + (rect.Width - contentWidth) / 2;
            int cy = rect.Y + rect.Height / 2;

            if (hasLeft)
            {
                Rectangle r = new Rectangle(x, cy - IconSize / 2, IconSize, IconSize);
                g.DrawImage(LeftIcon, r);
                x += IconSize + IconSpacing;
            }

            Rectangle textRect = new Rectangle(x, rect.Y, textSize.Width + 6, rect.Height);
            TextRenderer.DrawText(
                g,
                Text,
                Font,
                textRect,
                ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.NoPadding
            );

            x += textSize.Width + IconSpacing;

            if (hasRight)
            {
                Rectangle r = new Rectangle(x, cy - IconSize / 2, IconSize, IconSize);
                g.DrawImage(RightIcon, r);
            }
        }

        private GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
