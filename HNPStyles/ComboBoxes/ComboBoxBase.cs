using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace HNPStyles.Internal
{
    public class ComboBoxBase : Selector
    {
        static ComboBoxBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBoxBase), new FrameworkPropertyMetadata(typeof(ComboBoxBase)));
        }

        #region == Hint ==

        public static readonly DependencyProperty HintProperty = DependencyProperty.Register("Hint", typeof(object), typeof(ComboBoxBase), new PropertyMetadata(null,
            (d, e) => ((ComboBoxBase)d).OnHintChanged()));
        public object Hint { get => GetValue(HintProperty); set => SetValue(HintProperty, value); }

        protected virtual void OnHintChanged()
        {
            if (!HasItems || SelectedIndex == -1)
            {
                SelectedValue = Hint;
            }
        }

        #endregion

        #region == InternalMargin ==

        private static readonly DependencyPropertyKey InternalMarginPropertyKey = DependencyProperty.RegisterReadOnly("InternalMargin", typeof(Thickness), typeof(ComboBoxBase), new PropertyMetadata(new Thickness(),
          (d, e) => ((ComboBoxBase)d).OnInternalMarginChanged()));
        public static readonly DependencyProperty InternalMarginProperty = InternalMarginPropertyKey.DependencyProperty;
        public Thickness InternalMargin { get => (Thickness)GetValue(InternalMarginProperty); protected set => SetValue(InternalMarginPropertyKey, value); }

        protected virtual void OnInternalMarginChanged() { }

        #endregion

        #region == ArrowWidth ==

        public static readonly DependencyProperty ArrowWidthProperty = DependencyProperty.Register("ArrowWidth", typeof(double), typeof(ComboBoxBase), new PropertyMetadata(0d));
        public double ArrowWidth { get => (double)GetValue(ArrowWidthProperty); set => SetValue(ArrowWidthProperty, value); }

        #endregion
        #region == ArrowHeight ==

        public static readonly DependencyProperty ArrowHeightProperty = DependencyProperty.Register("ArrowHeight", typeof(double), typeof(ComboBoxBase), new PropertyMetadata(0d));
        public double ArrowHeight { get => (double)GetValue(ArrowHeightProperty); set => SetValue(ArrowHeightProperty, value); }

        #endregion

        #region == DropDownVerticalOffset ==

        public static readonly DependencyProperty DropDownVerticalOffsetProperty = DependencyProperty.Register("DropDownVerticalOffset", typeof(double), typeof(ComboBoxBase), new PropertyMetadata(0d));
        public double DropDownVerticalOffset { get => (double)GetValue(DropDownVerticalOffsetProperty); set => SetValue(DropDownVerticalOffsetProperty, value); }

        #endregion
        #region == MaxDropDownHeight ==

        public static readonly DependencyProperty MaxDropDownHeightProperty = DependencyProperty.Register("MaxDropDownHeight", typeof(double), typeof(ComboBoxBase), new PropertyMetadata(0d));
        public double MaxDropDownHeight { get => (double)GetValue(MaxDropDownHeightProperty); set => SetValue(MaxDropDownHeightProperty, value); }

        #endregion
        #region == DropDownMargin ==

        public static readonly DependencyProperty DropDownMarginProperty = DependencyProperty.Register("DropDownMargin", typeof(Thickness), typeof(ComboBoxBase), new PropertyMetadata(new Thickness()));
        public Thickness DropDownMargin { get => (Thickness)GetValue(DropDownMarginProperty); set => SetValue(DropDownMarginProperty, value); }

        #endregion
        #region == ItemMargin ==

        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register("ItemMargin", typeof(Thickness), typeof(ComboBoxBase), new PropertyMetadata(new Thickness()));
        public Thickness ItemMargin { get => (Thickness)GetValue(ItemMarginProperty); set => SetValue(ItemMarginProperty, value); }

        #endregion

        #region == InternalItems ==

        public static readonly DependencyProperty InternalItemsProperty = DependencyProperty.Register("InternalItems", typeof(ObservableCollection<ComboBoxItem>), typeof(ComboBoxBase), new PropertyMetadata(new ObservableCollection<ComboBoxItem>()));
        public ObservableCollection<ComboBoxItem> InternalItems { get => GetValue(InternalItemsProperty) as ObservableCollection<ComboBoxItem>; set => SetValue(InternalItemsProperty, value); }

        private void UpdateInternalItems()
        {
            object value = SelectedValue;
            InternalItems.Clear();
            foreach (object o in Items)
            {
                if (o != value)
                {
                    InternalItems.Add(new ComboBoxItem(o, () =>
                    {
                        SelectedValue = o;
                        ClosePopup();
                    }));
                }
            }
        }

        #endregion

        #region == Popup ==

        private Popup Popup = new();
        private bool IsDropDownOpen;

        private void OpenPopup()
        {
            IsDropDownOpen = true;
            VisualStateManager.GoToState(this, "DropDownOpen", true);
            UpdateInternalItems();
        }

        private void ClosePopup()
        {
            IsDropDownOpen = false;
            VisualStateManager.GoToState(this, "DropDownClose", true);
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_Button") is ButtonBase button)
            {
                button.Clicked += (s, e) =>
                {
                    if (IsDropDownOpen)
                    {
                        ClosePopup();
                    }
                    else
                    {
                        OpenPopup();
                    }
                };
            }

            Popup = (Popup)GetTemplateChild("PART_Popup");
            if (Popup != null)
            {
                Popup.Opened += (s, e) => Mouse.Capture(Popup.Child, CaptureMode.SubTree);
                Popup.Closed += (s, e) => Mouse.Capture(null);
                Popup.Child.MouseDown += Child_MouseDown;
                Popup.Child.MouseWheel += Child_MouseWheel;
            }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            object value = SelectedValue;
            SelectedItem = value;
            SelectedIndex = Items.IndexOf(value);
        }

        private void Child_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClosePopup();
        }

        protected override void OnLostMouseCapture(MouseEventArgs e)
        {
            base.OnLostMouseCapture(e);
            if (Popup.IsOpen)
            {
                ClosePopup();
            }
        }

        private void Child_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!new Rect(Popup.Child.RenderSize).Contains(Mouse.GetPosition(Popup.Child)))
            {
                ClosePopup();
            }
        }
    }
}
