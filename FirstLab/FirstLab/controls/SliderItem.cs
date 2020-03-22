using Xamarin.Forms;

namespace FirstLab.controls
{
    public static class SliderItem
    {
        public static StackLayout CreateSlider(string name, int min, int max, string unit)
        {
            var title = new Label {Text = name};

            var sliderValue = new Label {Text = min + " " + unit};

            var slider = new Slider
            {
                Maximum = max,
                Minimum = min,
                Value = min
            };

            slider.ValueChanged += (sender, args) => { sliderValue.Text = args.NewValue + " " + unit; };

            return new StackLayout
            {
                Children = {title, sliderValue, slider}
            };
        }
    }
}