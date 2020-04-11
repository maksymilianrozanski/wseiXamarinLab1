using System;
using System.Threading;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FirstLab.viewModels
{
    public class DetailsViewModel : BaseViewModel
    {
        public DetailsViewModel(INavigation navigation, MeasurementVmItem homePageViewModelItem) : base(navigation)
        {
            CaqiValue = ExtractCaqiValue(homePageViewModelItem);
            CaqiColor = ExtractColor(homePageViewModelItem);

            var thread = new Thread(() =>
            {
                Thread.Sleep(1000);
                while (true)
                {
                    var now = DateTime.Now.Second;
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

        private static int ExtractCaqiValue(MeasurementVmItem vmItem) =>
            vmItem.Measurements.current.indexes.Count >= 0
                ? Convert.ToInt32(vmItem.Measurements.current.indexes[0].value)
                : 0;

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

        private static Color ExtractColor(MeasurementVmItem vmItem)
        {
            if (vmItem.Measurements.current.indexes.Count < 0) return Color.Fuchsia;
            return ColorConverters.FromHex(vmItem.Measurements.current.indexes[0].color);
        }
    }
}