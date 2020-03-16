using System;
using System.Collections.Generic;
using System.Linq;
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

            MyStackLayout.Children.Add(Circle);
        }
    }
}