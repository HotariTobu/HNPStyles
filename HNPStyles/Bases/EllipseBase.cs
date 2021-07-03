using System.Windows;
using System.Windows.Media;

namespace HNPStyles
{
    public class EllipseBase : Internal.Base
    {
        static EllipseBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EllipseBase), new FrameworkPropertyMetadata(typeof(EllipseBase)));
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

            Geometry = new EllipseGeometry(new Rect(x, y, width, height));
        }
    }
}
