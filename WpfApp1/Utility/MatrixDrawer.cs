using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp1.Chemistry.Element;

namespace WpfApp1.Utility
{
    public class MatrixDrawer
    {
        public Element[,] matrix;
        public Canvas canvas;

        public Point startingPoint;
        public int spacing;

        public List<Tuple<Element, Element>> DrawnConnections = new();

        public MatrixDrawer(Element[,] matrix, Canvas canvas) 
        {
            this.matrix = matrix;
            this.canvas = canvas;
        }

        public async Task DrawMatrix(Point startingPoint, int spacing)
        {
            this.startingPoint = startingPoint;
            this.spacing = spacing;
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    if (matrix[x, y] == null) continue;

                    DrawingVisual visual = new();
                    using DrawingContext drawingContext = visual.RenderOpen();
                    DrawElement(drawingContext, x, y);

                    canvas.Children.Add(new VisualHost { Visual = visual });
                    await Task.Delay(100);
                }
            }
        }

        private DrawingContext DrawElement(DrawingContext drawingContext, int x, int y)
        {
            Element element = matrix[x, y];
            if (element == null) return drawingContext;

            // Draw Element
            var formattedText = element.GetFormattedName(canvas);
            Point textPoint = new(startingPoint.X + x * spacing - formattedText.Width / 2, startingPoint.Y + y * spacing - formattedText.Height / 2);

            drawingContext.DrawText(formattedText, textPoint);

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
                    drawingContext = DrawConnection(drawingContext, x, y, pos.Item1, pos.Item2, connection.Value);
                    DrawnConnections.Add(link);
                }
            }

            return drawingContext;
        }

        private DrawingContext DrawConnection(DrawingContext drawingContext, int x1, int y1, int x2, int y2, int strength)
        {
            var formattedText1 = matrix[x1, y1].GetFormattedName(canvas);
            var formattedText2 = matrix[x2, y2].GetFormattedName(canvas);

            Point textOffset1;
            Point textOffset2;
            if (x1 > x2)
            {
                textOffset1 = new(0, formattedText1.Height / 2);
                textOffset2 = new(formattedText2.Width, formattedText2.Height / 2);
            }
            else if (x1 < x2)
            {
                textOffset1 = new(formattedText1.Width, formattedText1.Height / 2);
                textOffset2 = new(0, formattedText2.Height / 2);
            }
            else if (y1 < y2)
            {
                textOffset1 = new(formattedText1.Width / 2, formattedText1.Height);
                textOffset2 = new(formattedText2.Width / 2, 0);
            }
            else
            {
                textOffset1 = new(formattedText1.Width / 2, 0);
                textOffset2 = new(formattedText2.Width / 2, formattedText2.Height);
            }

            Point point1 = new(textOffset1.X + startingPoint.X + x1 * spacing - formattedText1.Width / 2, textOffset1.Y + startingPoint.Y + y1 * spacing - formattedText1.Height / 2);
            Point point2 = new(textOffset2.X + startingPoint.X + x2 * spacing - formattedText2.Width / 2, textOffset2.Y + startingPoint.Y + y2 * spacing - formattedText2.Height / 2);

            drawingContext.DrawLine(new Pen(Brushes.Black, 1), point1, point2);
            return drawingContext;
        }
    }
}
