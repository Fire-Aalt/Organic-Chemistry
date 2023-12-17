using Avalonia;
using Avalonia.Controls;
using OrganicChemistry.Chemistry.Elements;
using OrganicChemistry.Config;
using OrganicChemistry.Controls;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OrganicChemistry.Utility
{
    public class MatrixDrawer(Element[,] matrix, Canvas canvas, int mainRowY)
    {
        public Element[,] matrix = matrix;
        public Canvas canvas = canvas;

        public Point startingPoint;
        public int elementSpacing;
        public int mainRow;
        public int mainRowY = mainRowY;

        public CancellationTokenSource cts = new();

        public List<Chemistry.ElementConnection> DrawnConnections = new();

        /// <summary>
        /// Draws matrix
        /// </summary>
        /// <param name="startingPoint"></param>
        /// <param name="spacing"></param>
        /// <param name="drawDelay"></param>
        /// <returns></returns>
        public async Task DrawMatrix(Point startingPoint, int spacing)
        {
            this.startingPoint = startingPoint;
            this.elementSpacing = spacing;

            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    if (matrix[x, y] == null) 
                        continue;

                    DrawElement(x, y);
                }
            }

            await Task.Delay(10);
        }

        /// <summary>
        /// Draws element at coordinates
        /// </summary>
        /// <param name="drawingContext"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private void DrawElement(int x, int y)
        {
            Element element = matrix[x, y];

            Point formulaPoint = new(
                startingPoint.X + x * elementSpacing,
                startingPoint.Y + y * elementSpacing);

            // Draw Element
            var formulaBlock = new ElementFormula { Element = element };
            Canvas.SetLeft(formulaBlock, formulaPoint.X);
            Canvas.SetTop(formulaBlock, formulaPoint.Y);
            canvas.Children.Add(formulaBlock);

            // Draw Order
            if (y == mainRowY && element is Carbon)
            {
                var orderBlock = new ElementOrder { Element = element };
                Canvas.SetLeft(orderBlock, formulaPoint.X);
                Canvas.SetTop(orderBlock, formulaPoint.Y);
                canvas.Children.Add(orderBlock);
            }

            // Draw Connections
            foreach (var connection in element.Connections)
            {
                Chemistry.ElementConnection elementConnection = new(element, connection.Key);
                if (DrawnConnections.Contains(elementConnection)) 
                    continue;

                var connectionBlock = new ElementConnection(element, connection.Key, connection.Value) 
                    { Indent = DrawingSettings.connectionSpacing, Spacing = elementSpacing };

                Canvas.SetLeft(connectionBlock, formulaPoint.X);
                Canvas.SetTop(connectionBlock, formulaPoint.Y);
                canvas.Children.Add(connectionBlock);

                DrawnConnections.Add(elementConnection);
            }
        }
    }
}