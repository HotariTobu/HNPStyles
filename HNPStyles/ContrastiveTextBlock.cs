using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HNPStyles
{
    public class ContrastiveTextBlock : TextBlock
    {
        static ContrastiveTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContrastiveTextBlock), new FrameworkPropertyMetadata(typeof(ContrastiveTextBlock)));
        }
    }
}
