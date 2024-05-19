using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Data;

public abstract class ObservableObject
{
    #region INotifyPropertyChanged

    public void RaisePropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion INotifyPropertyChanged
}