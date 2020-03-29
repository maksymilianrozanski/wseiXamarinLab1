using FirstLab.styles.circle;
using Xamarin.Forms;

namespace FirstLab.controls.circle
{
    public class CaqiLabel
    {
        public static Label CreateCaqiLabel()
        {
            var label = new Label
            {
                Style = CaqiCircleStyles.CaqiValueStyle()
            };

            label.SetBinding(Label.TextProperty, new Binding("CaqiValue"));

            return label;
        }
    }
}