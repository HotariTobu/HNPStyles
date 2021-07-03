using System.Windows;
using System.Windows.Controls;

namespace HNPStyles
{
    public class EllipseToggleButton : Internal.ToggleButtonBase
    {
        static EllipseToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EllipseToggleButton), new FrameworkPropertyMetadata(typeof(EllipseToggleButton)));
        }
    }
}
