using FirstLab.styles;
using FirstLab.styles.qualityText;
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
            label.SetBinding(Label.TextProperty, new Binding("QualityText"));
            return label;
        }
    }
}