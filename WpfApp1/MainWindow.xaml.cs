using System;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Algorithm;
using WpfApp1.Chemistry.Elements;
using WpfApp1.Utility;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MatrixDrawer? matrixDrawer = null;

        public Point startingPoint = new(100, 50);
        public int spacing = 50;
        public int drawDelay = 1;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Fill the matrix
        public async Task CreateMatrix()
        {
            var isomer = new IsomerAlgorithm(int.Parse(carbonBox.Text), 0, 0, 0, (checkBox.IsChecked ?? false));
            isomer.Start();

            await DrawMatrix(isomer.matrix);
        }

        // Draw the matrix
        public async Task DrawMatrix(Element[,] matrix)
        {
            matrixDrawer = new MatrixDrawer(matrix, canvas);

            await matrixDrawer.DrawMatrix(startingPoint, spacing, drawDelay);
        }

        public async Task ClearCanvas()
        {
            drawDelay = int.Parse(drawDelayBox.Text);
            matrixDrawer?.cts?.Cancel();

            await Task.Delay(drawDelay);
            canvas.Children.Clear();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await ClearCanvas();
            await CreateMatrix();
        }
    }
}
