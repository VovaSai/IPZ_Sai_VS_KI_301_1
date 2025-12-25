using System.Windows;
using System.Windows.Input;
using lab2_20.Entity;

namespace lab2_20.Windows;

public partial class CheckInfoBill : Window
{
    private Bill _bill;
    public CheckInfoBill(Bill bill)
    {
        _bill = bill;
        InitializeComponent();
        DataContext = _bill;
    }
    
    private void DragWindow(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }

    private void CloseWindow(object sender, RoutedEventArgs e)
    {
        Close();
    }
}