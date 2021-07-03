using System.Windows;
using System.Windows.Controls;

namespace HNPStyles
{
    public class EllipseRadioButton : Internal.RadioButtonBase
    {
        static EllipseRadioButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EllipseRadioButton), new FrameworkPropertyMetadata(typeof(EllipseRadioButton)));
        }
    }
}
