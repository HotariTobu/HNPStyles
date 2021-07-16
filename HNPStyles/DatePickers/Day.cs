using System.ComponentModel;

namespace HNPStyles.Internal.DatePickers
{
    public class Day : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region == Number ==

        private int _Number;
        public int Number
        {
            get => _Number;
            set
            {
                if (_Number != value)
                {
                    _Number = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Number)));
                }
            }
        }

        #endregion

        #region == IsFaded ==

        private bool _IsFaded;
        /// <summary>
        /// If the day is out of the month, true. else false
        /// </summary>
        public bool IsFaded
        {
            get => _IsFaded;
            set
            {
                if (_IsFaded != value)
                {
                    _IsFaded = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsFaded)));
                }
            }
        }

        #endregion
    }
}
