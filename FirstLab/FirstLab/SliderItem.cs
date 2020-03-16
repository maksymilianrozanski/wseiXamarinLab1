using Xamarin.Forms;

namespace FirstLab
{
    public static class SliderItem
    {
        public static StackLayout CreateSlider(string name, int min, int max, string unit)
        {
            var title = new Label {Text = name};

            var sliderValue = new Label {Text = min.ToString() + " " + unit};

            var slider = new Slider
            {
                Maximum = max,
                Minimum = min,
                Value = min
            };

            slider.ValueChanged += (sender, args) => { sliderValue.Text = args.NewValue.ToString() + " " + unit; };

            return new StackLayout
            {
                Children = {title, sliderValue, slider}
            };
        }
    }
}