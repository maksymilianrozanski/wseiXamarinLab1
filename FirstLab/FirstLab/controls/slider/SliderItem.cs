using FirstLab.styles;
using Xamarin.Forms;

namespace FirstLab.controls.slider
{
    public static class SliderItem
    {
        public static StackLayout CreateSlider(string name, int min, int max, string unit)
        {
            var title = new Label {Text = name, Style = SliderLabelStyle()};

            var sliderValue = new Label {Text = min + " " + unit, Style = SliderLabelStyle()};

            var slider = new Slider
            {
                Maximum = max,
                Minimum = min,
                Value = min,
                Style = SliderStyle()
            };

            slider.ValueChanged += (sender, args) => { sliderValue.Text = args.NewValue + " " + unit; };

            return new StackLayout
            {
                Children = {title, sliderValue, slider}
            };
        }

        private static Style SliderLabelStyle()
        {
            return new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter {Property = Label.FontSizeProperty, Value = 20},
                    new Setter {Property = Label.TextColorProperty, Value = Colors.TextColorMain},
                }
            };
        }

        private static Style SliderStyle()
        {
            return new Style(typeof(Label))
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
}