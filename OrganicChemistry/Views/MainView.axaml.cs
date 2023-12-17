using Avalonia;
using Avalonia.Controls;
using OrganicChemistry.ViewModels;

namespace OrganicChemistry.Views;

public partial class MainView : UserControl
{
    internal MainViewModel ViewModel => (MainViewModel)DataContext!;

    public MainView()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        ViewModel.Canvas = canvas;
    }
}
