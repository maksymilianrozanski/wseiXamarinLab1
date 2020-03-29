using FirstLab.styles.circle;
using FirstLab.viewModels;
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

            label.SetBinding(Label.TextProperty, new Binding(DetailsViewModel.CaqiValueBindName));

            return label;
        }
    }
}