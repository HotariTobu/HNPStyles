using System;
using System.ComponentModel;
using System.Windows.Input;

namespace HNPStyles.Internal
{
    public class ComboBoxItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ComboBoxItem(object value, Action clickAction)
        {
            Value = value;

            Command = new ClickCommand(clickAction);
        }

        #region == Value ==

        private object _Value;
        public object Value
        {
            get => _Value;
            set
            {
                if (_Value != value)
                {
                    _Value = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                }
            }
        }

        #endregion

        #region == Command ==

        class ClickCommand : ICommand
        {
            public ClickCommand(Action clickAction)
            {
                ClickAction = clickAction;
            }

            private readonly Action ClickAction;

            public bool CanExecute(object parameter) => true;
            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                ClickAction?.Invoke();
            }
        }

        public ICommand Command { get; private set; }

        #endregion
    }
}
