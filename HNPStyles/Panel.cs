using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HNPStyles
{
    public class Panel : Grid
    {
        static Panel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Panel), new FrameworkPropertyMetadata(typeof(Panel)));
        }

        #region == PrimaryColor ==

        public static readonly DependencyProperty PrimaryColorProperty = DependencyProperty.Register("PrimaryColor", typeof(Color), typeof(Panel), new PropertyMetadata(Colors.White,
            (d, e) => ((Panel)d).OnPrimaryColorChanged()));
        public Color PrimaryColor { get => (Color)GetValue(PrimaryColorProperty); set => SetValue(PrimaryColorProperty, value); }

        protected virtual void OnPrimaryColorChanged()
        {
            Background = new SolidColorBrush(PrimaryColor);

            UpdateForeground();
        }

        #endregion

        #region == OutlineBrushLightParameter ==

        public static readonly DependencyProperty OutlineBrushLightParameterProperty = DependencyProperty.Register("OutlineBrushLightParameter", typeof(double), typeof(Panel), new PropertyMetadata(0d));
        public double OutlineBrushLightParameter { get => (double)GetValue(OutlineBrushLightParameterProperty); set => SetValue(OutlineBrushLightParameterProperty, value); }

        #endregion
        #region == OutlineBrushDarkParameter ==

        public static readonly DependencyProperty OutlineBrushDarkParameterProperty = DependencyProperty.Register("OutlineBrushDarkParameter", typeof(double), typeof(Panel), new PropertyMetadata(0d));
        public double OutlineBrushDarkParameter { get => (double)GetValue(OutlineBrushDarkParameterProperty); set => SetValue(OutlineBrushDarkParameterProperty, value); }

        #endregion
        #region == OutlineThickness ==

        public static readonly DependencyProperty OutlineThicknessProperty = DependencyProperty.Register("OutlineThickness", typeof(double), typeof(Panel), new PropertyMetadata(0d));
        public double OutlineThickness { get => (double)GetValue(OutlineThicknessProperty); set => SetValue(OutlineThicknessProperty, value); }

        #endregion

        #region == InnerBrushLightParameter ==

        public static readonly DependencyProperty InnerBrushLightParameterProperty = DependencyProperty.Register("InnerBrushLightParameter", typeof(double), typeof(Panel), new PropertyMetadata(1d,
            (d, e) => { },
            (d, v) =>
            {
                double parameter = (double)v;
                return parameter <= 0 ? 1 : parameter;
            }));
        public double InnerBrushLightParameter { get => (double)GetValue(InnerBrushLightParameterProperty); set => SetValue(InnerBrushLightParameterProperty, value); }

        #endregion
        #region == InnerBrushDarkParameter ==

        public static readonly DependencyProperty InnerBrushDarkParameterProperty = DependencyProperty.Register("InnerBrushDarkParameter", typeof(double), typeof(Panel), new PropertyMetadata(1d,
            (d, e) => { },
            (d, v) =>
            {
                double parameter = (double)v;
                return parameter <= 0 ? 1 : parameter;
            }));
        public double InnerBrushDarkParameter { get => (double)GetValue(InnerBrushDarkParameterProperty); set => SetValue(InnerBrushDarkParameterProperty, value); }

        #endregion

        #region == LightAngle ==

        public static readonly DependencyProperty LightAngleProperty = DependencyProperty.Register("LightAngle", typeof(double), typeof(Panel), new PropertyMetadata(0d));
        public double LightAngle { get => (double)GetValue(LightAngleProperty); set => SetValue(LightAngleProperty, value); }

        #endregion
        #region == InnerShape ==

        public static readonly DependencyProperty InnerShapeProperty = DependencyProperty.Register("InnerShape", typeof(InnerShapes), typeof(Panel), new PropertyMetadata(InnerShapes.Flat));
        public InnerShapes InnerShape { get => (InnerShapes)GetValue(InnerShapeProperty); set => SetValue(InnerShapeProperty, value); }

        #endregion

        #region == IntensityRate ==

        public static readonly DependencyProperty IntensityRateProperty = DependencyProperty.Register("IntensityRate", typeof(double), typeof(Panel), new PropertyMetadata(1d));
        public double IntensityRate { get => (double)GetValue(IntensityRateProperty); set => SetValue(IntensityRateProperty, value); }

        #endregion

        #region == ScaleFactor ==

        public static readonly DependencyProperty ScaleFactorProperty = DependencyProperty.Register("ScaleFactor", typeof(double), typeof(Panel), new PropertyMetadata(1d));
        public double ScaleFactor { get => (double)GetValue(ScaleFactorProperty); set => SetValue(ScaleFactorProperty, value); }

        #endregion

        #region == Distance ==

        public static readonly DependencyProperty DistanceProperty = DependencyProperty.Register("Distance", typeof(double), typeof(Panel), new PropertyMetadata(0d));
        public double Distance { get => (double)GetValue(DistanceProperty); set => SetValue(DistanceProperty, value); }

        #endregion
        #region == DistanceToBlurRadiusFactor ==

        public static readonly DependencyProperty DistanceToBlurRadiusFactorProperty = DependencyProperty.Register("DistanceToBlurRadiusFactor", typeof(double), typeof(Panel), new PropertyMetadata(0d));
        public double DistanceToBlurRadiusFactor { get => (double)GetValue(DistanceToBlurRadiusFactorProperty); set => SetValue(DistanceToBlurRadiusFactorProperty, value); }

        #endregion

        #region == CornerRadius ==

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(Panel), new PropertyMetadata(new CornerRadius()));
        public CornerRadius CornerRadius { get => (CornerRadius)GetValue(CornerRadiusProperty); set => SetValue(CornerRadiusProperty, value); }

        #endregion
        #region == TextDistance ==

        public static readonly DependencyProperty TextDistanceProperty = DependencyProperty.Register("TextDistance", typeof(double), typeof(Panel), new PropertyMetadata(1d));
        public double TextDistance { get => (double)GetValue(TextDistanceProperty); set => SetValue(TextDistanceProperty, value); }

        #endregion
        #region == TextDistanceToBlurRadiusFactor ==

        public static readonly DependencyProperty TextDistanceToBlurRadiusFactorProperty = DependencyProperty.Register("TextDistanceToBlurRadiusFactor", typeof(double), typeof(Panel), new PropertyMetadata(0d));
        public double TextDistanceToBlurRadiusFactor { get => (double)GetValue(TextDistanceToBlurRadiusFactorProperty); set => SetValue(TextDistanceToBlurRadiusFactorProperty, value); }

        #endregion

        #region == BaseLength ==

        public static readonly DependencyProperty BaseLengthProperty = DependencyProperty.Register("BaseLength", typeof(double), typeof(Panel), new PropertyMetadata(0d));
        public double BaseLength { get => (double)GetValue(BaseLengthProperty); set => SetValue(BaseLengthProperty, value); }

        #endregion
        #region == DefaultMargin ==

        public static readonly DependencyProperty DefaultMarginProperty = DependencyProperty.Register("DefaultMargin", typeof(Thickness), typeof(Panel), new PropertyMetadata(new Thickness()));
        public Thickness DefaultMargin { get => (Thickness)GetValue(DefaultMarginProperty); set => SetValue(DefaultMarginProperty, value); }

        #endregion

        #region == VividBrush ==

        public static readonly DependencyProperty VividBrushProperty = DependencyProperty.Register("VividBrush", typeof(Brush), typeof(Panel), new PropertyMetadata(Brushes.White));
        public Brush VividBrush { get => (Brush)GetValue(VividBrushProperty); set => SetValue(VividBrushProperty, value); }

        #endregion

        #region == ForegroundThreshold ==

        public static readonly DependencyProperty ForegroundThresholdProperty = DependencyProperty.Register("ForegroundThreshold", typeof(double), typeof(Panel), new PropertyMetadata(0d,
            (d, e) => ((Panel)d).UpdateForeground()));
        public double ForegroundThreshold { get => (double)GetValue(ForegroundThresholdProperty); set => SetValue(ForegroundThresholdProperty, value); }

        #endregion
        #region == Foreground ==

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register("Foreground", typeof(Brush), typeof(Panel), new PropertyMetadata(Brushes.Black));
        public Brush Foreground { get => (Brush)GetValue(ForegroundProperty); set => SetValue(ForegroundProperty, value); }

        private void UpdateForeground()
        {
            Color color = PrimaryColor;
            Foreground = (color.R + color.G + color.B) / 3d < ForegroundThreshold ? Brushes.White : Brushes.Black;
        }

        #endregion
    }
}
