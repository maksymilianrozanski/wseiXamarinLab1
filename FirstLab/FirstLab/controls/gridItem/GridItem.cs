using Xamarin.Forms;

namespace FirstLab.controls.gridItem
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
                            {Spans = {new Span {Text = name, FontSize = 14}}}
                    },

                    new Label
                    {
                        FormattedText = new FormattedString
                        {
                            Spans =
                            {
                                new Span {Text = value.ToString(), FontSize = 20},
                                new Span {Text = " " + unit, FontSize = 14},
                                new Span {Text = " (" + percentValue + "%)"}
                            }
                        }
                    }
                },
                Resources = CreateResourceDictionary()
            };
        }

        private static ResourceDictionary CreateResourceDictionary()
        {
            var style = new Style(typeof(Span))
            {
                Setters = {new Setter {Property = Entry.TextColorProperty, Value = Color.Black}}
            };

            return new ResourceDictionary {style};
        }
    }
}