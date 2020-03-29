using FirstLab.styles.qualityText;
using FirstLab.viewModels;
using Xamarin.Forms;

namespace FirstLab.controls.qualityText
{
    public class QualityTextLabel
    {
        public static Label CreateQualityText()
        {
            var label = new Label
            {
                Style = QualityTextStyles.QualityTextStyle()
            };
            label.SetBinding(Label.TextProperty, new Binding(DetailsViewModel.QualityTextBindName));
            return label;
        }
    }
}