using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HNPStyles
{
    public class RoundBase : Internal.Base
    {
        static RoundBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RoundBase), new FrameworkPropertyMetadata(typeof(RoundBase)));
        }

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

            if (width < height)
            {
                double radiusX = width / 2;
                Size sizeX = new(radiusX, radiusX);
                Geometry = new PathGeometry(new PathFigure[]
                {
                    new PathFigure(new Point(x, radiusX + y), new PathSegment[]
                    {
                        new ArcSegment(new Point(width + x, radiusX + y), sizeX, 0, false, SweepDirection.Clockwise, true),
                        new LineSegment(new Point(width + x, height - radiusX + y), true),
                        new ArcSegment(new Point(x, height - radiusX + y), sizeX, 0, false, SweepDirection.Clockwise, true),
                    }, true),
                });
            }
            else
            {
                double radiusY = height / 2;
                Size sizeY = new(radiusY, radiusY);
                Geometry = new PathGeometry(new PathFigure[]
                {
                    new PathFigure(new Point(radiusY + x, height + y), new PathSegment[]
                    {
                        new ArcSegment(new Point(radiusY + x, y), sizeY, 0, false, SweepDirection.Clockwise, true),
                        new LineSegment(new Point(width - radiusY + x, y), true),
                        new ArcSegment(new Point(width - radiusY + x, height + y), sizeY, 0, false, SweepDirection.Clockwise, true),
                    }, true),
                });
            }
        }
    }
}
