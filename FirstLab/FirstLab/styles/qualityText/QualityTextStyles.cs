using Xamarin.Forms;

namespace FirstLab.styles.qualityText
{
    public class QualityTextStyles
    {
        public static Style QualityTextStyle()
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

        public static Style QualityTextDescriptionStyle()
        {
            return new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter {Property = Label.FontSizeProperty, Value = 14},
                    new Setter {Property = Label.TextColorProperty, Value = Colors.TextColorMain},
                }
            };
        }
    }
}