using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace chilehero_c
{
    public static class ColorUtils
    {
        public static Color FromHex(string hex) => ColorTranslator.FromHtml(hex);

        private static float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static Color Mix(params Color[] colors)
        {
            if (colors == null || colors.Length == 0)
                throw new ArgumentException("Debes pasar al menos un color.");

            int r = 0, g = 0, b = 0;
            foreach (var c in colors)
            {
                r += c.R;
                g += c.G;
                b += c.B;
            }

            return Color.FromArgb(r / colors.Length, g / colors.Length, b / colors.Length);
        }

        public static Color Lighten(Color c, float factor)
        {
            factor = Clamp(factor, 0f, 1f);
            int r = (int)Math.Round(c.R + (255 - c.R) * factor);
            int g = (int)Math.Round(c.G + (255 - c.G) * factor);
            int b = (int)Math.Round(c.B + (255 - c.B) * factor);
            return Color.FromArgb(r, g, b);
        }

        public static Color Darken(Color c, float factor)
        {
            factor = Clamp(factor, 0f, 1f);
            int r = (int)Math.Round(c.R * (1 - factor));
            int g = (int)Math.Round(c.G * (1 - factor));
            int b = (int)Math.Round(c.B * (1 - factor));
            return Color.FromArgb(r, g, b);
        }
    }
}
