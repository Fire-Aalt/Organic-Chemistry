using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace OrganicChemistry.Utility
{
    public static class TextFormater
    {
        public static Text FormatText(string text, TextStyle style, Visual visual)
        {
            switch (style)
            {
                case TextStyle.Element:
                    return new Text(new FormattedText(text,
                      CultureInfo.CurrentCulture,
                      FlowDirection.LeftToRight,
                      new Typeface("Verdana"),
                      14,
                      Brushes.Black,
                      VisualTreeHelper.GetDpi(visual).PixelsPerDip), style);
                case TextStyle.Index:
                    return new Text(new FormattedText(text,
                      CultureInfo.CurrentCulture,
                      FlowDirection.LeftToRight,
                      new Typeface("Verdana"),
                      10,
                      Brushes.Black,
                      VisualTreeHelper.GetDpi(visual).PixelsPerDip), style);
                case TextStyle.Order:
                    return new Text(new FormattedText(text,
                      CultureInfo.CurrentCulture,
                      FlowDirection.LeftToRight,
                      new Typeface("Verdana"),
                      12,
                      Brushes.Blue,
                      VisualTreeHelper.GetDpi(visual).PixelsPerDip), style);
                default:
                    goto case TextStyle.Element;
            }
        }
    }

    public enum TextStyle
    {
        Element,
        Index,
        Order
    }

    public class Text
    {
        public FormattedText formatted;
        public TextStyle style;

        public Text(FormattedText formatted, TextStyle style)
        {
            this.formatted = formatted;
            this.style = style;
        }
    }
}
