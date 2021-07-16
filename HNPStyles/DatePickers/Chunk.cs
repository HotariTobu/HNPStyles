using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HNPStyles.Internal.DatePickers
{
    public class Chunk : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region == Days ==

        private readonly ObservableCollection<Day> _Days = new();
        public ObservableCollection<Day> Days => _Days;

        #endregion

        #region == IsSelected ==

        private bool _IsSelected;
        public bool IsSelected
        {
            get => _IsSelected;
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                }
            }
        }

        #endregion
    }
}
