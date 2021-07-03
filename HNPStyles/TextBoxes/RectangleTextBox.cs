using HNPStyles.Internal;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HNPStyles
{
    public class RectangleTextBox : TextBoxBase
    {
        static RectangleTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RectangleTextBox), new FrameworkPropertyMetadata(typeof(RectangleTextBox)));

            TextWrappingProperty.OverrideMetadata(typeof(RectangleTextBox), new FrameworkPropertyMetadata((d, e) => ((RectangleTextBox)d).UpdateInternalWidth()));
        }

        #region == CornerRadius ==

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(RectangleTextBox), new PropertyMetadata(new CornerRadius(),
            (d, e) => ((RectangleTextBox)d).UpdateInternalMargin()));
        public CornerRadius CornerRadius { get => (CornerRadius)GetValue(CornerRadiusProperty); set => SetValue(CornerRadiusProperty, value); }

        #endregion
        #region == InternalMargin ==

        protected override void OnInternalMarginChanged()
        {
            base.OnInternalMarginChanged();
            UpdateInternalWidth();
            UpdateInternalHeight();
        }

        private void UpdateInternalMargin()
        {
            CornerRadius cornerRadius = CornerRadius;
            InternalMargin = new Thickness()
            {
                Left = Math.Max(cornerRadius.TopLeft, cornerRadius.BottomLeft),
                Top = Math.Max(cornerRadius.TopLeft, cornerRadius.TopLeft),
                Right = (ScrollViewer?.ComputedVerticalScrollBarVisibility) == Visibility.Visible ? 0 : Math.Max(cornerRadius.TopRight, cornerRadius.BottomRight),
                Bottom = (ScrollViewer?.ComputedHorizontalScrollBarVisibility) == Visibility.Visible ? 0 : Math.Max(cornerRadius.BottomLeft, cornerRadius.BottomRight),
            };
        }

        #endregion

        #region == IsScrollableBeyondLines ==

        public static readonly DependencyProperty IsScrollableBeyondLinesProperty = DependencyProperty.Register("IsScrollableBeyondLines", typeof(bool), typeof(RectangleTextBox), new PropertyMetadata(false,
          (d, e) => ((RectangleTextBox)d).OnIsScrollableBeyondLinesChanged()));
        public bool IsScrollableBeyondLines { get => (bool)GetValue(IsScrollableBeyondLinesProperty); set => SetValue(IsScrollableBeyondLinesProperty, value); }

        public static readonly RoutedEvent IsScrollableBeyondLinesChangedEvent = EventManager.RegisterRoutedEvent("IsScrollableBeyondLinesChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(RectangleTextBox));
        public event RoutedEventHandler IsScrollableBeyondLinesChanged { add => AddHandler(IsScrollableBeyondLinesChangedEvent, value); remove => RemoveHandler(IsScrollableBeyondLinesChangedEvent, value); }
        private void RaiseIsScrollableBeyondLinesChangedEvent() => RaiseEvent(new RoutedEventArgs(IsScrollableBeyondLinesChangedEvent));

        protected virtual void OnIsScrollableBeyondLinesChanged()
        {
            UpdateInternalHeight();
            RaiseIsScrollableBeyondLinesChangedEvent();
        }

        #endregion

        private ScrollViewerBase ScrollViewer = new();
        private ContentControl ContentWrapper = new();

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ScrollViewer = (ScrollViewerBase)GetTemplateChild("PART_ScrollViewer");
            if (ScrollViewer != null)
            {
                ScrollViewer.SuitableWidthChanged += (s, e) => UpdateInternalWidth();
                ScrollViewer.SuitableHeightChanged += (s, e) => UpdateInternalHeight();
                ScrollViewer.ComputedHorizontalScrollBarVisibilityChanged += (s, e) => UpdateInternalMargin();
                ScrollViewer.ComputedVerticalScrollBarVisibilityChanged += (s, e) => UpdateInternalMargin();
            }

            ContentWrapper = (ContentControl)GetTemplateChild("PART_ContentWrapper");
        }

        #region == ContentsChanged ==

        private int LastLineCount;

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            if (IsScrollableBeyondLines)
            {
                int length = Text.Length;
                if (length > 0)
                {
                    int lineCount = GetLineIndexFromCharacterIndex(length - 1);
                    if (lineCount != LastLineCount)
                    {
                        UpdateInternalHeight();
                    }
                    LastLineCount = lineCount;
                }
            }
        }

        private int LastSelectionStart;

        protected override void OnSelectionChanged(RoutedEventArgs e)
        {
            base.OnSelectionChanged(e);
            if (ScrollViewer != null)
            {
                Rect rect;
                if (SelectionStart == LastSelectionStart)
                {
                    rect = GetRectFromCharacterIndex(SelectionStart + SelectionLength);
                }
                else
                {
                    rect = GetRectFromCharacterIndex(SelectionStart);
                    LastSelectionStart = SelectionStart;
                }
                double right = rect.Right + 2;
                double width = ScrollViewer.SuitableWidth - InternalMargin.Right;
                double height = ScrollViewer.SuitableHeight - InternalMargin.Bottom;
                if (rect.Left < InternalMargin.Left)
                {
                    ScrollViewer.X -= rect.Left - InternalMargin.Left;
                }
                else if (right > width)
                {
                    ScrollViewer.X -= right - width;
                }
                if (rect.Top < InternalMargin.Top)
                {
                    ScrollViewer.Y -= rect.Top - InternalMargin.Top;
                }
                else if (rect.Bottom > height)
                {
                    ScrollViewer.Y -= rect.Bottom - height;
                }
            }
        }

        #endregion

        #region == InternalSize ==

        private void UpdateInternalWidth()
        {
            if (ScrollViewer == null || ContentWrapper == null)
            {
                return;
            }

            ContentWrapper.MinWidth = Math.Max(0, ScrollViewer.SuitableWidth - InternalMargin.Left - InternalMargin.Right);
            if (TextWrapping == TextWrapping.NoWrap)
            {
                ContentWrapper.MaxWidth = double.MaxValue;
            }
            else
            {
                ContentWrapper.MaxWidth = ContentWrapper.MinWidth;
            }
        }

        private void UpdateInternalHeight()
        {
            if (ScrollViewer == null || ContentWrapper == null)
            {
                return;
            }

            double min = Math.Max(0, ScrollViewer.SuitableHeight - InternalMargin.Top - InternalMargin.Bottom);

            if (IsScrollableBeyondLines)
            {
                int length = Text.Length;
                if (length > 0)
                {
                    min += GetRectFromCharacterIndex(length - 1).Top - ScrollViewer.Y - InternalMargin.Top;
                }
            }

            ContentWrapper.MinHeight = min;
        }

        #endregion
    }
}
