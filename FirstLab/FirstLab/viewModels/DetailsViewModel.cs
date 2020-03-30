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
                    QualityDescription = now % 2 == 0 ? "Hello World!" : "Hi World!";
                    PmTwoPointFiveValue = now;
                    PmTenValue = now;
                    PmTwoPointFivePercent = now;
                    PmTenPercent = now;
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
        public const string QualityDescriptionBindName = "QualityDescription";
        private string _qualityDescription;
        public const string PmTwoPointFiveValueBindName = nameof(PmTwoPointFiveValue);
        private int _pmTwoPointFiveValue;
        public const string PmTwoPointFivePercentBindName = nameof(PmTwoPointFivePercent);
        private int _pmTwoPointFivePercent;
        public const string PmTenValueBindName = nameof(PmTenValue);
        private int _pmTenValue;
        public const string PmTenPercentBindName =nameof(PmTenPercent);
        private int _pmTenPercent;

        public int PmTwoPointFivePercent
        {
            get => _pmTwoPointFivePercent;
            set
            {
                if (value == _pmTwoPointFivePercent) return;
                _pmTwoPointFivePercent = value;
                OnPropertyChanged(PmTwoPointFivePercentBindName);
            }
        }

        public int PmTwoPointFiveValue
        {
            get => _pmTwoPointFiveValue;
            set
            {
                if (value == _pmTwoPointFiveValue) return;
                _pmTwoPointFiveValue = value;
                OnPropertyChanged(PmTwoPointFiveValueBindName);
            }
        }

        public int PmTenPercent
        {
            get => _pmTenPercent;
            set
            {
                if (value == _pmTenPercent) return;
                _pmTenPercent = value;
                OnPropertyChanged(PmTenPercentBindName);
            }
        }

        public int PmTenValue
        {
            get => _pmTenValue;
            set
            {
                if (value == _pmTenValue) return;
                _pmTenValue = value;
                OnPropertyChanged(PmTenValueBindName);
            }
        }

        public string QualityDescription
        {
            get => _qualityDescription;
            set
            {
                if (value == _qualityDescription) return;
                _qualityDescription = value;
                OnPropertyChanged(QualityDescriptionBindName);
            }
        }

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