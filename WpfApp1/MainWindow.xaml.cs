using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using WpfApp1.Chemistry.Element;
using WpfApp1.Utility;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Element[,] _matrix = new Element[0, 0];

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Add(new VisualHost { Visual = CreateDrawingVisualText() });
        }

        // Create a DrawingVisual that contains text.
        public DrawingVisual CreateDrawingVisualText()
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            // Fill the matrix
            int numberOfElements = int.Parse(carbonBox.Text);
            _matrix = new Element[numberOfElements, numberOfElements];
            numberOfElements *= 3;
            var rng = new Random();
            for (int i = 0; i < numberOfElements; i++)
            {
                Element element = new Carbon();

                int x = rng.Next(0, _matrix.GetLength(0));
                int y = rng.Next(0, _matrix.GetLength(1));

                element.ConnectTo(MatrixUtil.TryGet(ref _matrix, x + 1, y), 1);
                element.ConnectTo(MatrixUtil.TryGet(ref _matrix, x - 1, y), 1);
                element.ConnectTo(MatrixUtil.TryGet(ref _matrix, x, y + 1), 1);
                element.ConnectTo(MatrixUtil.TryGet(ref _matrix, x, y - 1), 1);

                _matrix[x, y] = element;
            }

            // Draw the matrix
            Point startingPoint = new(100, 50);
            var matrixDrawer = new MatrixDrawer(_matrix, drawingContext, this);

            matrixDrawer.DrawMatrix(startingPoint, 50);
            drawingContext.Close();

            return drawingVisual;
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
        }
    }
}
