using System.Windows;

namespace HNPStyles
{
    public class RectangleProgressBar : Internal.ProgressBarBase
    {
        static RectangleProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RectangleProgressBar), new FrameworkPropertyMetadata(typeof(RectangleProgressBar)));
        }
    }
}
