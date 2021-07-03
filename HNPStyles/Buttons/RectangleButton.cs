using System.Windows;
using System.Windows.Controls;

namespace HNPStyles
{
    public class RectangleButton : Internal.ButtonBase
    {
        static RectangleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RectangleButton), new FrameworkPropertyMetadata(typeof(RectangleButton)));
        }
    }
}
