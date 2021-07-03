using System.Windows;
using System.Windows.Controls;

namespace HNPStyles
{
    public class EllipseButton : Internal.ButtonBase
    {
        static EllipseButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EllipseButton), new FrameworkPropertyMetadata(typeof(EllipseButton)));
        }
    }
}
