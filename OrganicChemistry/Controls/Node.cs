using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Globalization;

namespace OrganicChemistry.Controls
{
    public class Node: Control
    {
        /// <summary>
        /// Defines the <see cref="Text"/> property.
        /// </summary>
        public static readonly StyledProperty<string?> TextProperty =
            AvaloniaProperty.Register<Node, string?>(nameof(Text));

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string? Text
        {
            get => GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        /// Renders the control.
        /// </summary>
        /// <param name="context">The drawing context.</param>
        public override void Render(DrawingContext context)
        {
            // context.FillRectangle(Brushes.Aqua, new Rect(Bounds.Size));

            context.DrawEllipse(Brushes.Aqua, new Pen(Brushes.Black), new Rect(Bounds.Size));

            // Make the FormattedText object.
            Typeface typeface = new Typeface("Times New Roman");
            double em_size = Math.Min(Bounds.Width / 2, Bounds.Height / 2);
            FormattedText formatted_text = new FormattedText(
                Text, CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                typeface, em_size, Brushes.Blue);

            // Center the text horizontally.
            formatted_text.TextAlignment = TextAlignment.Center;

            /*
            var scale = LayoutHelper.GetLayoutScale(this);
            var padding = LayoutHelper.RoundLayoutThickness(Padding, scale, scale);
            var top = padding.Top;
            var left = padding.Left;
            */

            var top = (Bounds.Height - formatted_text.Height) / 2;
            var left = (Bounds.Width - formatted_text.Width) / 2;

            // Draw the text.
            context.DrawText(formatted_text, new Point(left, top));
        }
    }
}
