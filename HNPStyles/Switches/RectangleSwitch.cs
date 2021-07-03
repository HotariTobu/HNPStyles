using System.Windows;

namespace HNPStyles
{
    public class RectangleSwitch : Internal.SwitchBase
    {
        static RectangleSwitch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RectangleSwitch), new FrameworkPropertyMetadata(typeof(RectangleSwitch)));
        }
    }
}
