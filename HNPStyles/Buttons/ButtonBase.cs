using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HNPStyles.Internal
{
    public class ButtonBase : ContentControl
    {
        static ButtonBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonBase), new FrameworkPropertyMetadata(typeof(ButtonBase)));
        }

        #region == Clicked ==

        public static readonly RoutedEvent ClickedEvent = EventManager.RegisterRoutedEvent("Clicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ButtonBase));
        public event RoutedEventHandler Clicked { add => AddHandler(ClickedEvent, value); remove => RemoveHandler(ClickedEvent, value); }
        private void RaiseClickedEvent() => RaiseEvent(new RoutedEventArgs(ClickedEvent));

        protected virtual void OnMouseClicked()
        {
            RaiseClickedEvent();
        }

        #endregion

        #region == MouseEvents ==

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            VisualStateManager.GoToState(this, "MouseOver", true);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            VisualStateManager.GoToState(this, "Normal", true);
        }

        protected bool IsMouseDown;

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            IsMouseDown = true;
            VisualStateManager.GoToState(this, "Pressed", true);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (IsMouseDown)
            {
                OnMouseClicked();
            }
            IsMouseDown = false;
            VisualStateManager.GoToState(this, "MouseOver", true);
        }

        #endregion
    }
}
