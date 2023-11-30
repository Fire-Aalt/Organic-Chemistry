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
        public Element[,] Matrix = new Element[0, 0];

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
            // Create an instance of a DrawingVisual.
            DrawingVisual drawingVisual = new DrawingVisual();

            // Retrieve the DrawingContext from the DrawingVisual.
            DrawingContext drawingContext = drawingVisual.RenderOpen();


            Point furthestCenter = new(50, 50);
            int numberOfElements = int.Parse(carbonBox.Text);
            Matrix = new Element[numberOfElements, numberOfElements];

            for (int i = 0; i < numberOfElements; i++)
            {
                FormattedText text = TextFormater.FormatText("CH4", TextStyle.Element, this);
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
