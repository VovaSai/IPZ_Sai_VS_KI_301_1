using System.Windows;
using System.Windows.Input;

namespace lab2_20.Windows;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void CreateBillClick(object sender, RoutedEventArgs e)
    {
        var createBillClick = new CreateBill();
        createBillClick.Show();
        Close();
    }
    
    private void ViewBillsClick(object sender, RoutedEventArgs e)
    {
        var viewBills = new ViewBills();
        viewBills.Show();
    }
    
    private void ViewPaidBillsClick(object sender, RoutedEventArgs e)
    {
        var viewPaidBills = new ViewPaidBills();
        viewPaidBills.Show();
    }

    private void CloseClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
    
    private void DragWindow(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }

    private void MinimizeWindow(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void MaximizeWindow(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState == WindowState.Maximized 
            ? WindowState.Normal 
            : WindowState.Maximized;
    }
    private void CloseWindow(object sender, RoutedEventArgs e)
    {
        Close();
    }
}