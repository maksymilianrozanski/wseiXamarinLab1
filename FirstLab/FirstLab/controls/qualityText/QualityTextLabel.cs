using FirstLab.styles;
using Xamarin.Forms;

namespace FirstLab.controls.qualityText
{
    public class QualityTextLabel
    {
        public static Label CreateQualityText()
        {
            return new Label
            {
                Text = "Świetna jakość!",
                Style = QualityTextStyle()
            };
        }

        private static Style QualityTextStyle()
        {
            return new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter {Property = Label.FontSizeProperty, Value = 20},
                    new Setter {Property = Label.TextColorProperty, Value = Colors.TextColorMain},
                    new Setter {Property = Label.FontProperty, Value = FontAttributes.Bold},
                    new Setter {Property = Label.HorizontalTextAlignmentProperty, Value = TextAlignment.Center},
                }
            };
        }
    }
}