using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HNPStyles.Internal.DatePickers
{
    public class Week : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region == Chunks ==

        private readonly ObservableCollection<Chunk> _Chunks = new();
        public ObservableCollection<Chunk> Chunks => _Chunks;

        #endregion
    }
}
