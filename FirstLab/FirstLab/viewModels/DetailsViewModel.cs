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
                    CaqiValue = DateTime.Now.Second;
                    Humidity = DateTime.Now.Second;
                    Pressure = DateTime.Now.Second + 1000;
                }
            });
            thread.Start();
        }

        private int _caqiValue;
        private int _humidity;
        private int _pressure;

        public int CaqiValue
        {
            get => _caqiValue;
            set
            {
                if (value == _caqiValue) return;
                _caqiValue = value;
                OnPropertyChanged("CaqiValue");
            }
        }

        public int Humidity
        {
            get => _humidity;
            set
            {
                if (value == _humidity) return;
                _humidity = value;
                OnPropertyChanged("Humidity");
            }
        }

        public int Pressure
        {
            get => _pressure;
            set
            {
                if (value == _pressure) return;
                _pressure = value;
                OnPropertyChanged("Pressure");
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