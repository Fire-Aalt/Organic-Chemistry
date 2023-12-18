using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OrganicChemistry.Algorithm;
using OrganicChemistry.Chemistry.Elements;
using OrganicChemistry.Utility;
using System;
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
    [NotifyDataErrorInfo]
    [Required]
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

    private readonly double _padding = 100;

    private int _mainRowY;
    private Point _min;

    [ObservableProperty]
    private Canvas? _canvas;

    [RelayCommand]
    public async Task Create()
    {
        ValidateAllProperties();

        if (HasErrors)
        {
            return;
        }

        ClearCanvas();

        await CreateMatrix();
    }

    public MainViewModel()
    {
        IsomerTypeSelectedItem = IsomerType[0];
    }

    // Fill the matrix
    public async Task CreateMatrix()
    {
        ArgumentNullException.ThrowIfNull(Canvas);

        var isomerAlgo = new IsomerAlgorithm(CarbonNumber, ChlorineNumber, BromNumber, IodineNumber, IsomerTypeSelectedItem);
        await isomerAlgo.Start();

        var width = isomerAlgo.max.X - isomerAlgo.min.X;
        var height = isomerAlgo.max.Y - isomerAlgo.min.Y;
        if (isomerAlgo.matrix.GetLength(0) == 0)
        {
            Canvas.Width = 0;
            Canvas.Height = 0;
        }
        else {
            Canvas.Width = width * Spacing + _padding * 2;
            Canvas.Height = height * Spacing + _padding * 2;
        }

        _min = isomerAlgo.min;
        _mainRowY = isomerAlgo.mainRowY;
        await DrawMatrix(isomerAlgo.matrix);
    }

    // Draw the matrix
    private async Task DrawMatrix(Element[,] matrix)
    {
        ArgumentNullException.ThrowIfNull(Canvas);

        _matrixDrawer = new MatrixDrawer(matrix, Canvas, _mainRowY);

        await _matrixDrawer.DrawMatrix(_padding, Spacing, _min);
    }

    private void ClearCanvas()
    {
        _matrixDrawer?.cts?.Cancel();
        Canvas?.Children.Clear();
    }
}
