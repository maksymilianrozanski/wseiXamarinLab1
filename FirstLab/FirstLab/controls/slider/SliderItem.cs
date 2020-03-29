using FirstLab.styles;
using FirstLab.styles.slider;
using Xamarin.Forms;

namespace FirstLab.controls.slider
{
    public static class SliderItem
    {
        public static StackLayout CreateSlider(string name, int min, int max, string unit, string bindingName)
        {
            var title = new Label {Text = name, Style = SliderStyles.SliderLabelStyle()};

            var sliderValueLabel = new Label {Text = min + " " + unit, Style = SliderStyles.SliderLabelStyle()};

            var slider = new Slider
            {
                Maximum = max,
                Minimum = min,
                Value = min,
                Style = SliderStyles.SliderStyle()
            };
            slider.SetBinding(Slider.ValueProperty, new Binding(bindingName));

            slider.ValueChanged += (sender, args) => { sliderValueLabel.Text = args.NewValue + " " + unit; };

            return new StackLayout
            {
                Children = {title, sliderValueLabel, slider}
            };
        }
    }
}