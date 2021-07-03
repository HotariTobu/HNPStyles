using System.Windows;
using System.Windows.Controls;

namespace HNPStyles
{
    public class RectangleRadioButton : Internal.RadioButtonBase
    {
        static RectangleRadioButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RectangleRadioButton), new FrameworkPropertyMetadata(typeof(RectangleRadioButton)));
        }
    }
}
