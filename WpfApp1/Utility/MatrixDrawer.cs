using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp1.Chemistry;
using WpfApp1.Chemistry.Elements;
using WpfApp1.Config;
using Point = System.Windows.Point;

namespace WpfApp1.Utility
{
    public class MatrixDrawer
    {
        public Element[,] matrix;
        public Canvas canvas;

        public Point startingPoint;
        public int elementSpacing;
        public int mainRow;

        public CancellationTokenSource cts;

        public List<ElementConnection> DrawnConnections = new();

        public MatrixDrawer(Element[,] matrix, Canvas canvas) 
        {
            cts = new CancellationTokenSource();
            this.matrix = matrix;
            this.canvas = canvas;
        }

        /// <summary>
        /// Draws matrix
        /// </summary>
        /// <param name="startingPoint"></param>
        /// <param name="spacing"></param>
        /// <param name="drawDelay"></param>
        /// <returns></returns>
        public async Task DrawMatrix(Point startingPoint, int spacing, int drawDelay)
        {
            this.startingPoint = startingPoint;
            this.elementSpacing = spacing;

            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    if (matrix[x, y] == null) continue;

                    DrawingVisual visual = new();
                    using DrawingContext drawingContext = visual.RenderOpen();
                    DrawElement(drawingContext, x, y);

                    try
                    {
                        await Task.Delay(drawDelay, cts.Token);
                    }
                    catch (Exception)
                    {
                        return;
                    }

                    canvas.Children.Add(new VisualHost { Visual = visual });
                }
            }
        }

        /// <summary>
        /// Draws element at coordinates
        /// </summary>
        /// <param name="drawingContext"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private DrawingContext DrawElement(DrawingContext drawingContext, int x, int y)
        {
            Element element = matrix[x, y];

            // Draw Element
            element.GenerateFormula(canvas);
            Point formulaPoint = new(
                startingPoint.X + x * elementSpacing - element.Formula.Size.Width / 2,
                startingPoint.Y + y * elementSpacing - element.Formula.Size.Height / 2);

            Point textPoint = formulaPoint;
            foreach (var text in element.Formula.Name)
            {
                if (text.style == TextStyle.Index)
                    textPoint.Y = formulaPoint.Y + element.Formula.Name[0].formatted.Height * (1 - DrawingSettings.indexOverlapPercent);
                else
                    textPoint.Y = formulaPoint.Y;

                drawingContext.DrawText(text.formatted, textPoint);
                textPoint.X += text.formatted.Width;
            }

            // Draw Connections
            foreach (var connection in element.Connections)
            {
                ElementConnection elementConnection = new(element, connection.Key);
                if (DrawnConnections.Contains(elementConnection)) continue;

                matrix[connection.Key.x, connection.Key.y].GenerateFormula(canvas);

                drawingContext = DrawConnection(drawingContext, x, y, connection.Key.x, connection.Key.y, connection.Value);
                DrawnConnections.Add(elementConnection);
            }

            return drawingContext;
        }

        /// <summary>
        /// Draws connection between 2 elements at coordinates with given strength
        /// </summary>
        /// <param name="drawingContext"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="strength"></param>
        /// <returns></returns>
        private DrawingContext DrawConnection(DrawingContext drawingContext, int x1, int y1, int x2, int y2, int strength)
        {
            var size1 = matrix[x1, y1].Formula.Size;
            var size2 = matrix[x2, y2].Formula.Size;

            Tuple<Point, Point> textOffsets;
            if (x1 < x2)
            {
                textOffsets = new Tuple<Point, Point>(
                    new(size1.Width + DrawingSettings.connectionPadding, size1.Height / 2),
                    new(-DrawingSettings.connectionPadding, size2.Height / 2));
            }
            else if (y1 < y2)
            {
                textOffsets = new Tuple<Point, Point>(
                    new(size1.Width / 2, size1.Height),
                    new(size2.Width / 2, 0));
            }
            else
                return drawingContext;

            var centerPoints = new PointsConnection(
                GetCenterPoint(textOffsets.Item1, x1, y1, size1),
                GetCenterPoint(textOffsets.Item2, x2, y2, size2));

            for (int i = -strength / 2; i <= strength / 2; i++)
            {
                if (i == 0 && strength % 2 == 0) continue;

                var connection = GetAdjustedConnection(new PointsConnection(centerPoints), textOffsets, strength, i);
                drawingContext.DrawLine(new Pen(Brushes.Black, DrawingSettings.connectionWidth), connection.point1, connection.point2);
            }

            return drawingContext;
        }

        private Point GetCenterPoint(Point textOffset, int x, int y, SizeF size)
        {
            return new Point(
                startingPoint.X + textOffset.X + x * elementSpacing - size.Width / 2,
                startingPoint.Y + textOffset.Y + y * elementSpacing - size.Height / 2
            );
        }

        private PointsConnection GetAdjustedConnection(PointsConnection connection, Tuple<Point, Point> textOffsets, int strength, int i)
        {
            if (textOffsets.Item2.X == 0 || Math.Abs(textOffsets.Item2.X) == DrawingSettings.connectionPadding)
            {
                connection.AddY(i * (DrawingSettings.connectionSpacing + DrawingSettings.connectionWidth));

                if (strength % 2 == 0)
                    connection.SubtractY(DrawingSettings.connectionSpacing / 2 * Math.Sign(i));
            }
            else if (textOffsets.Item2.Y == 0 || Math.Abs(textOffsets.Item2.Y) == DrawingSettings.connectionPadding)
            {
                connection.AddX(i * (DrawingSettings.connectionSpacing + DrawingSettings.connectionWidth));

                if (strength % 2 == 0)
                    connection.SubtractX(DrawingSettings.connectionSpacing / 2 * Math.Sign(i));
            }

            return connection;
        }
    }
}
