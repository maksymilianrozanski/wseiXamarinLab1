using FirstLab.styles;
using FirstLab.styles.circle;
using Xamarin.Forms;

namespace FirstLab.controls.circle
{
    public class CircleFrame : ContentPage
    {
        public static Frame CreateCircleFrame()
        {
            var caqiValue = CaqiLabel.CreateCaqiLabel();

            var circleFrameStyle = CaqiCircleStyles.CircleFrameStyle();

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
    }
}