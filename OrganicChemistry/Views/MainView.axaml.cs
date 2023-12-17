using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using OrganicChemistry.Algorithm;
using OrganicChemistry.Chemistry.Elements;
using OrganicChemistry.Utility;
using System.Threading.Tasks;

namespace OrganicChemistry.Views;

public partial class MainView : UserControl
{
    private MatrixDrawer? _matrixDrawer;

    private readonly Point _startingPoint = new(100, 50);

    public string[] IsomerType { get; set; } = ["Alkane", "Alkene", "Alkyne", "Alkadiene"];

    public string IsomerTypeSelectedItem { get; set; }

    public int CarbonNumber { get; set; } = 1;

    public int ChlorineNumber { get; set; } = 0;

    public int BromNumber { get; set; } = 0;

    public int IodineNumber { get; set; } = 0;

    public bool CreateGrid { get; set; } = false;

    public int Spacing { get; set; } = 50;

    public int DrawDelay { get; set; } = 0;

    public int mainRowY;

    public MainView()
    {
        InitializeComponent();

        IsomerTypeSelectedItem = IsomerType.Length > 0 ? IsomerType[0] : string.Empty;
        DataContext = this;
    }

    // Fill the matrix
    public async Task CreateMatrix()
    {
        var isomerAlgo = new IsomerAlgorithm(CarbonNumber, ChlorineNumber, BromNumber, IodineNumber, IsomerTypeSelectedItem);
        await isomerAlgo.Start();

        mainRowY = isomerAlgo.mainRowY;
        await DrawMatrix(isomerAlgo.matrix);
    }

    // Draw the matrix
    public async Task DrawMatrix(Element[,] matrix)
    {
        _matrixDrawer = new MatrixDrawer(matrix, canvas, mainRowY);

        await _matrixDrawer.DrawMatrix(_startingPoint, Spacing, DrawDelay, CreateGrid);
    }

    public async Task ClearCanvas()
    {
        DrawDelay = 0;
        _matrixDrawer?.cts?.Cancel();

        await Task.Delay(DrawDelay);
        canvas.Children.Clear();
    }

    [RelayCommand]
    public async Task Create()
    {
        await ClearCanvas();
        await CreateMatrix();
    }
}
