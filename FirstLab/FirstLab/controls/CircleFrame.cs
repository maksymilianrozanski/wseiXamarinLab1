using Xamarin.Forms;

namespace FirstLab.controls
{
    public class CircleFrame : ContentPage
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

            var circleFrameStyle = CircleFrameStyle();

            var circle = new Frame
            {
                Style = circleFrameStyle,
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

        private static Style CircleFrameStyle()
        {
            return new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter {Property = BackgroundColorProperty, Value = Color.GreenYellow},
                    new Setter {Property = HeightRequestProperty, Value = 100},
                    new Setter {Property = WidthRequestProperty, Value = 100},
                    new Setter {Property = Frame.CornerRadiusProperty, Value = 100},
                    new Setter {Property = Frame.BorderColorProperty, Value = Color.Gray},
                    new Setter {Property = View.VerticalOptionsProperty, Value = LayoutOptions.Start},
                    new Setter {Property = View.HorizontalOptionsProperty, Value = LayoutOptions.Center}
                }
            };
        }
    }
}