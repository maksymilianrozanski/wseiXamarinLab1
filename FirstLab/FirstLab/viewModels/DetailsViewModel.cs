using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using FirstLab.location;
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
                    CaqiColor = now % 2 == 0 ? Color.MediumAquamarine : Color.Chartreuse;
                }
            });
            thread.Start();
        }

        public const string CaqiValueBindName = nameof(CaqiValue);
        private int _caqiValue;
        public const string CaqiColorBindName = nameof(CaqiColor);
        private Color _caqiColor;
        public const string HumidityBindName = nameof(Humidity);
        private int _humidity;
        public const string PressureBindName = nameof(Pressure);
        private int _pressure;
        public const string QualityTextBindName = nameof(QualityText);
        private string _qualityText;
        public const string QualityDescriptionBindName = nameof(QualityDescription);
        private string _qualityDescription;
        public const string PmTwoPointFiveValueBindName = nameof(PmTwoPointFiveValue);
        private int _pmTwoPointFiveValue;
        public const string PmTwoPointFivePercentBindName = nameof(PmTwoPointFivePercent);
        private int _pmTwoPointFivePercent;
        public const string PmTenValueBindName = nameof(PmTenValue);
        private int _pmTenValue;
        public const string PmTenPercentBindName = nameof(PmTenPercent);
        private int _pmTenPercent;

        public int PmTwoPointFivePercent
        {
            get => _pmTwoPointFivePercent;
            set
            {
                if (value == _pmTwoPointFivePercent) return;
                _pmTwoPointFivePercent = value;
                NotifyPropertyChanged();
            }
        }

        public int PmTwoPointFiveValue
        {
            get => _pmTwoPointFiveValue;
            set
            {
                if (value == _pmTwoPointFiveValue) return;
                _pmTwoPointFiveValue = value;
                NotifyPropertyChanged();
            }
        }

        public int PmTenPercent
        {
            get => _pmTenPercent;
            set
            {
                if (value == _pmTenPercent) return;
                _pmTenPercent = value;
                NotifyPropertyChanged();
            }
        }

        public int PmTenValue
        {
            get => _pmTenValue;
            set
            {
                if (value == _pmTenValue) return;
                _pmTenValue = value;
                NotifyPropertyChanged();
            }
        }

        public string QualityDescription
        {
            get => _qualityDescription;
            set
            {
                if (value == _qualityDescription) return;
                _qualityDescription = value;
                NotifyPropertyChanged();
            }
        }

        public int CaqiValue
        {
            get => _caqiValue;
            set
            {
                if (value == _caqiValue) return;
                _caqiValue = value;
                NotifyPropertyChanged();
            }
        }

        public int Humidity
        {
            get => _humidity;
            set
            {
                if (value == _humidity) return;
                _humidity = value;
                NotifyPropertyChanged();
            }
        }

        public int Pressure
        {
            get => _pressure;
            set
            {
                if (value == _pressure) return;
                _pressure = value;
                NotifyPropertyChanged();
            }
        }

        public string QualityText
        {
            get => _qualityText;
            set
            {
                if (value == _qualityText) return;
                _qualityText = value;
                NotifyPropertyChanged();
            }
        }

        public INavigation Navigation { get; set; }

        public Color CaqiColor
        {
            get => _caqiColor;
            set
            {
                if (value == _caqiColor) return;
                _caqiColor = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}