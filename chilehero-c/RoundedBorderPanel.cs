using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace chilehero_c
{
    public class RoundedBorderPanel : Panel
    {
        public int BorderRadius { get; set; } = 12;
        public int BorderSize { get; set; } = 2;
        public Color BorderColor { get; set; } = ColorTranslator.FromHtml("#F2C200");
        public Color FillColor { get; set; } = ColorTranslator.FromHtml("#202024");
        public Color BorderFocusColor { get; set; } = ColorTranslator.FromHtml("#FFD54A");

        private bool _focused;

        public RoundedBorderPanel()
        {
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.Padding = new Padding(12, 10, 12, 10);
            this.BackColor = Color.Transparent; // se dibuja a mano
        }

        public void SetFocused(bool focused)
        {
            _focused = focused;
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = this.ClientRectangle;
            rect.Width -= 1;
            rect.Height -= 1;

            using (GraphicsPath path = GetRoundedRectPath(rect, BorderRadius))
            {
                // Relleno
                using (SolidBrush brush = new SolidBrush(FillColor))
                {
                    e.Graphics.FillPath(brush, path);
                }

                // Borde
                Color bColor = _focused ? BorderFocusColor : BorderColor;
                using (Pen pen = new Pen(bColor, BorderSize))
                {
                    pen.Alignment = PenAlignment.Inset;
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            if (radius <= 0)
            {
                path.AddRectangle(rect);
                path.CloseFigure();
                return path;
            }

            int d = radius * 2;
            Rectangle arc = new Rectangle(rect.Location, new Size(d, d));

            // Top-left
            path.AddArc(arc, 180, 90);

            // Top-right
            arc.X = rect.Right - d;
            path.AddArc(arc, 270, 90);

            // Bottom-right
            arc.Y = rect.Bottom - d;
            path.AddArc(arc, 0, 90);

            // Bottom-left
            arc.X = rect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }
    }
}