using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace chilehero_c
{
    public class GradientPanel : Panel
    {
        public Color StartColor { get; set; } = ColorTranslator.FromHtml("#01030A");
        public Color EndColor { get; set; } = ColorTranslator.FromHtml("#040719");
        public LinearGradientMode Mode { get; set; } = LinearGradientMode.BackwardDiagonal;

        public GradientPanel()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            Rectangle rect = ClientRectangle;
            if (rect.Width <= 0 || rect.Height <= 0) return;

            using (var brush = new LinearGradientBrush(rect, StartColor, EndColor, Mode))
            {
                e.Graphics.FillRectangle(brush, rect);
            }
        }
    }
}
