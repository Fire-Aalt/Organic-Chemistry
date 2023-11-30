using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace WpfApp1.Utility
{
    public static class TextFormater
    {
        public static FormattedText FormatText(string text, TextStyle style, Visual visual)
        {
            switch (style)
            {
                case TextStyle.Element:
                    return new FormattedText(text,
                      CultureInfo.CurrentCulture,
                      FlowDirection.LeftToRight,
                      new Typeface("Verdana"),
                      14,
                      Brushes.Black,
                      VisualTreeHelper.GetDpi(visual).PixelsPerDip);
                default:
                    goto case TextStyle.Element;
            }
        }

    }

    public enum TextStyle
    {
        Element
    }
}
