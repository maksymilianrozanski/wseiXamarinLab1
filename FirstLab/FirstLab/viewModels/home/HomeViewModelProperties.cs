using System.Collections.Generic;
using System.Windows.Input;
using FirstLab.network;
using FirstLab.network.models;
using Xamarin.Forms;

namespace FirstLab.viewModels.home
{
    public partial class HomeViewModel
    {
        private string _errorMessage;
        private bool _isLoading;
        private List<MeasurementVmItem> _measurementVmItems;
        private List<MapLocation> _mapLocations;
        internal const double MaxInstallationDistance = 100.0;
        public ICommand MyCommand { get; set; }

        public ICommand ForceRefreshCommand => new Command(async () =>
            await LoadMultipleValues(ForceLoading));

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public List<MeasurementVmItem> MeasurementInstallationVmItems
        {
            get => _measurementVmItems;
            set => SetProperty(ref _measurementVmItems, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public List<MapLocation> MapLocations
        {
            get => _mapLocations;
            set => SetProperty(ref _mapLocations, value);
        }

        private readonly Network _network;
    }
}