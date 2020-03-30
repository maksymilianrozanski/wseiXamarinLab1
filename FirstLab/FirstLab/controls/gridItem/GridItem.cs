using FirstLab.styles.gridItem;
using Xamarin.Forms;

namespace FirstLab.controls.gridItem
{
    public static class GridItem
    {
        public static StackLayout CreateGridItem(string name, int value, string unit, int percentValue,
            string bindingNameMgM3, string bindingNamePercent)
        {
            var absoluteValueSpan = new Span {FontSize = 20};
            absoluteValueSpan.SetBinding(Span.TextProperty, bindingNameMgM3, BindingMode.OneWay);
            var percentValueSpan = new Span();
            percentValueSpan.SetBinding(Span.TextProperty, bindingNamePercent, BindingMode.OneWay,
                stringFormat: "{0}%");

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
                                absoluteValueSpan,
                                new Span {Text = " " + unit + " ", FontSize = 14},
                                percentValueSpan
                            }
                        }
                    }
                },
                Resources = GridItemStyle.CreateResourceDictionary()
            };
        }
    }
}