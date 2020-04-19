using Xamarin.Forms;

namespace FirstLab.styles.slider
{
    public class SliderStyles
    {
        public static Style SliderLabelStyle() =>
            new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter {Property = Label.FontSizeProperty, Value = 20},
                    new Setter {Property = Label.TextColorProperty, Value = Colors.TextColorMain}
                }
            };

        public static Style SliderStyle() =>
            new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter {Property = Slider.ThumbColorProperty, Value = Colors.AccentColorPrimary},
                    new Setter {Property = Slider.MinimumTrackColorProperty, Value = Colors.AccentColorPrimary},
                    new Setter {Property = Slider.MaximumTrackColorProperty, Value = Colors.AccentColorSecondary}
                }
            };
    }
}