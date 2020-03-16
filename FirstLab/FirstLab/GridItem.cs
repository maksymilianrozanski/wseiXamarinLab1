using Xamarin.Forms;

namespace FirstLab
{
    public static class GridItem
    {
        public static StackLayout CreateGridItem(string name, int value, string unit, int percentValue)
        {
            return new StackLayout
            {
                Children =
                {
                    new Label {Text = name},
                    new Label {Text = value + " " + unit + " (" + percentValue + "%)"}
                }
            };
        }
    }
}