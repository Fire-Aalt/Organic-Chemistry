using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Add(new VisualHost { Visual = CreateDrawingVisualText() });
            canvas.Height = 100;
            canvas.Width = 100;
        }

        // Create a DrawingVisual that contains text.
        public DrawingVisual CreateDrawingVisualText()
        {
            // Create an instance of a DrawingVisual.
            DrawingVisual drawingVisual = new DrawingVisual();

            // Retrieve the DrawingContext from the DrawingVisual.
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            // Draw a formatted text string into the DrawingContext.
            drawingContext.DrawText(
               new FormattedText(textBox1.Text,
                  CultureInfo.CurrentCulture,
                  FlowDirection.LeftToRight,
                  new Typeface("Verdana"),
                  36,
                  Brushes.Black,
                  VisualTreeHelper.GetDpi(this).PixelsPerDip),
                  new Point(200, 116));

            drawingContext.DrawLine(
                new Pen(Brushes.Red, 1), 
                new Point(0, 0), 
                new Point(100, 100));

            //drawingContext.DrawEllipse();

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
