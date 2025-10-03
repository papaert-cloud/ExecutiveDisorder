using System.Collections.Generic;
using Avalonia.Controls;
using ExecutiveDisorder.Avalonia.ViewModels;

namespace ExecutiveDisorder.Avalonia;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}
