using System.Windows;

namespace HNPStyles
{
    public class RoundTextBox : Internal.TextBoxBase
    {
        static RoundTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RoundTextBox), new FrameworkPropertyMetadata(typeof(RoundTextBox)));
        }

        #region == InternalMargin ==

        private void UpdateInternalMargin()
        {
            double width = RenderSize.Width;
            double height = RenderSize.Height;
            double leftAndRight = width < height ? width / 2 : height / 2;
            InternalMargin = new Thickness()
            {
                Left = leftAndRight,
                Right = leftAndRight,
            };
        }

        #endregion

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateInternalMargin();
        }
    }
}
