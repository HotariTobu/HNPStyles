using System.Windows;
using System.Windows.Controls;

namespace HNPStyles
{
    public class RectangleToggleButton : Internal.ToggleButtonBase
    {
        static RectangleToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RectangleToggleButton), new FrameworkPropertyMetadata(typeof(RectangleToggleButton)));
        }
    }
}
