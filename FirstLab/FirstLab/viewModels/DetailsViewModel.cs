using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using FirstLab.location;
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

    public class DetailsViewModel : BaseViewModel
    {
        public DetailsViewModel(INavigation navigation) : base(navigation)
        {
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
            set => SetProperty(ref _pmTwoPointFivePercent, value);
        }

        public int PmTwoPointFiveValue
        {
            get => _pmTwoPointFiveValue;
            set => SetProperty(ref _pmTwoPointFiveValue, value);
        }

        public int PmTenPercent
        {
            get => _pmTenPercent;
            set => SetProperty(ref _pmTenPercent, value);
        }

        public int PmTenValue
        {
            get => _pmTenValue;
            set => SetProperty(ref _pmTenValue, value);
        }

        public string QualityDescription
        {
            get => _qualityDescription;
            set => SetProperty(ref _qualityDescription, value);
        }

        public int CaqiValue
        {
            get => _caqiValue;
            set => SetProperty(ref _caqiValue, value);
        }

        public int Humidity
        {
            get => _humidity;
            set => SetProperty(ref _humidity, value);
        }

        public int Pressure
        {
            get => _pressure;
            set => SetProperty(ref _pressure, value);
        }

        public string QualityText
        {
            get => _qualityText;
            set => SetProperty(ref _qualityText, value);
        }

        public Color CaqiColor
        {
            get => _caqiColor;
            set => SetProperty(ref _caqiColor, value);
        }
    }
}