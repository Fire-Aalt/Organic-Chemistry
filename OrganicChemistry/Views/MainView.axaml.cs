using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using OrganicChemistry.Algorithm;
using OrganicChemistry.Chemistry.Elements;
using OrganicChemistry.Utility;
using OrganicChemistry.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OrganicChemistry.Views;

public partial class MainView : UserControl
{

    private MainViewModel _mainViewModel;

    public MainView()
    {
        InitializeComponent();

        _mainViewModel = new MainViewModel();
        _mainViewModel.Canvas = canvas;

        DataContext = _mainViewModel;
    }


}
