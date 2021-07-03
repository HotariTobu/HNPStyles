using System.Windows;
using System.Windows.Controls;

namespace HNPStyles
{
    public class RoundToggleButton : Internal.ToggleButtonBase
    {
        static RoundToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RoundToggleButton), new FrameworkPropertyMetadata(typeof(RoundToggleButton)));
        }
    }
}
