using Xamarin.Forms;

namespace FirstLab.styles.gridItem
{
    public class GridItemStyle
    {
        public static ResourceDictionary CreateResourceDictionary()
        {
            var style = new Style(typeof(Span))
            {
                Setters = {new Setter {Property = Entry.TextColorProperty, Value = Colors.TextColorMain}}
            };

            return new ResourceDictionary {style};
        }
    }
}