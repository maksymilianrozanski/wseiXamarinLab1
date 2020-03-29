using FirstLab.styles;
using FirstLab.styles.qualityText;
using FirstLab.viewModels;
using Xamarin.Forms;

namespace FirstLab.controls.qualityText
{
    public static class QualityTextDescription
    {
        public static Label CreateQualityTextDescription()
        {
            var label = new Label
            {
                Text = "Możesz bezpiecznie wyjść z domu bez swojej maski anty-smogowej i nie bać się o swoje zdrowie.",
                Style = QualityTextStyles.QualityTextDescriptionStyle()
            };
            label.SetBinding(Label.TextProperty, new Binding(DetailsViewModel.QualityDescriptionBindName));
            return label;
        }
    }
}