using System.Globalization;
using Avalonia;
using Avalonia.Media;

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
                      Brushes.Black), style);
                case TextStyle.Index:
                    return new Text(new FormattedText(text,
                      CultureInfo.CurrentCulture,
                      FlowDirection.LeftToRight,
                      new Typeface("Verdana"),
                      10,
                      Brushes.Black), style);
                case TextStyle.Order:
                    return new Text(new FormattedText(text,
                      CultureInfo.CurrentCulture,
                      FlowDirection.LeftToRight,
                      new Typeface("Verdana"),
                      12,
                      Brushes.Blue), style);
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

    public class Text(FormattedText formatted, TextStyle style)
    {
        public FormattedText formatted = formatted;
        public TextStyle style = style;
    }
}
