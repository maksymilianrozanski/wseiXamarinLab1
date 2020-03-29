using System;
using System.ComponentModel;
using System.Threading;
using Xamarin.Forms;

namespace FirstLab.viewModels
{
    public class DetailsViewModel : INotifyPropertyChanged
    {
        public DetailsViewModel(INavigation navigation)
        {
            Navigation = navigation;

            var thread = new Thread(() =>
            {
                Thread.Sleep(2000);
                while (true)
                {
                    CaqiValue = DateTime.Now.Second.ToString();
                }
            });
            thread.Start();

            Console.WriteLine("after thread start");
        }

        private string _caqiValue;

        public string CaqiValue
        {
            get => _caqiValue;
            set
            {
                if (value == _caqiValue) return;
                _caqiValue = value;
                OnPropertyChanged("CaqiValue");
            }
        }


        public INavigation Navigation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            else
            {
                Console.WriteLine("Property changed is null!");
            }
        }
    }
}