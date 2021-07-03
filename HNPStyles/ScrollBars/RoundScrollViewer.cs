using System.Windows;

namespace HNPStyles
{
    public class RoundScrollViewer : Internal.ScrollViewerBase
    {
        static RoundScrollViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RoundScrollViewer), new FrameworkPropertyMetadata(typeof(RoundScrollViewer)));
        }
    }
}
