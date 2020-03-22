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
                    new Label
                    {
                        FormattedText = new FormattedString
                            {Spans = {new Span {Text = name, TextColor = Color.Black, FontSize = 14}}}
                    },

                    new Label
                    {
                        FormattedText = new FormattedString
                        {
                            Spans =
                            {
                                new Span {Text = value.ToString(), TextColor = Color.Black, FontSize = 20},
                                new Span {Text = " " + unit, TextColor = Color.Black, FontSize = 14},
                                new Span {Text = " (" + percentValue + "%)"}
                            }
                        }
                    }
                }
            };
        }
    }
}