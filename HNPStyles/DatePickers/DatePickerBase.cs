using HNPStyles.Internal.DatePickers;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HNPStyles.Internal
{
    public class DatePickerBase : Control
    {
        static DatePickerBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DatePickerBase), new FrameworkPropertyMetadata(typeof(DatePickerBase)));
        }

        #region == Month ==

        private static readonly DependencyPropertyKey MonthPropertyKey = DependencyProperty.RegisterReadOnly("Month", typeof(string), typeof(DatePickerBase), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty MonthProperty = MonthPropertyKey.DependencyProperty;
        public string Month { get => (string)GetValue(MonthProperty); private set => SetValue(MonthPropertyKey, value); }

        #endregion

        #region == Weeks ==

        public static readonly DependencyProperty WeeksProperty = DependencyProperty.Register("Weeks", typeof(ObservableCollection<Week>), typeof(DatePickerBase), new PropertyMetadata(new ObservableCollection<Week>()));
        public ObservableCollection<Week> Weeks { get => GetValue(WeeksProperty) as ObservableCollection<Week>; set => SetValue(WeeksProperty, value); }

        #endregion

        #region == MouseEvents ==

        #endregion
    }
}
