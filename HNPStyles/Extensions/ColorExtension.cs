using System;
using System.Windows.Media;

namespace HNPStyles
{
    public static class ColorExtension
    {
        public static Color Add(in this Color color1, in Color color2) => Color.FromRgb(CoerceByte(color1.R + color2.R), CoerceByte(color1.G + color2.G), CoerceByte(color1.B + color2.B));
        public static Color Add(in this Color color1, in double value) => Color.FromRgb(CoerceByte(color1.R + value), CoerceByte(color1.G + value), CoerceByte(color1.B + value));
        public static Color Sub(in this Color color1, in Color color2) => Color.FromRgb(CoerceByte(color1.R - color2.R), CoerceByte(color1.G - color2.G), CoerceByte(color1.B - color2.B));
        public static Color Sub(in this Color color1, in double value) => Color.FromRgb(CoerceByte(color1.R - value), CoerceByte(color1.G - value), CoerceByte(color1.B - value));
        public static Color Mul(in this Color color1, in double value) => Color.FromRgb(CoerceByte(color1.R * value), CoerceByte(color1.G * value), CoerceByte(color1.B * value));
        public static Color Div(in this Color color1, in double value) => Color.FromRgb(CoerceByte(color1.R / value), CoerceByte(color1.G / value), CoerceByte(color1.B / value));
        public static byte CoerceByte(double value) => (byte)(value < 0 ? 0 : value > 255 ? 255 : Math.Round(value));
    }
}
