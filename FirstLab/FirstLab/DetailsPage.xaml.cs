using System;
using FirstLab.controls.circle;
using FirstLab.controls.gridItem;
using FirstLab.controls.qualityText;
using FirstLab.controls.slider;
using FirstLab.viewModels;
using Xamarin.Forms;

namespace FirstLab
{
    public partial class DetailsPage : ContentPage
    {
        private StackLayout MyStackLayout => myStackLayout;

        public Frame Circle;

        public Frame QualityText;

        public Grid PMValuesGrid;

        public StackLayout HumiditySlider;

        public StackLayout PressureSlider;

        public DetailsPage(MeasurementVmItem vmItem)
        {
            InitializeComponent();
            var vm = new DetailsViewModel(Navigation, vmItem);
            this.BindingContext = vm;

            Circle = CircleFrame.CreateCircleFrame();

            QualityText = new Frame
            {
                Content = new StackLayout
                {
                    Children =
                    {
                        QualityTextLabel.CreateQualityText(),
                        QualityTextDescription.CreateQualityTextDescription(),
                    }
                }
            };

            PMValuesGrid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition
                    {
                        Width = GridLength.Star
                    }
                },

                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition
                    {
                        Height = GridLength.Star
                    },
                    new RowDefinition
                    {
                        Height = GridLength.Star
                    }
                }
            };

            PMValuesGrid.Children.Add(
                GridItem.CreateGridItem("PM 2.5", "mg/m3", DetailsViewModel.PmTwoPointFiveValueBindName,
                    DetailsViewModel.PmTwoPointFivePercentBindName), 0, 0);
            PMValuesGrid.Children.Add(
                GridItem.CreateGridItem("PM 10", "mg/m3", DetailsViewModel.PmTenValueBindName,
                    DetailsViewModel.PmTenPercentBindName), 1, 0);

            HumiditySlider = SliderItem.CreateSlider("Humidity", 0, 100, "%", DetailsViewModel.HumidityBindName);
            PressureSlider = SliderItem.CreateSlider("Pressure", 900, 1100, "hPa", DetailsViewModel.PressureBindName);

            MyStackLayout.Children.Add(Circle);
            MyStackLayout.Children.Add(QualityText);
            MyStackLayout.Children.Add(PMValuesGrid);
            MyStackLayout.Children.Add(HumiditySlider);
            MyStackLayout.Children.Add(PressureSlider);
        }
    }
}