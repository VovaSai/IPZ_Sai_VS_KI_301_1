using System.Windows;
using System.Windows.Input;
using lab2_20.Entity;
using lab2_20.Request;

namespace lab2_20.Windows;

public partial class CreateBill : Window
{
    private Bill _bill { get; set; }
    public CreateBill()
    {
        InitializeComponent();
        _bill = new Bill();
        DataContext = _bill;
    }

    private async void CreateBillClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var success = await AddBill.RequestAsync(_bill.Name, _bill.Price, _bill.Requisites, _bill.Days);
            if (success)
            {
                MessageBox.Show("Ви успішно створили рахунок", "Створення рахунку");
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка: {ex.Message}", "Помилка");
        }
    }
    
    private void CloseClick(object sender, RoutedEventArgs e)
    {
        var mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
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
}