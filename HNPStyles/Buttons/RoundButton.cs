using System.Windows;
using System.Windows.Controls;

namespace HNPStyles
{
    public class RoundButton : Internal.ButtonBase
    {
        static RoundButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RoundButton), new FrameworkPropertyMetadata(typeof(RoundButton)));
        }
    }
}
