using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using OrganicChemistry.Chemistry.Elements;
using OrganicChemistry.Utility;

namespace OrganicChemistry.Controls
{
    public class ElementOrder : Control
    {
        /// <summary>
        /// Defines the <see cref="Element"/> property.
        /// </summary>
        public static readonly StyledProperty<Element> ElementProperty =
            AvaloniaProperty.Register<ElementOrder, Element>(nameof(Element));

        /// <summary>
        /// Gets or sets the text.  
        /// </summary>
        public Element Element
        {
            get => GetValue(ElementProperty);
            set => SetValue(ElementProperty, value);
        }

        /// <summary>
        /// Renders the control.
        /// </summary>
        /// <param name="context">The drawing context.</param>
        public override void Render(DrawingContext context)
        {
            if (Element == null) { return; }

            var text = TextFormater.FormatText(Element.x.ToString(), TextStyle.Order, this);

            Point orderPoint = new(-text.formatted.Width - Element.Formula.Size.Width / 2, -text.formatted.Height - Element.Formula.Size.Height / 2);

            context.DrawText(text.formatted, orderPoint);
        }
    }
}
