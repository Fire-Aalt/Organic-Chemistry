using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OrganicChemistry.Algorithm;
using OrganicChemistry.Chemistry.Elements;
using OrganicChemistry.Utility;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OrganicChemistry.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private string[] _isomerType = ["Alkane", "Alkene", "Alkyne", "Alkadiene"];

    [ObservableProperty]
    private string _isomerTypeSelectedItem;

    [ObservableProperty]
    [Range(0, 391)]
    private int _carbonNumber = 1;

    [ObservableProperty]
    private int _chlorineNumber = 0;

    [ObservableProperty]
    private int _bromNumber = 0;

    [ObservableProperty]
    private int _iodineNumber = 0;

    [ObservableProperty]
    private int _spacing = 50;

    private MatrixDrawer? _matrixDrawer;
    private readonly Point _startingPoint = new(100, 50);
    public int mainRowY;

    [ObservableProperty]
    private Canvas _canvas;

    [RelayCommand]
    public async Task Create()
    {
        await ClearCanvas();
        await CreateMatrix();
    }

    public MainViewModel()
    {
        IsomerTypeSelectedItem = IsomerType[0];
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
        _matrixDrawer = new MatrixDrawer(matrix, Canvas, mainRowY);

        await _matrixDrawer.DrawMatrix(_startingPoint, Spacing);
    }

    public async Task ClearCanvas()
    {
        _matrixDrawer?.cts?.Cancel();
        Canvas.Children.Clear();
    }
}
