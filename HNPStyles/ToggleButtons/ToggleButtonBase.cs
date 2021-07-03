using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace HNPStyles.Internal
{
    public class ToggleButtonBase : ButtonBase
    {
        static ToggleButtonBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleButtonBase), new FrameworkPropertyMetadata(typeof(ToggleButtonBase)));
        }

        #region == IsChecked ==

        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(ToggleButtonBase), new PropertyMetadata(false,
          (d, e) => ((ToggleButtonBase)d).OnIsCheckedChanged()));
        public bool IsChecked { get => (bool)GetValue(IsCheckedProperty); set => SetValue(IsCheckedProperty, value); }

        public static readonly RoutedEvent IsCheckedChangedEvent = EventManager.RegisterRoutedEvent("IsCheckedChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ToggleButtonBase));
        public event RoutedEventHandler IsCheckedChanged { add => AddHandler(IsCheckedChangedEvent, value); remove => RemoveHandler(IsCheckedChangedEvent, value); }
        private void RaiseIsCheckedChangedEvent() => RaiseEvent(new RoutedEventArgs(IsCheckedChangedEvent));

        protected virtual void OnIsCheckedChanged()
        {
            RaiseIsCheckedChangedEvent();
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateIntensity();
        }

        #region == MouseEvents ==

        protected override void OnMouseClicked()
        {
            base.OnMouseClicked();
            IsChecked ^= true;
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            VisualStateManager.GoToState(this, "Checked", true);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            UpdateIntensity();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            VisualStateManager.GoToState(this, "Pressed", true);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            VisualStateManager.GoToState(this, "Checked", true);
        }

        private void UpdateIntensity()
        {
            if (IsChecked)
            {
                VisualStateManager.GoToState(this, "Checked", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "NotChecked", true);
            }
        }

        #endregion
    }
}
