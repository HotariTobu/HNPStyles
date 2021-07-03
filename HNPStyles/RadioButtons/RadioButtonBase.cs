using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HNPStyles.Internal
{
    public class RadioButtonBase : ButtonBase
    {
        static RadioButtonBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadioButtonBase), new FrameworkPropertyMetadata(typeof(RadioButtonBase)));
        }

        #region == VividBrush ==

        public static readonly DependencyProperty VividBrushProperty = DependencyProperty.Register("VividBrush", typeof(Brush), typeof(RadioButtonBase), new PropertyMetadata(Brushes.White));
        public Brush VividBrush { get => (Brush)GetValue(VividBrushProperty); set => SetValue(VividBrushProperty, value); }

        #endregion

        #region == IsChecked ==

        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(RadioButtonBase), new PropertyMetadata(false,
          (d, e) => ((RadioButtonBase)d).OnIsCheckedChanged()));
        public bool IsChecked { get => (bool)GetValue(IsCheckedProperty); set => SetValue(IsCheckedProperty, value); }

        public static readonly RoutedEvent IsCheckedChangedEvent = EventManager.RegisterRoutedEvent("IsCheckedChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(RadioButtonBase));
        public event RoutedEventHandler IsCheckedChanged { add => AddHandler(IsCheckedChangedEvent, value); remove => RemoveHandler(IsCheckedChangedEvent, value); }
        private void RaiseIsCheckedChangedEvent() => RaiseEvent(new RoutedEventArgs(IsCheckedChangedEvent));

        protected virtual void OnIsCheckedChanged()
        {
            UpdateIntensity();
            ResetRadioButtons();
            RaiseIsCheckedChangedEvent();
        }

        #endregion

        #region == GroupName ==

        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register("GroupName", typeof(string), typeof(RadioButtonBase), new PropertyMetadata(string.Empty,
          (d, e) => ((RadioButtonBase)d).OnGroupNameChanged()));
        public string GroupName { get => (string)GetValue(GroupNameProperty); set => SetValue(GroupNameProperty, value); }

        public static readonly RoutedEvent GroupNameChangedEvent = EventManager.RegisterRoutedEvent("GroupNameChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(RadioButtonBase));
        public event RoutedEventHandler GroupNameChanged { add => AddHandler(GroupNameChangedEvent, value); remove => RemoveHandler(GroupNameChangedEvent, value); }
        private void RaiseGroupNameChangedEvent() => RaiseEvent(new RoutedEventArgs(GroupNameChangedEvent));

        protected virtual void OnGroupNameChanged()
        {
            UpdateRadioButtons();
            RaiseGroupNameChangedEvent();
        }

        #endregion
        #region == RadioButtons ==

        private readonly List<RadioButtonBase> RadioButtons = new();

        private void UpdateRadioButtons()
        {
            if (Parent is System.Windows.Controls.Panel parent)
            {
                RadioButtons.Clear();
                string name = GroupName;
                foreach (UIElement child in parent.Children)
                {
                    if (child != this && child is RadioButtonBase radio && radio.GroupName.Equals(name))
                    {
                        RadioButtons.Add(radio);
                        radio.RadioButtons.Add(this);
                    }
                }
            }
        }

        private void ResetRadioButtons()
        {
            if (IsChecked)
            {
                foreach (RadioButtonBase radio in RadioButtons)
                {
                    if (radio != this)
                    {
                        radio.IsChecked = false;
                    }
                }
            }
        }

        #endregion

        #region == ButtonWidth ==

        public static readonly DependencyProperty ButtonWidthProperty = DependencyProperty.Register("ButtonWidth", typeof(double), typeof(RadioButtonBase), new PropertyMetadata(0d));
        public double ButtonWidth { get => (double)GetValue(ButtonWidthProperty); set => SetValue(ButtonWidthProperty, value); }

        #endregion
        #region == ButtonHeight ==

        public static readonly DependencyProperty ButtonHeightProperty = DependencyProperty.Register("ButtonHeight", typeof(double), typeof(RadioButtonBase), new PropertyMetadata(0d));
        public double ButtonHeight { get => (double)GetValue(ButtonHeightProperty); set => SetValue(ButtonHeightProperty, value); }

        #endregion
        #region == MarkWidth ==

        public static readonly DependencyProperty MarkWidthProperty = DependencyProperty.Register("MarkWidth", typeof(double), typeof(RadioButtonBase), new PropertyMetadata(0d));
        public double MarkWidth { get => (double)GetValue(MarkWidthProperty); set => SetValue(MarkWidthProperty, value); }

        #endregion
        #region == MarkHeight ==

        public static readonly DependencyProperty MarkHeightProperty = DependencyProperty.Register("MarkHeight", typeof(double), typeof(RadioButtonBase), new PropertyMetadata(0d));
        public double MarkHeight { get => (double)GetValue(MarkHeightProperty); set => SetValue(MarkHeightProperty, value); }

        #endregion
        #region == Interval ==

        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register("Interval", typeof(double), typeof(RadioButtonBase), new PropertyMetadata(0d));
        public double Interval { get => (double)GetValue(IntervalProperty); set => SetValue(IntervalProperty, value); }

        #endregion

        #region == ButtonAlignment ==

        public static readonly DependencyProperty ButtonAlignmentProperty = DependencyProperty.Register("ButtonAlignment", typeof(VerticalAlignment), typeof(RadioButtonBase), new PropertyMetadata(VerticalAlignment.Top));
        public VerticalAlignment ButtonAlignment { get => (VerticalAlignment)GetValue(ButtonAlignmentProperty); set => SetValue(ButtonAlignmentProperty, value); }

        #endregion
        #region == ContentAlignment ==

        public static readonly DependencyProperty ContentAlignmentProperty = DependencyProperty.Register("ContentAlignment", typeof(VerticalAlignment), typeof(RadioButtonBase), new PropertyMetadata(VerticalAlignment.Top));
        public VerticalAlignment ContentAlignment { get => (VerticalAlignment)GetValue(ContentAlignmentProperty); set => SetValue(ContentAlignmentProperty, value); }

        #endregion

        #region == MarklineVisibility ==

        private static readonly DependencyPropertyKey MarklineVisibilityPropertyKey = DependencyProperty.RegisterReadOnly("MarklineVisibility", typeof(Visibility), typeof(RadioButtonBase), new PropertyMetadata(Visibility.Collapsed));
        public static readonly DependencyProperty MarklineVisibilityProperty = MarklineVisibilityPropertyKey.DependencyProperty;
        public Visibility MarklineVisibility { get => (Visibility)GetValue(MarklineVisibilityProperty); private set => SetValue(MarklineVisibilityPropertyKey, value); }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateRadioButtons();
            UpdateIntensity();
        }

        #region == MouseEvents ==

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            if (!IsChecked)
            {
                VisualStateManager.GoToState(this, "CheckedMouseOver", true);
            }

            MarklineVisibility = Visibility.Visible;
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            if (!IsChecked)
            {
                VisualStateManager.GoToState(this, "NotChecked", true);
            }

            MarklineVisibility = Visibility.Collapsed;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            VisualStateManager.GoToState(this, "Pressed", true);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            IsChecked |= IsMouseDown;
            base.OnMouseUp(e);
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
