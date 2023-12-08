﻿using System;
using System.Collections.Generic;
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
        public int elementSpacing;
        public int connectionSpacing = 5;
        public double connectionPadding = 1.5;

        public List<Tuple<Element, Element>> DrawnConnections = new();

        public MatrixDrawer(Element[,] matrix, Canvas canvas) 
        {
            this.matrix = matrix;
            this.canvas = canvas;
        }

        public async Task DrawMatrix(Point startingPoint, int spacing)
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

                    await Task.Delay(100);
                    canvas.Children.Add(new VisualHost { Visual = visual });
                }
            }
        }

        private DrawingContext DrawElement(DrawingContext drawingContext, int x, int y)
        {
            Element element = matrix[x, y];
            if (element == null) return drawingContext;

            // Draw Element
            var formattedText = element.GetFormattedName(canvas);
            Point textPoint = new(startingPoint.X + x * elementSpacing - formattedText.Width / 2, startingPoint.Y + y * elementSpacing - formattedText.Height / 2);

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

            Tuple<Point, Point> textOffsets;
            if (x1 < x2)
            {
                textOffsets = new Tuple<Point, Point>(
                    new(formattedText1.Width + connectionPadding, formattedText1.Height / 2),
                    new(-connectionPadding, formattedText2.Height / 2));
            }
            else if (y1 < y2)
            {
                textOffsets = new Tuple<Point, Point>(
                    new(formattedText1.Width / 2, formattedText1.Height),
                    new(formattedText2.Width / 2, 0));
            }
            else
                return drawingContext;

            var centerPoints = new Tuple<Point, Point>(
                GetCenterPoint(textOffsets.Item1, x1, y1, formattedText1),
                GetCenterPoint(textOffsets.Item2, x2, y2, formattedText2));

            bool even = strength % 2 == 0;
            for (int i = -strength / 2; i <= strength / 2; i++)
            {
                if (i == 0 && even) continue;

                var points = GetAdjustedPoints(centerPoints, textOffsets, i, even);
                drawingContext.DrawLine(new Pen(Brushes.Black, 1), points.Item1, points.Item2);
            }

            return drawingContext;
        }

        private Point GetCenterPoint(Point textOffset, int x, int y, FormattedText formattedText)
        {
            return new Point(
                startingPoint.X + textOffset.X + x * elementSpacing - formattedText.Width / 2,
                startingPoint.Y + textOffset.Y + y * elementSpacing - formattedText.Height / 2
            );
        }

        private Tuple<Point, Point> GetAdjustedPoints(Tuple<Point, Point> centerPoints, Tuple<Point, Point> textOffsets, int i, bool even)
        {
            Point point1 = centerPoints.Item1, point2 = centerPoints.Item2;
            if (textOffsets.Item2.X == 0 || Math.Abs(textOffsets.Item2.X) == connectionPadding)
            {
                point1.Y += connectionSpacing * i;
                point2.Y += connectionSpacing * i;
                if (even)
                {
                    point1.Y -= connectionSpacing / 2 * Math.Sign(i);
                    point2.Y -= connectionSpacing / 2 * Math.Sign(i);
                }
            }
            else if (textOffsets.Item2.Y == 0 || Math.Abs(textOffsets.Item2.Y) == connectionPadding)
            {
                point1.X += connectionSpacing * i;
                point2.X += connectionSpacing * i;
                if (even)
                {
                    point1.X -= connectionSpacing / 2 * Math.Sign(i);
                    point2.X -= connectionSpacing / 2 * Math.Sign(i);
                }
            }

            return new Tuple<Point, Point>(point1, point2);
        }
    }
}
