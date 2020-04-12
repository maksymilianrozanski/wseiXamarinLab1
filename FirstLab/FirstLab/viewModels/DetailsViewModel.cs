using System;
using System.Linq;
using System.Threading;
using FirstLab.network.models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FirstLab.viewModels
{
    public class DetailsViewModel : BaseViewModel
    {
        public const string CaqiValueBindName = nameof(CaqiValue);
        public const string CaqiColorBindName = nameof(CaqiColor);
        public const string HumidityBindName = nameof(Humidity);
        public const string PressureBindName = nameof(Pressure);
        public const string QualityTextBindName = nameof(QualityText);
        public const string QualityDescriptionBindName = nameof(QualityDescription);
        public const string PmTwoPointFiveValueBindName = nameof(PmTwoPointFiveValue);
        public const string PmTwoPointFivePercentBindName = nameof(PmTwoPointFivePercent);
        public const string PmTenValueBindName = nameof(PmTenValue);
        public const string PmTenPercentBindName = nameof(PmTenPercent);

        public static readonly Func<string, Func<MeasurementVmItem, int>> ExtractIntValue =
            key =>
                vmItem =>
                {
                    if (vmItem.Measurements.current.values.Exists(it => it.name == key))
                        return Convert.ToInt32(vmItem.Measurements.current.values.First(it => it.name == key).value);
                    return -1;
                };

        private Color _caqiColor;
        private int _caqiValue;
        private int _humidity;
        private int _pmTenPercent;
        private int _pmTenValue;
        private int _pmTwoPointFivePercent;
        private int _pmTwoPointFiveValue;
        private int _pressure;
        private string _qualityDescription;
        private string _qualityText;

        public DetailsViewModel(INavigation navigation, MeasurementVmItem homePageViewModelItem) : base(navigation)
        {
            CaqiValue = ExtractCaqiValue(homePageViewModelItem);
            CaqiColor = ExtractColor(homePageViewModelItem);
            Humidity = ExtractIntValue("HUMIDITY")(homePageViewModelItem);
            Pressure = ExtractIntValue("PRESSURE")(homePageViewModelItem);
            QualityText = FirstIndex(homePageViewModelItem).description;

            var thread = new Thread(() =>
            {
                Thread.Sleep(1000);
                while (true)
                {
                    var now = DateTime.Now.Second;
                    QualityDescription = now % 2 == 0 ? "Hello World!" : "Hi World!";
                    PmTwoPointFiveValue = now;
                    PmTenValue = now;
                    PmTwoPointFivePercent = now;
                    PmTenPercent = now;
                }
            });
            thread.Start();
        }

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

        private static int ExtractCaqiValue(MeasurementVmItem vmItem)
        {
            return vmItem.Measurements.current.indexes.Count >= 0
                ? Convert.ToInt32(vmItem.Measurements.current.indexes[0].value)
                : 0;
        }

        public static Index FirstIndex(MeasurementVmItem vmItem)
        {
            return vmItem.Measurements.current.indexes?.ElementAtOrDefault(0) ?? new Index();
        }

        private static Color ExtractColor(MeasurementVmItem vmItem)
        {
            if (vmItem.Measurements.current.indexes.Count < 0) return Color.Fuchsia;
            return ColorConverters.FromHex(vmItem.Measurements.current.indexes[0].color);
        }
    }
}