using System;
using System.Windows;
using System.Windows.Media;

namespace HNPStyles
{
    public class RectangleBase : Internal.Base
    {
        static RectangleBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RectangleBase), new FrameworkPropertyMetadata(typeof(RectangleBase)));
        }

        #region == CornerRadius ==

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(RectangleBase), new PropertyMetadata(new CornerRadius(),
            (d, e) => ((RectangleBase)d).UpdateGeometry()));
        public CornerRadius CornerRadius { get => (CornerRadius)GetValue(CornerRadiusProperty); set => SetValue(CornerRadiusProperty, value); }

        #endregion

        protected override void UpdateGeometry()
        {
            base.UpdateGeometry();

            double thickness = OutlineThickness;
            double x = thickness / 2;
            double y = thickness / 2;
            double width = ActualWidth - thickness;
            double height = ActualHeight - thickness;

            if (width < 0)
            {
                width = 0;
            }
            if (height < 0)
            {
                height = 0;
            }

            double limit = Math.Min(width / 2, height / 2);
            double coerceRadius(double radius) => radius > limit ? limit : radius;

            double topLeft = coerceRadius(CornerRadius.TopLeft);
            double topRight = coerceRadius(CornerRadius.TopRight);
            double bottomRight = coerceRadius(CornerRadius.BottomRight);
            double bottomLeft = coerceRadius(CornerRadius.BottomLeft);

            Geometry = new PathGeometry(new PathFigure[]
                {
                    new PathFigure(new Point(x, topLeft + y), new PathSegment[]
                    {
                        new ArcSegment(new Point(topLeft + x, y), new Size(topLeft, topLeft), 0, false, SweepDirection.Clockwise, true),
                        new LineSegment(new Point(width - topRight + x, y), true),
                        new ArcSegment(new Point(width + x, topRight + y), new Size(topRight, topRight), 0, false, SweepDirection.Clockwise, true),
                        new LineSegment(new Point(width + x, height - bottomRight + y), true),
                        new ArcSegment(new Point(width - bottomRight + x, height + y), new Size(bottomRight, bottomRight), 0, false, SweepDirection.Clockwise, true),
                        new LineSegment(new Point(bottomLeft + x, height + y), true),
                        new ArcSegment(new Point(x, height - bottomLeft + y), new Size(bottomLeft, bottomLeft), 0, false, SweepDirection.Clockwise, true),
                    }, true),
                });
        }
    }
}
