using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System;

namespace OrganicChemistry.Controls
{
    public class DoubleLine: Line
    {
        /// <summary>
        /// Defines the <see cref="Text"/> property.
        /// </summary>
        public static readonly StyledProperty<int> IndentProperty =
            AvaloniaProperty.Register<DoubleLine, int>(nameof(Indent), 2);

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public int Indent
        {
            get => GetValue(IndentProperty);
            set => SetValue(IndentProperty, value);
        }

        /// <summary>
        /// https://ru.stackoverflow.com/questions/456488/%D0%9A%D0%BE%D0%BE%D1%80%D0%B4%D0%B8%D0%BD%D0%B0%D1%82%D1%8B-%D0%BE%D1%82%D1%80%D0%B5%D0%B7%D0%BA%D0%B0-%D0%BF%D0%B0%D1%80%D0%B0%D0%BB%D0%BB%D0%B5%D0%BB%D1%8C%D0%BD%D0%BE%D0%B3%D0%BE-%D0%B7%D0%B0%D0%B4%D0%B0%D0%BD%D0%BD%D0%BE%D0%BC%D1%83-%D0%BE%D1%82%D1%80%D0%B5%D0%B7%D0%BA%D1%83
        /// </summary>
        /// <returns></returns>
        protected override Geometry CreateDefiningGeometry()
        {
            var lineLength = Math.Sqrt(Math.Pow(StartPoint.X - EndPoint.X, 2) + Math.Pow(StartPoint.Y - EndPoint.Y, 2));
            var vector = new Point(StartPoint.Y - EndPoint.Y, EndPoint.X - StartPoint.X);
            var ce = vector / lineLength * Indent;

            var startPoint1 = StartPoint + ce;
            var endPoint1 = EndPoint + ce;

            var startPoint2 = StartPoint - ce;
            var endPoint2 = EndPoint - ce;

            var line1 = new LineGeometry(startPoint1, endPoint1);
            var line2 = new LineGeometry(startPoint2, endPoint2);

            // Add all the geometries to a GeometryGroup.
            GeometryGroup myGeometryGroup = new();
            myGeometryGroup.Children.Add(line1);
            myGeometryGroup.Children.Add(line2);

            return myGeometryGroup;
        }
    }
}
