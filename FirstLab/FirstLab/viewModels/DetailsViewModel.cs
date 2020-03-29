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
                Thread.Sleep(1000);
                while (true)
                {
                    var now = DateTime.Now.Second;
                    CaqiValue = now;
                    Humidity = now;
                    Pressure = now + 1000;
                    QualityText = now % 2 == 0 ? "Good" : "Bad";
                }
            });
            thread.Start();
        }

        public const string CaqiValueBindName = "CaqiValue";
        private int _caqiValue;
        public const string HumidityBindName = "Humidity";
        private int _humidity;
        public const string PressureBindName = "Pressure";
        private int _pressure;
        public const string QualityTextBindName = "QualityText";
        private string _qualityText;

        public int CaqiValue
        {
            get => _caqiValue;
            set
            {
                if (value == _caqiValue) return;
                _caqiValue = value;
                OnPropertyChanged(CaqiValueBindName);
            }
        }

        public int Humidity
        {
            get => _humidity;
            set
            {
                if (value == _humidity) return;
                _humidity = value;
                OnPropertyChanged(HumidityBindName);
            }
        }

        public int Pressure
        {
            get => _pressure;
            set
            {
                if (value == _pressure) return;
                _pressure = value;
                OnPropertyChanged(PressureBindName);
            }
        }

        public string QualityText
        {
            get => _qualityText;
            set
            {
                if (value == _qualityText) return;
                _qualityText = value;
                OnPropertyChanged(QualityTextBindName);
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