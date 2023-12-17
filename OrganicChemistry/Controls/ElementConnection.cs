using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using OrganicChemistry.Chemistry;
using OrganicChemistry.Chemistry.Elements;
using OrganicChemistry.Config;
using System;

namespace OrganicChemistry.Controls
{
    public class ElementConnection(Element element1, Element element2, int strength) : Control
    {
        public static readonly StyledProperty<double> IndentProperty =
            AvaloniaProperty.Register<ElementConnection, double>(nameof(Indent), 2.0);


        public static readonly StyledProperty<double> SpacingProperty =
            AvaloniaProperty.Register<ElementConnection, double>(nameof(Spacing), 2.0);

        public double Indent
        {
            get => GetValue(IndentProperty);
            set => SetValue(IndentProperty, value);
        }

        public double Spacing
        {
            get => GetValue(SpacingProperty);
            set => SetValue(SpacingProperty, value);
        }

        private readonly Element _element1 = element1, _element2 = element2;
        private readonly int _strength = strength;

        private ConnectionAlignment _alignment;

        public override void Render(DrawingContext context)
        {
            var size1 = _element1.Formula.Size;
            var size2 = _element2.Formula.Size;

            Tuple<Point, Point> textOffsets;
            if (_element1.x < _element2.x)
            {
                _alignment = ConnectionAlignment.Horizontal;
                textOffsets = new Tuple<Point, Point>(
                    new(size1.Width + DrawingSettings.connectionPadding, size1.Height / 2),
                    new(-DrawingSettings.connectionPadding, size2.Height / 2));
            }
            else if (_element1.y < _element2.y)
            {
                _alignment = ConnectionAlignment.Vertical;
                textOffsets = new Tuple<Point, Point>(
                    new(size1.Width / 2, size1.Height),
                    new(size2.Width / 2, 0));
            }
            else
                return;

            var centerPoints = new PointsConnection(
                GetCenterPoint(_element1, textOffsets.Item1, size1),
                GetCenterPoint(_element2, textOffsets.Item2, size2));

            for (int i = -_strength / 2; i <= _strength / 2; i++)
            {
                if (i == 0 && _strength % 2 == 0) continue;

                var connection = GetAdjustedConnection(new PointsConnection(centerPoints), textOffsets, _strength, i);
                context.DrawLine(new Pen(Brushes.Black, DrawingSettings.connectionWidth), connection.point1, connection.point2);
            }
        }

        private Point GetCenterPoint(Element element, Point textOffset, Size size)
        {
            var point = new Point(
                textOffset.X - size.Width / 2,
                textOffset.Y - size.Height / 2
            );

            if (_alignment == ConnectionAlignment.Horizontal && element.x > _element1.x)
            {
                point += new Point(Spacing, 0);
            }
            else if (_alignment == ConnectionAlignment.Vertical && element.y > _element1.y)
            {
                point += new Point(0, Spacing);
            }

            return point;
        }

        private PointsConnection GetAdjustedConnection(PointsConnection connection, Tuple<Point, Point> textOffsets, int strength, int i)
        {
            if (_alignment == ConnectionAlignment.Horizontal)
            {
                connection.AddY(i * (Indent + DrawingSettings.connectionWidth));

                if (strength % 2 == 0)
                    connection.SubtractY(Indent / 2 * Math.Sign(i));
            }
            else if (_alignment == ConnectionAlignment.Vertical)
            {
                connection.AddX(i * (Indent + DrawingSettings.connectionWidth));

                if (strength % 2 == 0)
                    connection.SubtractX(Indent / 2 * Math.Sign(i));
            }

            return connection;
        }

        public enum ConnectionAlignment
        {
            Horizontal,
            Vertical
        }
    }
}
