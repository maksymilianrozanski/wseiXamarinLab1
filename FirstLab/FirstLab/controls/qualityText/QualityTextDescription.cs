using Xamarin.Forms;

namespace FirstLab.controls.qualityText
{
    public static class QualityTextDescription
    {
        public static Label CreateQualityTextDescription()
        {
            return new Label
            {
                Text = "Możesz bezpiecznie wyjść z domu bez swojej maski anty-smogowej i nie bać się o swoje zdrowie.",
                Style = QualityTextDescriptionStyle()
            };
        }

        private static Style QualityTextDescriptionStyle()
        {
            return new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter {Property = Label.FontSizeProperty, Value = 14},
                    new Setter {Property = Label.TextColorProperty, Value = Color.Black},
                }
            };
        }
    }
}