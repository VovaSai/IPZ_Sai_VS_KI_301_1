using System.Windows;
using System.Windows.Input;
using lab2_20.Entity;
using lab2_20.Request;

namespace lab2_20.Windows;

public partial class ChangeBillWindow : Window
{
    private Bill _bill { get; set; }

    public ChangeBillWindow(Bill bill)
    {
        _bill = bill;
        InitializeComponent();
        DataContext = _bill;
    }

    private async void ChangeBillClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var success =
                await ChangeBill.RequestAsync(_bill.Id, _bill.Name, _bill.Price, _bill.Requisites, _bill.Days);
            if (success)
            {
                MessageBox.Show("Ви успішно оновили рахунок", "Створення рахунку");
                var viewBills = new ViewBills();
                viewBills.Show();
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
        var viewBills = new ViewBills();
        viewBills.Show();
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