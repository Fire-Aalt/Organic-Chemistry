using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using OrganicChemistry.Chemistry;
using OrganicChemistry.Chemistry.Elements;
using OrganicChemistry.Config;
using OrganicChemistry.Utility;

namespace OrganicChemistry.Controls
{
    public class ElementFormula: Control
    {
        /// <summary>
        /// Defines the <see cref="Element"/> property.
        /// </summary>
        public static readonly StyledProperty<Element> ElementProperty =
            AvaloniaProperty.Register<ElementFormula, Element>(nameof(Element));

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
            Formula formula = Element.Formula;
            if (formula == null) { return; }

            Point formulaPoint = new(
                -formula.Size.Width / 2,
                -formula.Size.Height / 2);

            Point textPoint = formulaPoint;
            foreach (var text in formula.Name)
            {
                if (text.style == TextStyle.Index)
                    textPoint = new Point(textPoint.X, formulaPoint.Y + formula.Name[0].formatted.Height * (1 - DrawingSettings.indexOverlapPercent));
                else
                    textPoint = new Point(textPoint.X, formulaPoint.Y);

                context.DrawText(text.formatted, textPoint);
                textPoint += new Point(text.formatted.Width, 0);
            }
        }
    }
}
