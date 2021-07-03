using System.Windows;

namespace HNPStyles
{
    public class RectangleScrollViewer : Internal.ScrollViewerBase
    {
        static RectangleScrollViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RectangleScrollViewer), new FrameworkPropertyMetadata(typeof(RectangleScrollViewer)));
        }
    }
}
