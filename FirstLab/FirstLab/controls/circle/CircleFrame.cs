using FirstLab.styles;
using Xamarin.Forms;

namespace FirstLab.controls.circle
{
    public class CircleFrame : ContentPage
    {
        public static Frame CreateCircleFrame()
        {
            var caqiValue = CaqiLabel.CreateCaqiLabel();

            var circleFrameStyle = CircleFrameStyle();

            var circle = new Frame
            {
                Style = circleFrameStyle,
                Content = new StackLayout
                {
                    Children =
                    {
                        caqiValue,
                        CaqiText.CreateCaqiTextLabel()
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
                    new Setter {Property = BackgroundColorProperty, Value = Colors.GoodAirQuality},
                    new Setter {Property = HeightRequestProperty, Value = 100},
                    new Setter {Property = WidthRequestProperty, Value = 100},
                    new Setter {Property = Frame.CornerRadiusProperty, Value = 100},
                    new Setter {Property = Frame.BorderColorProperty, Value = Colors.AccentColorSecondary},
                    new Setter {Property = View.VerticalOptionsProperty, Value = LayoutOptions.Start},
                    new Setter {Property = View.HorizontalOptionsProperty, Value = LayoutOptions.Center}
                }
            };
        }
    }
}