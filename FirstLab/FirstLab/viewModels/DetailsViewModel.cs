using System;
using System.Collections.Generic;
using System.Linq;
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
        public const string QualityTextBindName = nameof(QualityDescription);
        public const string QualityDescriptionBindName = nameof(QualityAdvice);
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
        private string _qualityAdvice;
        private string _qualityDescription;

        private List<(Value, Standard)> _valuesWithStandards;

        public DetailsViewModel(INavigation navigation, MeasurementVmItem homePageViewModelItem) : base(navigation)
        {
            CaqiValue = ExtractCaqiValue(homePageViewModelItem);
            CaqiColor = ExtractColor(homePageViewModelItem);
            Humidity = ExtractIntValue("HUMIDITY")(homePageViewModelItem);
            Pressure = ExtractIntValue("PRESSURE")(homePageViewModelItem);
            QualityDescription = FirstIndex(homePageViewModelItem).description;
            QualityAdvice = FirstIndex(homePageViewModelItem).advice;
            ValuesWithStandards = ExtractValuesWithStandards(homePageViewModelItem);
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

        public string QualityAdvice
        {
            get => _qualityAdvice;
            set => SetProperty(ref _qualityAdvice, value);
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

        public string QualityDescription
        {
            get => _qualityDescription;
            set => SetProperty(ref _qualityDescription, value);
        }

        public Color CaqiColor
        {
            get => _caqiColor;
            set => SetProperty(ref _caqiColor, value);
        }

        public List<(Value, Standard)> ValuesWithStandards
        {
            get => _valuesWithStandards;
            set
            {
                SetProperty(ref _valuesWithStandards, value);
                PmTenValue = Convert.ToInt32(GetValueByName(ValuesWithStandards, "PM10").Item1.value);
                PmTenPercent = Convert.ToInt32(GetValueByName(_valuesWithStandards, "PM10").Item2.percent);
                PmTwoPointFiveValue = Convert.ToInt32(GetValueByName(_valuesWithStandards, "PM25").Item1.value);
                PmTwoPointFivePercent = Convert.ToInt32(GetValueByName(_valuesWithStandards, "PM25").Item2.percent);
            }
        }

        public static List<(Value, Standard)> ExtractValuesWithStandards(MeasurementVmItem vmItem)
        {
            var values = vmItem.Measurements.current.values;
            var standards = vmItem.Measurements.current.standards;
            return values.Join(standards, value => value.name, standard => standard.pollutant,
                (value, standard) => (value, standard)).ToList();
        }

        public static (Value, Standard) GetValueByName(List<(Value, Standard)> list, string name) =>
            list.FirstOrDefault(it => it.Item1.name == name);

        private static int ExtractCaqiValue(MeasurementVmItem vmItem) =>
            vmItem.Measurements.current.indexes.Count >= 0
                ? Convert.ToInt32(vmItem.Measurements.current.indexes[0].value)
                : 0;

        public static Index FirstIndex(MeasurementVmItem vmItem) =>
            vmItem.Measurements.current.indexes?.ElementAtOrDefault(0) ?? new Index();

        private static Color ExtractColor(MeasurementVmItem vmItem)
        {
            if (vmItem.Measurements.current.indexes.Count < 0) return Color.Fuchsia;
            return ColorConverters.FromHex(vmItem.Measurements.current.indexes[0].color);
        }
    }
}