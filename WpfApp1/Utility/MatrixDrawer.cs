using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp1.Chemistry.Element;
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

        public List<Tuple<Element, Element>> DrawnConnections = new();

        public MatrixDrawer(Element[,] matrix, Canvas canvas) 
        {
            cts = new CancellationTokenSource();
            this.matrix = matrix;
            this.canvas = canvas;
        }

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
                    catch (Exception _)
                    {
                        return;
                    }

                    canvas.Children.Add(new VisualHost { Visual = visual });
                }
            }
        }

        private DrawingContext DrawElement(DrawingContext drawingContext, int x, int y)
        {
            Element element = matrix[x, y];
            if (element == null) return drawingContext;

            // Draw Element
            element.FinalizeFormula(canvas);
            Point formulaPoint = new(
                startingPoint.X + x * elementSpacing - element.FormulaSize.Width / 2,
                startingPoint.Y + y * elementSpacing - element.FormulaSize.Height / 2);

            Point textPoint = formulaPoint;
            foreach (var text in element.Formula)
            {
                if (text.style == TextStyle.Index)
                    textPoint.Y = formulaPoint.Y + element.Formula[0].formatted.Height * (1 - DrawingSettings.indexOverlapPercent);
                else
                    textPoint.Y = formulaPoint.Y;

                drawingContext.DrawText(text.formatted, textPoint);
                textPoint.X += text.formatted.Width;
            }

            // Draw Connections
            foreach (var connection in element.Connections)
            {
                Tuple<Element, Element> link = new(element, connection.Key);
                Tuple<Element, Element> swappedLink = new(connection.Key, element);

                bool exists = false;
                for (int i = 0; i < DrawnConnections.Count; i++)
                {
                    if (DrawnConnections[i] == link || DrawnConnections[i] == swappedLink)
                    {
                        exists = true; 
                        break;
                    }
                }
                if (exists) continue;

                var pos = MatrixUtil.TryGetElementPos(ref matrix, x, y, connection.Key);
                if (pos != null)
                {
                    matrix[pos.Item1, pos.Item2].FinalizeFormula(canvas);
                    drawingContext = DrawConnection(drawingContext, x, y, pos.Item1, pos.Item2, connection.Value);
                    DrawnConnections.Add(link);
                }
            }

            return drawingContext;
        }

        private DrawingContext DrawConnection(DrawingContext drawingContext, int x1, int y1, int x2, int y2, int strength)
        {
            var size1 = matrix[x1, y1].FormulaSize;
            var size2 = matrix[x2, y2].FormulaSize;

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

            var centerPoints = new Tuple<Point, Point>(
                GetCenterPoint(textOffsets.Item1, x1, y1, size1),
                GetCenterPoint(textOffsets.Item2, x2, y2, size2));

            bool even = strength % 2 == 0;
            for (int i = -strength / 2; i <= strength / 2; i++)
            {
                if (i == 0 && even) continue;

                var points = GetAdjustedPoints(centerPoints, textOffsets, i, even);
                drawingContext.DrawLine(new Pen(Brushes.Black, 1), points.Item1, points.Item2);
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

        private Tuple<Point, Point> GetAdjustedPoints(Tuple<Point, Point> centerPoints, Tuple<Point, Point> textOffsets, int i, bool even)
        {
            Point point1 = centerPoints.Item1, point2 = centerPoints.Item2;
            if (textOffsets.Item2.X == 0 || Math.Abs(textOffsets.Item2.X) == DrawingSettings.connectionPadding)
            {
                point1.Y += DrawingSettings.connectionSpacing * i;
                point2.Y += DrawingSettings.connectionSpacing * i;
                if (even)
                {
                    point1.Y -= DrawingSettings.connectionSpacing / 2 * Math.Sign(i);
                    point2.Y -= DrawingSettings.connectionSpacing / 2 * Math.Sign(i);
                }
            }
            else if (textOffsets.Item2.Y == 0 || Math.Abs(textOffsets.Item2.Y) == DrawingSettings.connectionPadding)
            {
                point1.X += DrawingSettings.connectionSpacing * i;
                point2.X += DrawingSettings.connectionSpacing * i;
                if (even)
                {
                    point1.X -= DrawingSettings.connectionSpacing / 2 * Math.Sign(i);
                    point2.X -= DrawingSettings.connectionSpacing / 2 * Math.Sign(i);
                }
            }

            return new Tuple<Point, Point>(point1, point2);
        }
    }
}
