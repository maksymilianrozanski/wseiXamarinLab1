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
                Style = CaqiLabelStyle()
            };
        }

        private static Style CaqiLabelStyle()
        {
            return new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter {Property = Label.FontSizeProperty, Value = 32},
                    new Setter {Property = Label.TextColorProperty, Value = Color.Black},
                    new Setter {Property = Label.HorizontalTextAlignmentProperty, Value = TextAlignment.Center},
                    new Setter {Property = Label.VerticalTextAlignmentProperty, Value = TextAlignment.Start}
                }
            };
        }
    }
}