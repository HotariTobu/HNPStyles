using System.Windows;

namespace HNPStyles
{
    public class RoundSwitch : Internal.SwitchBase
    {
        static RoundSwitch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RoundSwitch), new FrameworkPropertyMetadata(typeof(RoundSwitch)));
        }
    }
}
