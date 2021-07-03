using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HNPStyles.Internal
{
    public class BarBase : Control
    {
        static BarBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BarBase), new FrameworkPropertyMetadata(typeof(BarBase)));
        }

        public BarBase()
        {
            IsVisibleChanged += BarBase_IsVisibleChanged;
        }

        #region == IsHorizontal ==

        private static readonly DependencyPropertyKey IsHorizontalPropertyKey = DependencyProperty.RegisterReadOnly("IsHorizontal", typeof(bool), typeof(BarBase), new PropertyMetadata(false,
          (d, e) => ((BarBase)d).OnIsHorizontalChanged()));
        public static readonly DependencyProperty IsHorizontalProperty = IsHorizontalPropertyKey.DependencyProperty;
        public bool IsHorizontal { get => (bool)GetValue(IsHorizontalProperty); private set => SetValue(IsHorizontalPropertyKey, value); }

        public static readonly RoutedEvent IsHorizontalChangedEvent = EventManager.RegisterRoutedEvent("IsHorizontalChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(BarBase));
        public event RoutedEventHandler IsHorizontalChanged { add => AddHandler(IsHorizontalChangedEvent, value); remove => RemoveHandler(IsHorizontalChangedEvent, value); }
        private void RaiseIsHorizontalChangedEvent() => RaiseEvent(new RoutedEventArgs(IsHorizontalChangedEvent));

        protected virtual void OnIsHorizontalChanged()
        {
            RaiseIsHorizontalChangedEvent();
        }

        private void UpdateIsHorizontal()
        {
            IsHorizontal = ActualWidth > ActualHeight;
        }

        #endregion
        #region == Length ==

        protected double Length;
        private void UpdateLength() => Length = IsHorizontal ? ActualWidth : ActualHeight;

        #endregion
        #region == MinLength ==

        protected double MinLength;
        private void UpdateMinLength() => MinLength = IsHorizontal ? ActualHeight : ActualWidth;

        #endregion

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            OnIsHorizontalChanged();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateIsHorizontal();
            UpdateLength();
            UpdateMinLength();
        }

        private void BarBase_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                UpdateIsHorizontal();
                UpdateLength();
            }
        }

        protected double GetMousePosition()
        {
            Point point = Mouse.GetPosition(this);
            return IsHorizontal ? point.X : point.Y;
        }
    }
}
