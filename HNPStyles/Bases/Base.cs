using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HNPStyles.Internal
{
    public class Base : ContentControl
    {
        static Base()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Base), new FrameworkPropertyMetadata(typeof(Base)));
        }

        #region == PrimaryColor ==

        public static readonly DependencyProperty PrimaryColorProperty = DependencyProperty.Register("PrimaryColor", typeof(Color), typeof(Base), new PropertyMetadata(Colors.White,
            (d, e) => ((Base)d).OnPrimaryColorChanged()));
        public Color PrimaryColor { get => (Color)GetValue(PrimaryColorProperty); set => SetValue(PrimaryColorProperty, value); }

        private GradientStopCollection OutlineGradientStops = new();
        private GradientStopCollection InnerGradientStops = new();

        protected virtual void OnPrimaryColorChanged()
        {
            UpdateOutlineGradientStops();
            UpdateInnerGradientStops();
            UpdateShadowBrushes();
            UpdateOutlineBrush();
            UpdateInnerBrush();
        }

        #endregion
        #region == ShadowBrushes ==

        private static readonly DependencyPropertyKey ShadowBrush1PropertyKey = DependencyProperty.RegisterReadOnly("ShadowBrush1", typeof(Brush), typeof(Base), new PropertyMetadata(Brushes.White));
        public static readonly DependencyProperty ShadowBrush1Property = ShadowBrush1PropertyKey.DependencyProperty;
        public Brush ShadowBrush1 { get => (Brush)GetValue(ShadowBrush1Property); private set => SetValue(ShadowBrush1PropertyKey, value); }

        private static readonly DependencyPropertyKey ShadowBrush2PropertyKey = DependencyProperty.RegisterReadOnly("ShadowBrush2", typeof(Brush), typeof(Base), new PropertyMetadata(Brushes.White));
        public static readonly DependencyProperty ShadowBrush2Property = ShadowBrush2PropertyKey.DependencyProperty;
        public Brush ShadowBrush2 { get => (Brush)GetValue(ShadowBrush2Property); private set => SetValue(ShadowBrush2PropertyKey, value); }

        private void UpdateShadowBrushes()
        {
            Color color = PrimaryColor;
            ShadowBrush1 = new SolidColorBrush(color.Mul(1 + Intensity));
            ShadowBrush2 = new SolidColorBrush(color.Mul(1 - Intensity));
        }

        #endregion

        #region == OutlineBrushLightParameter ==

        public static readonly DependencyProperty OutlineBrushLightParameterProperty = DependencyProperty.Register("OutlineBrushLightParameter", typeof(double), typeof(Base), new PropertyMetadata(0d,
            (d, e) => ((Base)d).UpdateOutlineGradientStops()));
        public double OutlineBrushLightParameter { get => (double)GetValue(OutlineBrushLightParameterProperty); set => SetValue(OutlineBrushLightParameterProperty, value); }

        #endregion
        #region == OutlineBrushDarkParameter ==

        public static readonly DependencyProperty OutlineBrushDarkParameterProperty = DependencyProperty.Register("OutlineBrushDarkParameter", typeof(double), typeof(Base), new PropertyMetadata(0d,
            (d, e) => ((Base)d).UpdateOutlineGradientStops()));
        public double OutlineBrushDarkParameter { get => (double)GetValue(OutlineBrushDarkParameterProperty); set => SetValue(OutlineBrushDarkParameterProperty, value); }

        #endregion
        #region == OutlineBrush ==

        private static readonly DependencyPropertyKey OutlineBrushPropertyKey = DependencyProperty.RegisterReadOnly("OutlineBrush", typeof(Brush), typeof(Base), new PropertyMetadata(new SolidColorBrush()));
        public static readonly DependencyProperty OutlineBrushProperty = OutlineBrushPropertyKey.DependencyProperty;
        public Brush OutlineBrush { get => (Brush)GetValue(OutlineBrushProperty); private set => SetValue(OutlineBrushPropertyKey, value); }

        private void UpdateOutlineGradientStops()
        {
            Color color = PrimaryColor;
            OutlineGradientStops = new GradientStopCollection()
                {
                    new GradientStop(color.Sub(OutlineBrushLightParameter), 0),
                    new GradientStop(color.Sub(OutlineBrushDarkParameter), 1),
                };
        }

        private void UpdateOutlineBrush()
        {
            OutlineBrush = new LinearGradientBrush(OutlineGradientStops, StartPoint, EndPoint)
            {
                Opacity = OutlineOpacity
            };
        }

        #endregion
        #region == OutlineOpacity ==

        public static readonly DependencyProperty OutlineOpacityProperty = DependencyProperty.Register("OutlineOpacity", typeof(double), typeof(Base), new PropertyMetadata(0d,
            (d, e) => ((Base)d).UpdateOutlineBrush()));
        public double OutlineOpacity { get => (double)GetValue(OutlineOpacityProperty); set => SetValue(OutlineOpacityProperty, value); }

        private static readonly DependencyPropertyKey IsOutlineShownPropertyKey = DependencyProperty.RegisterReadOnly("IsOutlineShown", typeof(bool), typeof(Base), new PropertyMetadata(false));
        public static readonly DependencyProperty IsOutlineShownProperty = IsOutlineShownPropertyKey.DependencyProperty;
        public bool IsOutlineShown { get => (bool)GetValue(IsOutlineShownProperty); private set => SetValue(IsOutlineShownPropertyKey, value); }

        private void UpdateOutlineOpacity() => IsOutlineShown = !IsEnabled || (IsOutlinedWhenIntensityZero && Intensity == 0);

        #endregion
        #region == OutlineThickness ==

        public static readonly DependencyProperty OutlineThicknessProperty = DependencyProperty.Register("OutlineThickness", typeof(double), typeof(Base), new PropertyMetadata(0d,
            (d, e) => ((Base)d).UpdateGeometry()));
        public double OutlineThickness { get => (double)GetValue(OutlineThicknessProperty); set => SetValue(OutlineThicknessProperty, value); }

        #endregion

        #region == InnerBrushLightParameter ==

        public static readonly DependencyProperty InnerBrushLightParameterProperty = DependencyProperty.Register("InnerBrushLightParameter", typeof(double), typeof(Base), new PropertyMetadata(1d,
            (d, e) => ((Base)d).UpdateInnerGradientStops(),
            (d, v) =>
            {
                double parameter = (double)v;
                return parameter <= 0 ? 1 : parameter;
            }));
        public double InnerBrushLightParameter { get => (double)GetValue(InnerBrushLightParameterProperty); set => SetValue(InnerBrushLightParameterProperty, value); }

        #endregion
        #region == InnerBrushDarkParameter ==

        public static readonly DependencyProperty InnerBrushDarkParameterProperty = DependencyProperty.Register("InnerBrushDarkParameter", typeof(double), typeof(Base), new PropertyMetadata(1d,
            (d, e) => ((Base)d).UpdateInnerGradientStops(),
            (d, v) =>
            {
                double parameter = (double)v;
                return parameter <= 0 ? 1 : parameter;
            }));
        public double InnerBrushDarkParameter { get => (double)GetValue(InnerBrushDarkParameterProperty); set => SetValue(InnerBrushDarkParameterProperty, value); }

        #endregion
        #region == InnerBrush ==

        private static readonly DependencyPropertyKey InnerBrushPropertyKey = DependencyProperty.RegisterReadOnly("InnerBrush", typeof(Brush), typeof(Base), new PropertyMetadata(Brushes.White));
        public static readonly DependencyProperty InnerBrushProperty = InnerBrushPropertyKey.DependencyProperty;
        public Brush InnerBrush { get => (Brush)GetValue(InnerBrushProperty); protected set => SetValue(InnerBrushPropertyKey, value); }

        private void UpdateInnerGradientStops()
        {
            Color color = PrimaryColor;
            InnerGradientStops = new GradientStopCollection()
                {
                    new GradientStop(color.Add(color.Div(InnerBrushLightParameter)), 0),
                    new GradientStop(color.Sub(color.Div(InnerBrushDarkParameter)), 1),
                };
        }

        private void UpdateInnerBrush()
        {
            InnerBrush = InnerShape switch
            {
                InnerShapes.Concave => new LinearGradientBrush(InnerGradientStops, StartPoint, EndPoint),
                InnerShapes.Convex => new LinearGradientBrush(InnerGradientStops, EndPoint, StartPoint),
                _ => new SolidColorBrush(PrimaryColor)
            };
        }

        #endregion

        #region == LightAngle ==

        public static readonly DependencyProperty LightAngleProperty = DependencyProperty.Register("LightAngle", typeof(double), typeof(Base), new PropertyMetadata(0d,
            (d, e) => ((Base)d).OnLightAngleChanged()));
        public double LightAngle { get => (double)GetValue(LightAngleProperty); set => SetValue(LightAngleProperty, value); }

        protected virtual void OnLightAngleChanged()
        {
            UpdateGradientColorPoints();
            UpdateDXDY();
        }

        private Point StartPoint;
        private Point EndPoint;

        private void UpdateGradientColorPoints()
        {
            double width = RenderSize.Width;
            double height = RenderSize.Height;
            double radius = Math.Sqrt(width * width + height * height) / 2;
            double radian1 = LightAngle * Math.PI / 180;
            double radian2 = radian1 + Math.PI;
            Point point = new(width / 2, height / 2);

            StartPoint = new Point(radius * Math.Cos(radian1) + point.X, radius * Math.Sin(radian1) + point.Y);
            EndPoint = new Point(radius * Math.Cos(radian2) + point.X, radius * Math.Sin(radian2) + point.Y);

            StartPoint.X /= width;
            StartPoint.Y /= height;
            EndPoint.X /= width;
            EndPoint.Y /= height;

            UpdateOutlineBrush();
            UpdateInnerBrush();
        }

        #endregion
        #region == InnerShape ==

        public static readonly DependencyProperty InnerShapeProperty = DependencyProperty.Register("InnerShape", typeof(InnerShapes), typeof(Base), new PropertyMetadata(InnerShapes.Flat,
            (d, e) => ((Base)d).UpdateInnerBrush()));
        public InnerShapes InnerShape { get => (InnerShapes)GetValue(InnerShapeProperty); set => SetValue(InnerShapeProperty, value); }

        #endregion

        #region == IntensityRate ==

        public static readonly DependencyProperty IntensityRateProperty = DependencyProperty.Register("IntensityRate", typeof(double), typeof(Base), new PropertyMetadata(1d,
            (d, e) => ((Base)d).OnIntensityChanged()));
        public double IntensityRate { get => (double)GetValue(IntensityRateProperty); set => SetValue(IntensityRateProperty, value); }

        #endregion
        #region == Intensity ==

        public static readonly DependencyProperty IntensityProperty = DependencyProperty.Register("Intensity", typeof(double), typeof(Base), new PropertyMetadata(0d,
            (d, e) => ((Base)d).OnIntensityChanged()));
        public double Intensity { get => (double)GetValue(IntensityProperty) * IntensityRate; set => SetValue(IntensityProperty, value); }

        protected virtual void OnIntensityChanged()
        {
            double intensity = Intensity;
            OuterShadowVisibility = intensity > 0 ? Visibility.Visible : Visibility.Collapsed;
            InnerShadowVisibility = intensity < 0 ? Visibility.Visible : Visibility.Collapsed;

            UpdateShadowBrushes();
            UpdateOutlineOpacity();
            UpdateScale();
        }

        #endregion
        #region == IsOutlinedWhenIntensityZero ==

        public static readonly DependencyProperty IsOutlinedWhenIntensityZeroProperty = DependencyProperty.Register("IsOutlinedWhenIntensityZero", typeof(bool), typeof(Base), new PropertyMetadata(false,
            (d, e) => ((Base)d).UpdateOutlineOpacity()));
        public bool IsOutlinedWhenIntensityZero { get => (bool)GetValue(IsOutlinedWhenIntensityZeroProperty); set => SetValue(IsOutlinedWhenIntensityZeroProperty, value); }

        #endregion

        #region == ScaleFactor ==

        public static readonly DependencyProperty ScaleFactorProperty = DependencyProperty.Register("ScaleFactor", typeof(double), typeof(Base), new PropertyMetadata(1d,
            (d, e) => ((Base)d).UpdateScale()));
        public double ScaleFactor { get => (double)GetValue(ScaleFactorProperty); set => SetValue(ScaleFactorProperty, value); }

        #endregion
        #region == Scale ==

        private static readonly DependencyPropertyKey ScalePropertyKey = DependencyProperty.RegisterReadOnly("Scale", typeof(double), typeof(Base), new PropertyMetadata(1d));
        public static readonly DependencyProperty ScaleProperty = ScalePropertyKey.DependencyProperty;
        public double Scale { get => (double)GetValue(ScaleProperty); private set => SetValue(ScalePropertyKey, value); }

        private void UpdateScale()
        {
            double scale = Intensity * ScaleFactor + 1;
            Scale = scale < 0 ? 0 : scale > 2 ? 2 : scale;
        }

        #endregion

        #region == OuterShadowVisibility ==

        private static readonly DependencyPropertyKey OuterShadowVisibilityPropertyKey = DependencyProperty.RegisterReadOnly("OuterShadowVisibility", typeof(Visibility), typeof(Base), new PropertyMetadata(Visibility.Collapsed));
        public static readonly DependencyProperty OuterShadowVisibilityProperty = OuterShadowVisibilityPropertyKey.DependencyProperty;
        public Visibility OuterShadowVisibility { get => (Visibility)GetValue(OuterShadowVisibilityProperty); private set => SetValue(OuterShadowVisibilityPropertyKey, value); }

        #endregion
        #region == InnerShadowVisibility ==

        private static readonly DependencyPropertyKey InnerShadowVisibilityPropertyKey = DependencyProperty.RegisterReadOnly("InnerShadowVisibility", typeof(Visibility), typeof(Base), new PropertyMetadata(Visibility.Collapsed));
        public static readonly DependencyProperty InnerShadowVisibilityProperty = InnerShadowVisibilityPropertyKey.DependencyProperty;
        public Visibility InnerShadowVisibility { get => (Visibility)GetValue(InnerShadowVisibilityProperty); private set => SetValue(InnerShadowVisibilityPropertyKey, value); }

        #endregion

        #region == DistanceRate ==

        public static readonly DependencyProperty DistanceRateProperty = DependencyProperty.Register("DistanceRate", typeof(double), typeof(Base), new PropertyMetadata(1d,
          (d, e) => ((Base)d).OnDistanceChanged()));
        public double DistanceRate { get => (double)GetValue(DistanceRateProperty); set => SetValue(DistanceRateProperty, value); }

        #endregion
        #region == Distance ==

        public static readonly DependencyProperty DistanceProperty = DependencyProperty.Register("Distance", typeof(double), typeof(Base), new PropertyMetadata(0d,
          (d, e) => ((Base)d).OnDistanceChanged()));
        public double Distance { get => (double)GetValue(DistanceProperty) * DistanceRate; set => SetValue(DistanceProperty, value); }

        protected virtual void OnDistanceChanged()
        {
            UpdateDXDY();
            UpdateBlurRadius();
        }

        #endregion
        #region == DistanceToBlurRadiusFactor ==

        public static readonly DependencyProperty DistanceToBlurRadiusFactorProperty = DependencyProperty.Register("DistanceToBlurRadiusFactor", typeof(double), typeof(Base), new PropertyMetadata(0d,
          (d, e) => ((Base)d).UpdateBlurRadius()));
        public double DistanceToBlurRadiusFactor { get => (double)GetValue(DistanceToBlurRadiusFactorProperty); set => SetValue(DistanceToBlurRadiusFactorProperty, value); }

        #endregion
        #region == BlurRadius ==

        private static readonly DependencyPropertyKey BlurRadiusPropertyKey = DependencyProperty.RegisterReadOnly("BlurRadius", typeof(double), typeof(Base), new PropertyMetadata(0d));
        public static readonly DependencyProperty BlurRadiusProperty = BlurRadiusPropertyKey.DependencyProperty;
        public double BlurRadius { get => (double)GetValue(BlurRadiusProperty); private set => SetValue(BlurRadiusPropertyKey, value); }

        private void UpdateBlurRadius()
        {
            BlurRadius = Distance * DistanceToBlurRadiusFactor;
        }

        #endregion

        #region == DX, DY ==

        private static readonly DependencyPropertyKey DX1PropertyKey = DependencyProperty.RegisterReadOnly("DX1", typeof(double), typeof(Base), new PropertyMetadata(0d));
        public static readonly DependencyProperty DX1Property = DX1PropertyKey.DependencyProperty;
        public double DX1 { get => (double)GetValue(DX1Property); private set => SetValue(DX1PropertyKey, value); }

        private static readonly DependencyPropertyKey DY1PropertyKey = DependencyProperty.RegisterReadOnly("DY1", typeof(double), typeof(Base), new PropertyMetadata(0d));
        public static readonly DependencyProperty DY1Property = DY1PropertyKey.DependencyProperty;
        public double DY1 { get => (double)GetValue(DY1Property); private set => SetValue(DY1PropertyKey, value); }

        private static readonly DependencyPropertyKey DX2PropertyKey = DependencyProperty.RegisterReadOnly("DX2", typeof(double), typeof(Base), new PropertyMetadata(0d));
        public static readonly DependencyProperty DX2Property = DX2PropertyKey.DependencyProperty;
        public double DX2 { get => (double)GetValue(DX2Property); private set => SetValue(DX2PropertyKey, value); }

        private static readonly DependencyPropertyKey DY2PropertyKey = DependencyProperty.RegisterReadOnly("DY2", typeof(double), typeof(Base), new PropertyMetadata(0d));
        public static readonly DependencyProperty DY2Property = DY2PropertyKey.DependencyProperty;
        public double DY2 { get => (double)GetValue(DY2Property); private set => SetValue(DY2PropertyKey, value); }

        private void UpdateDXDY()
        {
            double distance = Distance;
            double radian = LightAngle * Math.PI / 180;
            DX2 = -(DX1 = distance * Math.Cos(radian));
            DY2 = -(DY1 = distance * Math.Sin(radian));

            UpdateGeometry1();
            UpdateGeometry2();
        }

        #endregion
        #region == Geometry ==

        public static readonly DependencyProperty GeometryProperty = DependencyProperty.Register("Geometry", typeof(Geometry), typeof(Base), new PropertyMetadata(Geometry.Empty,
            (d, e) =>
            {
                Base b = (Base)d;
                b.UpdateGeometry1();
                b.UpdateGeometry2();
            }));
        public Geometry Geometry 
        { 
            get => GetValue(GeometryProperty) as Geometry;
            set
            {
                value.Freeze();
                SetValue(GeometryProperty, value);
            }
        }

        protected virtual void UpdateGeometry() { }

        #endregion
        #region == Geometry1 ==

        private static readonly DependencyPropertyKey Geometry1PropertyKey = DependencyProperty.RegisterReadOnly("Geometry1", typeof(Geometry), typeof(Base), new PropertyMetadata(Geometry.Empty));
        public static readonly DependencyProperty Geometry1Property = Geometry1PropertyKey.DependencyProperty;
        public Geometry Geometry1 { get => (Geometry)GetValue(Geometry1Property); private set => SetValue(Geometry1PropertyKey, value); }

        private void UpdateGeometry1()
        {
            if (Geometry?.Clone() is Geometry geometry)
            {
                geometry.Transform = new TranslateTransform(DX1, DY1);
                geometry.Freeze();
                Geometry1 = geometry;
            }
        }

        #endregion
        #region == Geometry2 ==

        private static readonly DependencyPropertyKey Geometry2PropertyKey = DependencyProperty.RegisterReadOnly("Geometry2", typeof(Geometry), typeof(Base), new PropertyMetadata(Geometry.Empty));
        public static readonly DependencyProperty Geometry2Property = Geometry2PropertyKey.DependencyProperty;
        public Geometry Geometry2 { get => (Geometry)GetValue(Geometry2Property); private set => SetValue(Geometry2PropertyKey, value); }

        private void UpdateGeometry2()
        {
            if (Geometry?.Clone() is Geometry geometry)
            {
                geometry.Transform = new TranslateTransform(DX2, DY2);
                geometry.Freeze();
                Geometry2 = geometry;
            }
        }

        #endregion

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateGradientColorPoints();
            UpdateGeometry();
        }
    }
}
