using FirstLab.controls;
using FirstLab.controls.circle;
using FirstLab.controls.qualityText;
using Xamarin.Forms;

namespace FirstLab
{
    public partial class MainPage : ContentPage
    {
        private StackLayout MyStackLayout => myStackLayout;

        public Frame Circle;

        public Frame QualityText;

        public Grid PMValuesGrid;

        public StackLayout HumiditySlider;

        public StackLayout PressureSlider;

        public MainPage()
        {
            InitializeComponent();

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
                GridItem.CreateGridItem("PM 2.5", 34, "mg/m3", 137), 0, 0);
            PMValuesGrid.Children.Add(
                GridItem.CreateGridItem("PM 10", 67, "mg/m3", 135), 1, 0);

            HumiditySlider = SliderItem.CreateSlider("Humidity", 0, 100, "%");
            PressureSlider = SliderItem.CreateSlider("Pressure", 900, 1100, "hPa");

            MyStackLayout.Children.Add(Circle);
            MyStackLayout.Children.Add(QualityText);
            MyStackLayout.Children.Add(PMValuesGrid);
            MyStackLayout.Children.Add(HumiditySlider);
            MyStackLayout.Children.Add(PressureSlider);
        }
    }
}