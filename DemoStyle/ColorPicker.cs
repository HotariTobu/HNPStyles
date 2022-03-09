using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DemoStyle
{
    /// <summary>
    /// このカスタム コントロールを XAML ファイルで使用するには、手順 1a または 1b の後、手順 2 に従います。
    ///
    /// 手順 1a) 現在のプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Opecap"
    ///
    ///
    /// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Opecap;assembly=Opecap"
    ///
    /// また、XAML ファイルのあるプロジェクトからこのプロジェクトへのプロジェクト参照を追加し、
    /// リビルドして、コンパイル エラーを防ぐ必要があります:
    ///
    ///     ソリューション エクスプローラーで対象のプロジェクトを右クリックし、
    ///     [参照の追加] の [プロジェクト] を選択してから、このプロジェクトを参照し、選択します。
    ///
    ///
    /// 手順 2)
    /// コントロールを XAML ファイルで使用します。
    ///
    ///     <MyNamespace:ColorPicker/>
    ///
    /// </summary>
    public class ColorPicker : Control
    {
        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
        }

        #region == Quality ==

        public int Quality { get => (int)GetValue(QualityProperty); set => SetValue(QualityProperty, value); }
        public static readonly DependencyProperty QualityProperty = DependencyProperty.Register("Quality", typeof(int), typeof(ColorPicker), new PropertyMetadata(0, (d, e) =>
        {
            if (d is ColorPicker colorPicker)
            {
                colorPicker.ColorWheel = DrawColorWheel(colorPicker.Quality);
            }
        }));

        #endregion
        #region == ColorWheel ==

        public ImageSource ColorWheel { get => GetValue(ColorWheelProperty) as ImageSource; set => SetValue(ColorWheelProperty, value); }
        public static readonly DependencyProperty ColorWheelProperty = DependencyProperty.Register("ColorWheel", typeof(ImageSource), typeof(ColorPicker), new PropertyMetadata(null));

        #endregion
        #region == ColorPoint ==

        public Point ColorPoint { get => (Point)GetValue(ColorPointProperty); set => SetValue(ColorPointProperty, value); }
        public static readonly DependencyProperty ColorPointProperty = DependencyProperty.Register("ColorPoint", typeof(Point), typeof(ColorPicker), new PropertyMetadata(new Point()));

        #endregion

        #region == Color ==

        public Color Color { get => (Color)GetValue(ColorProperty); set => SetValue(ColorProperty, value); }
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(ColorPicker), new PropertyMetadata(Colors.White,
            (d,e)=>
            {
                if (d is ColorPicker colorPicker && !colorPicker.IsUpdatingColor)
                {
                    colorPicker.IsSettingColor = true;
                    (double, double, double) hsv = GetHSVFromColor(colorPicker.Color);
                    double theta = hsv.Item1 * TwicePI - Math.PI;
                    double radius = hsv.Item2;
                    colorPicker.ColorPoint = new Point(Math.Cos(theta) * radius, Math.Sin(theta) * radius);
                    colorPicker.BaseColor = GetColorFromHSV(hsv.Item1, hsv.Item2, 1);
                    colorPicker.ColorOpacity = colorPicker.Color.A / 255d;
                    colorPicker.ColorValue = 1 - hsv.Item3;
                    colorPicker.IsSettingColor = false;
                    colorPicker.RaiseColorChangedEvent();
                }
            }));

        private bool IsSettingColor;

        #endregion
        #region == BaseColor ==

        public Color BaseColor { get => (Color)GetValue(BaseColorProperty); set => SetValue(BaseColorProperty, value); }
        public static readonly DependencyProperty BaseColorProperty = DependencyProperty.Register("BaseColor", typeof(Color), typeof(ColorPicker), new PropertyMetadata(Colors.White, (d, e) => UpdateColor(d)));

        #endregion
        #region == ColorOpacity ==

        public double ColorOpacity { get => (double)GetValue(ColorOpacityProperty); set => SetValue(ColorOpacityProperty, value); }
        public static readonly DependencyProperty ColorOpacityProperty = DependencyProperty.Register("ColorOpacity", typeof(double), typeof(ColorPicker), new PropertyMetadata(1d, (d, e) => UpdateColor(d), (d, baseValue) =>
             {
                 double value = (double)baseValue;
                 return value < 0 ? 0 : value > 1 ? 1 : value;
             }));

        #endregion
        #region == ColorValue ==

        public double ColorValue { get => (double)GetValue(ColorValueProperty); set => SetValue(ColorValueProperty, value); }
        public static readonly DependencyProperty ColorValueProperty = DependencyProperty.Register("ColorValue", typeof(double), typeof(ColorPicker), new PropertyMetadata(0d, (d, e) => UpdateColor(d), (d, baseValue) =>
        {
            double value = (double)baseValue;
            return value < 0 ? 0 : value > 1 ? 1 : value;
        }));

        #endregion

        #region == ColorChanged ==

        public static readonly RoutedEvent ColorChangedEvent = EventManager.RegisterRoutedEvent("ColorChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ColorPicker));
        public event RoutedEventHandler ColorChanged { add => AddHandler(ColorChangedEvent, value); remove => RemoveHandler(ColorChangedEvent, value); }
        private void RaiseColorChangedEvent() => RaiseEvent(new RoutedEventArgs(ColorChangedEvent));

        #endregion

        private static void UpdateColor(DependencyObject d)
        {
            if (d is ColorPicker colorPicker && !colorPicker.IsSettingColor)
            {
                colorPicker.IsUpdatingColor = true;
                double alpha = 255 * colorPicker.ColorOpacity;
                double value = 1 - colorPicker.ColorValue;
                double red = colorPicker.BaseColor.R * value;
                double green = colorPicker.BaseColor.G * value;
                double blue = colorPicker.BaseColor.B * value;
                colorPicker.Color = Color.FromArgb((byte)alpha, (byte)red, (byte)green, (byte)blue);
                colorPicker.IsUpdatingColor = false;
                colorPicker.RaiseColorChangedEvent();
            }
        }

        private bool IsUpdatingColor;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("ColorWheelGrid") is Grid ColorWheelGrid)
            {
                ColorWheelGrid.MouseDown += ColorWheelGrid_MouseDown;
                ColorWheelGrid.MouseMove += ColorWheelGrid_MouseMove;
            }
        }

        private void ColorWheelGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
            {
                SetBaseColor(e.GetPosition(frameworkElement), frameworkElement.ActualWidth / 2);
            }
        }

        private void ColorWheelGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement && (e.LeftButton == MouseButtonState.Pressed || e.MiddleButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed))
            {
                SetBaseColor(e.GetPosition(frameworkElement), frameworkElement.ActualWidth / 2);
            }
        }

        private static readonly double TwicePI = 2 * Math.PI;

        private void SetBaseColor(Point point, double radius)
        {
            point.X -= radius;
            point.Y -= radius;
            ColorPoint = new Point(point.X / radius, point.Y / radius);
            BaseColor = GetColorFromHSV((Math.Atan2(point.Y, point.X) + Math.PI) / TwicePI, ((Vector)point).Length / radius, 1);
        }

        private static ImageSource DrawColorWheel(int radius)
        {
            int size = radius * 2;
            WriteableBitmap writeableBitmap = new WriteableBitmap(size, size, 96, 96, PixelFormats.Bgr24, null);

            int length = (int)(TwicePI * size);
            Color[] colors = new Color[length];
            for (int i = 0; i < length; i++)
            {
                colors[i] = GetColorFromHSV((double)i / length, 1, 1);
            }

            byte[] pixels = new byte[size * size * 3];
            for (int y = 0; y < size; y++)
            {
                int disY = y - radius;
                for (int x = 0; x < size; x++)
                {
                    int index = (size * y + x) * 3;
                    Color color = colors[(int)((Math.Atan2(disY, x - radius) + Math.PI) / TwicePI * length) - 1];
                    pixels[index] = color.B;
                    pixels[index + 1] = color.G;
                    pixels[index + 2] = color.R;
                }
            }

            writeableBitmap.WritePixels(new Int32Rect(0, 0, size, size), pixels, size * 3, 0, 0);
            return writeableBitmap;
        }

        private static Color GetColorFromHSV(double h, double s, double v)
        {
            h = h < 1 ? h : 0;
            double max = v;
            double range = s * max;
            double min = max - range;
            double[] rgb = new double[3];
            int hd = (int)(h * 6) % 6;
            int hdd = (int)(h * 3) % 3;
            rgb[hdd] = max;
            rgb[((hd + 1) / 2 + 1) % 3] = min;
            rgb[(5 - hd) % 3] = (hd % 2 == 0 ? -1 : 1) * (h * 6 - hdd * 2 - 1) * range + min;
            return Color.FromRgb((byte)(rgb[0] * 255), (byte)(rgb[1] * 255), (byte)(rgb[2] * 255));
        }

        private static (double,double,double) GetHSVFromColor(Color color)
        {
            double r = color.R / 255d;
            double g = color.G / 255d;
            double b = color.B / 255d;
            double max = r > g ? r : g;
            max = max > b ? max : b;
            double min = r < g ? r : g;
            min = min < b ? min : b;
            double range = max - min;
            double h = 0;
            double s = range;
            double v = max;
            if (range > 0)
            {
                if (max == r)
                {
                    h = (g - b) / range + 1;
                }
                else if (max == g)
                {
                    h = (b - r) / range + 3;
                }
                else
                {
                    h = (r - g) / range + 5;
                }
                h /= 6;
                s /= max;
            }
            return (h, s, v);
        }
    }
}
