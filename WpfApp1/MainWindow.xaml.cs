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
        private double _interval = 20;
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
            int centerRow = _matrix.GetLength(0) / 2;
            for (int i = 0; i < numberOfElements; i++)
            {
                Element element = new Carbon();

                element.ConnectTo(MatrixUtil.TryGet(ref _matrix, centerRow + 1, i), 1);
                element.ConnectTo(MatrixUtil.TryGet(ref _matrix, centerRow - 1, i), 1);
                element.ConnectTo(MatrixUtil.TryGet(ref _matrix, centerRow, i + 1), 1);
                element.ConnectTo(MatrixUtil.TryGet(ref _matrix, centerRow, i - 1), 1);

                _matrix[centerRow, i] = element;
            }

            // Draw the matrix
            Point furthestCenter = new(100, 50);
            for (int i = 0; i < numberOfElements; i++)
            {
                Element element = _matrix[centerRow, i];
                FormattedText text = TextFormater.FormatText(element.GetName(), TextStyle.Element, this);
                double yCenter = text.Height / 2;
                Point textPoint = new(furthestCenter.X, furthestCenter.Y - yCenter);

                drawingContext.DrawText(text, textPoint);
                if (i != numberOfElements - 1)
                {
                    Point startPoint = new(furthestCenter.X + text.Width, furthestCenter.Y);
                    Point endPoint = new(furthestCenter.X + text.Width + _interval, furthestCenter.Y);
                    drawingContext.DrawLine(new Pen(Brushes.Black, 1), startPoint, endPoint);
                    furthestCenter = endPoint;
                }
            }

            // Close the DrawingContext to persist changes to the DrawingVisual.
            drawingContext.Close();

            return drawingVisual;
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
        }
    }
}
