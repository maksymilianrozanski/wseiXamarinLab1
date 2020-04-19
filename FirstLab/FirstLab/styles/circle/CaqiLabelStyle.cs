using Xamarin.Forms;

namespace FirstLab.styles.circle
{
    public static class CaqiCircleStyles
    {
        public static Style CaqiValueStyle() =>
            new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter {Property = Label.FontSizeProperty, Value = 32},
                    new Setter {Property = Label.TextColorProperty, Value = Colors.TextColorMain},
                    new Setter {Property = Label.HorizontalTextAlignmentProperty, Value = TextAlignment.Center},
                    new Setter {Property = Label.VerticalTextAlignmentProperty, Value = TextAlignment.Start}
                }
            };

        public static Style CaqiTextStyle() =>
            new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter {Property = Label.FontSizeProperty, Value = 12},
                    new Setter {Property = Label.TextColorProperty, Value = Colors.AccentColorTertiary},
                    new Setter {Property = Label.HorizontalTextAlignmentProperty, Value = TextAlignment.Center},
                    new Setter {Property = Label.VerticalTextAlignmentProperty, Value = TextAlignment.Start}
                }
            };

        public static Style CircleFrameStyle() =>
            new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter {Property = VisualElement.BackgroundColorProperty, Value = Colors.GoodAirQuality},
                    new Setter {Property = VisualElement.HeightRequestProperty, Value = 100},
                    new Setter {Property = VisualElement.WidthRequestProperty, Value = 100},
                    new Setter {Property = Frame.CornerRadiusProperty, Value = 100},
                    new Setter {Property = Frame.BorderColorProperty, Value = Colors.AccentColorSecondary},
                    new Setter {Property = View.VerticalOptionsProperty, Value = LayoutOptions.Start},
                    new Setter {Property = View.HorizontalOptionsProperty, Value = LayoutOptions.Center}
                }
            };
    }
}