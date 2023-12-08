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

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await CreateMatrix();
        }

        public async Task CreateMatrix()
        {
            // Fill the matrix
            int numberOfElements = int.Parse(carbonBox.Text);
            _matrix = new Element[numberOfElements, numberOfElements];
            numberOfElements *= 3;
            var rng = new Random();
            for (int i = 0; i < numberOfElements; i++)
            {
                int x = rng.Next(0, _matrix.GetLength(0));
                int y = rng.Next(0, _matrix.GetLength(1));
                int strength = rng.Next(1, 4);

                if (_matrix[x, y] != null) continue;
                Element element = new Carbon();


                element.ConnectTo(MatrixUtil.TryGet(ref _matrix, x + 1, y), strength);
                element.ConnectTo(MatrixUtil.TryGet(ref _matrix, x - 1, y), strength);
                element.ConnectTo(MatrixUtil.TryGet(ref _matrix, x, y + 1), strength);
                element.ConnectTo(MatrixUtil.TryGet(ref _matrix, x, y - 1), strength);

                _matrix[x, y] = element;
            }

            await DrawMatrix();
        }

        public async Task DrawMatrix()
        {
            // Draw the matrix
            Point startingPoint = new(100, 50);
            var matrixDrawer = new MatrixDrawer(_matrix, canvas);

            await matrixDrawer.DrawMatrix(startingPoint, 50);
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
        }
    }
}
