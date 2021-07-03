using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HNPStyles.Internal
{
    public class TextBoxBase : TextBox
    {
        static TextBoxBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxBase), new FrameworkPropertyMetadata(typeof(TextBoxBase)));
        }

        #region == Hint ==

        public static readonly DependencyProperty HintProperty = DependencyProperty.Register("Hint", typeof(object), typeof(TextBoxBase), new PropertyMetadata(null));
        public object Hint { get => GetValue(HintProperty); set => SetValue(HintProperty, value); }

        #endregion
        #region == HintVisibility ==

        private static readonly DependencyPropertyKey HintVisibilityPropertyKey = DependencyProperty.RegisterReadOnly("HintVisibility", typeof(Visibility), typeof(TextBoxBase), new PropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty HintVisibilityProperty = HintVisibilityPropertyKey.DependencyProperty;
        public Visibility HintVisibility { get => (Visibility)GetValue(HintVisibilityProperty); private set => SetValue(HintVisibilityPropertyKey, value); }

        private void UpdateHintVisibility() => HintVisibility = string.IsNullOrWhiteSpace(Text) ? (IsFocused ? (IsReadOnly ? Visibility.Visible : Visibility.Collapsed) : Visibility.Visible) : Visibility.Collapsed;

        #endregion

        #region == InternalMargin ==

        private static readonly DependencyPropertyKey InternalMarginPropertyKey = DependencyProperty.RegisterReadOnly("InternalMargin", typeof(Thickness), typeof(TextBoxBase), new PropertyMetadata(new Thickness(),
          (d, e) => ((TextBoxBase)d).OnInternalMarginChanged()));
        public static readonly DependencyProperty InternalMarginProperty = InternalMarginPropertyKey.DependencyProperty;
        public Thickness InternalMargin { get => (Thickness)GetValue(InternalMarginProperty); protected set => SetValue(InternalMarginPropertyKey, value); }

        protected virtual void OnInternalMarginChanged() { }

        #endregion

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            UpdateHintVisibility();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            UpdateHintVisibility();
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            UpdateHintVisibility();
        }
    }
}
