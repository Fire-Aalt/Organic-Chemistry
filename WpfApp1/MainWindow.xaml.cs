using System;
using System.Threading.Tasks;
using System.Windows;
using OrganicChemistry.Algorithm;
using OrganicChemistry.Chemistry.Elements;
using OrganicChemistry.Utility;

namespace OrganicChemistry
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string[] IsomerType { get; set; } 
        public MatrixDrawer? matrixDrawer = null;

        public Point startingPoint = new(100, 50);
        public int spacing = 50;
        public int drawDelay = 0;

        public int mainRowY;

        public MainWindow()
        {
            InitializeComponent();

            IsomerType = new string[] { "Alkane", "Alkene", "Alkyne", "Alkadiene"};
            DataContext = this;
        }

        // Fill the matrix
        public async Task CreateMatrix()
        {
            var isomerAlgo = new IsomerAlgorithm(int.Parse(carbonBox.Text), int.Parse(chlorBox.Text), int.Parse(bromBox.Text), int.Parse(iodineBox.Text), isomerType.Text);
            isomerAlgo.Start();

            mainRowY = isomerAlgo.mainRowY;
            await DrawMatrix(isomerAlgo.matrix);
        }

        // Draw the matrix
        public async Task DrawMatrix(Element[,] matrix)
        {
            matrixDrawer = new MatrixDrawer(matrix, canvas, mainRowY);

            await matrixDrawer.DrawMatrix(startingPoint, spacing, drawDelay, createGrifCheckBox.IsChecked ??= false);
        }

        public async Task ClearCanvas()
        {
            drawDelay = 0;
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
