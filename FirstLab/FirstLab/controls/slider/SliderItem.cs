using FirstLab.styles;
using FirstLab.styles.slider;
using Xamarin.Forms;

namespace FirstLab.controls.slider
{
    public static class SliderItem
    {
        public static StackLayout CreateSlider(string name, int min, int max, string unit)
        {
            var title = new Label {Text = name, Style = SliderStyles.SliderLabelStyle()};

            var sliderValue = new Label {Text = min + " " + unit, Style = SliderStyles.SliderLabelStyle()};

            var slider = new Slider
            {
                Maximum = max,
                Minimum = min,
                Value = min,
                Style = SliderStyles.SliderStyle()
            };

            slider.ValueChanged += (sender, args) => { sliderValue.Text = args.NewValue + " " + unit; };

            return new StackLayout
            {
                Children = {title, sliderValue, slider}
            };
        }
    }
}