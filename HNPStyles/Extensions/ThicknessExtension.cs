using System;
using System.Windows;

namespace HNPStyles
{
    public static class ThicknessExtension
    {
        public static Thickness Add(in this Thickness thickness1, in Thickness thickness2) => new(thickness1.Left + thickness2.Left, thickness1.Top + thickness2.Top, thickness1.Right + thickness2.Right, thickness1.Bottom + thickness2.Bottom);
        public static Thickness Add(in this Thickness thickness1, in double value) => new(thickness1.Left + value, thickness1.Top + value, thickness1.Right + value, thickness1.Bottom + value);
        public static Thickness Sub(in this Thickness thickness1, in Thickness thickness2) => new(thickness1.Left - thickness2.Left, thickness1.Top - thickness2.Top, thickness1.Right - thickness2.Right, thickness1.Bottom - thickness2.Bottom);
        public static Thickness Sub(in this Thickness thickness1, in double value) => new(thickness1.Left - value, thickness1.Top - value, thickness1.Right - value, thickness1.Bottom - value);
        public static Thickness Mul(in this Thickness thickness1, in double value) => new(thickness1.Left * value, thickness1.Top * value, thickness1.Right * value, thickness1.Bottom * value);
        public static Thickness Div(in this Thickness thickness1, in double value) => new(thickness1.Left / value, thickness1.Top / value, thickness1.Right / value, thickness1.Bottom / value);
    }
}
