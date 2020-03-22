using Xamarin.Forms;

namespace FirstLab.controls
{
    public class CircleFrame
    {
        public static Frame CreateCircleFrame()
        {
            var cAqiValue = new Label
            {
                FontSize = 32,
                Text = "56",
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Start
            };

            var circle = new Frame
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
                        cAqiValue,
                        new Label
                        {
                            Text = "CAQI",
                            HorizontalTextAlignment = TextAlignment.Center
                        }
                    }
                }
            };

            return circle;
        }
    }
}