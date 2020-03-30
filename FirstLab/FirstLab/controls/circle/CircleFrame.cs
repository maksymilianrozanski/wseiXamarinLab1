using FirstLab.styles.circle;
using FirstLab.viewModels;
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
            circle.SetBinding(BackgroundColorProperty, new Binding(DetailsViewModel.CaqiColorBindName));
            return circle;
        }
    }
}