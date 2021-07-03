using System.Windows;

namespace HNPStyles
{
    public class RoundProgressBar : Internal.ProgressBarBase
    {
        static RoundProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RoundProgressBar), new FrameworkPropertyMetadata(typeof(RoundProgressBar)));
        }

        #region == CornerRadius ==

        private static readonly DependencyPropertyKey CornerRadiusPropertyKey = DependencyProperty.RegisterReadOnly("CornerRadius", typeof(CornerRadius), typeof(RoundProgressBar), new PropertyMetadata(new CornerRadius()));
        public static readonly DependencyProperty CornerRadiusProperty = CornerRadiusPropertyKey.DependencyProperty;
        public CornerRadius CornerRadius { get => (CornerRadius)GetValue(CornerRadiusProperty); private set => SetValue(CornerRadiusPropertyKey, value); }

        private void UpdateCornerRadius()
        {
            CornerRadius = IsHorizontal ? new CornerRadius(ActualHeight / 2) : new CornerRadius(ActualWidth / 2);
        }

        #endregion

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateCornerRadius();
        }
    }
}
