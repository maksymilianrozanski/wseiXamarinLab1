using FirstLab.styles.circle;
using Xamarin.Forms;

namespace FirstLab.controls.circle
{
    public class CaqiLabel
    {
        public static Label CreateCaqiLabel()
        {
            return new Label
            {
                Text = "56",
                Style = CaqiCircleStyles.CaqiValueStyle()
            };
        }
    }
}