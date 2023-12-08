using System;
using System.Threading.Tasks;
using System.Windows;
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
            int numberOfElements = int.Parse(carbonBox.Text);
            _matrix = new Element[numberOfElements, numberOfElements];

            int mainRow = _matrix.GetLength(0) / 2;
            if (_matrix.GetLength(0) % 2 == 0)
                mainRow--;

            numberOfElements *= 3;
            var rng = new Random();
            for (int i = 0; i < numberOfElements; i++)
            {
                int x = rng.Next(0, _matrix.GetLength(0));
                int y = rng.Next(0, _matrix.GetLength(1));
                int strength = rng.Next(1, 4);

                if (_matrix[x, y] != null) continue;
                Element element = new Carbon();

                if (y == mainRow || (checkBox.IsChecked ?? false))
                {
                    element.ConnectTo(MatrixUtil.TryGet(ref _matrix, x + 1, y), strength);
                    element.ConnectTo(MatrixUtil.TryGet(ref _matrix, x - 1, y), strength);
                }

                element.ConnectTo(MatrixUtil.TryGet(ref _matrix, x, y + 1), strength);
                element.ConnectTo(MatrixUtil.TryGet(ref _matrix, x, y - 1), strength);

                _matrix[x, y] = element;
            }

            await DrawMatrix();
        }

        // Draw the matrix
        public async Task DrawMatrix()
        {
            matrixDrawer = new MatrixDrawer(_matrix, canvas);

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
