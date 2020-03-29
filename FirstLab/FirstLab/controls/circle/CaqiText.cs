using FirstLab.styles;
using Xamarin.Forms;

namespace FirstLab.controls.circle
{
    public class CaqiText
    {
        public static Label CreateCaqiTextLabel()
        {
            return new Label
            {
                Text = "CAQI",
                Style = CaqiTextStyle()
            };
        }

        private static Style CaqiTextStyle()
        {
            return new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter {Property = Label.FontSizeProperty, Value = 12},
                    new Setter {Property = Label.TextColorProperty, Value = Colors.AccentColorTertiary},
                    new Setter {Property = Label.HorizontalTextAlignmentProperty, Value = TextAlignment.Center},
                    new Setter {Property = Label.VerticalTextAlignmentProperty, Value = TextAlignment.Start}
                }
            };
        }
    }
}