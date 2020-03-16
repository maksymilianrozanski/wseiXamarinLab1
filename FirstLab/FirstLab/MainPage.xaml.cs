using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Color = System.Drawing.Color;

namespace FirstLab
{
    public partial class MainPage : ContentPage
    {
        private StackLayout MyStackLayout => myStackLayout;

        public Frame Circle;

        public Label CAqiValue;

        public Frame QualityText;

        public Grid PMValuesGrid;

        public StackLayout HumiditySlider;

        public StackLayout PressureSlider;

        public MainPage()
        {
            InitializeComponent();

            CAqiValue = new Label
            {
                FontSize = 32,
                Text = "56",
                TextColor = Xamarin.Forms.Color.Black,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Start
            };

            Circle = new Frame
            {
                BackgroundColor = Color.GreenYellow,
                HeightRequest = 100,
                WidthRequest = 100,
                CornerRadius = 100,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center,
                Content = new StackLayout
                {
                    Children =
                    {
                        CAqiValue,
                        new Label
                        {
                            Text = "CAQI",
                            HorizontalTextAlignment = TextAlignment.Center
                        }
                    }
                }
            };

            QualityText = new Frame
            {
                Content = new StackLayout
                {
                    Children =
                    {
                        new Label
                        {
                            Text = "Świetna jakość!",
                            FontAttributes = FontAttributes.Bold
                        },

                        new Label
                        {
                            Text = "Możesz bezpiecznie wyjść z domu"
                        }
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