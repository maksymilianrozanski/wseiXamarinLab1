using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace FirstLab.viewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }
        public virtual event PropertyChangedEventHandler PropertyChanged;

        protected BaseViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            void NotifyPropertyChanged() =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            NotifyPropertyChanged();
            return true;
        }
    }
}