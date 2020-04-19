using FirstLab.styles.circle;
using Xamarin.Forms;

namespace FirstLab.controls.circle
{
    public class CaqiText
    {
        public static Label CreateCaqiTextLabel() =>
            new Label
            {
                Text = "CAQI",
                Style = CaqiCircleStyles.CaqiTextStyle()
            };
    }
}