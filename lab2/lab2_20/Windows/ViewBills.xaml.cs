using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using lab2_20.Entity;
using lab2_20.Request;
using lab2_20.Response;

namespace lab2_20.Windows;

public partial class ViewBills : Window
{
    public ObservableCollection<Bill> Bills { get; set; } = new ObservableCollection<Bill>();
    public ViewBills()
    {
        InitializeComponent();
        LoadBills();
        DataContext = this;
    }

    private async void LoadBills()
    {
        var allBills = new List<Bill>
        {
            new Bill { Id = 1, Name = "Electricity", Price = 120.5, Requisites = "1234567890", Days = 5, isPaid = false },
            new Bill { Id = 2, Name = "Water", Price = 80.75, Requisites = "9876543210", Days = 3, isPaid = true },
            new Bill { Id = 3, Name = "Internet", Price = 300.0, Requisites = "5555555555", Days = 10, isPaid = false },
            new Bill { Id = 4, Name = "Gas", Price = 150.0, Requisites = "111222333", Days = 7, isPaid = false },
            new Bill { Id = 5, Name = "Phone", Price = 60.0, Requisites = "444555666", Days = 2, isPaid = true },
            new Bill { Id = 6, Name = "Rent", Price = 5000.0, Requisites = "A-100", Days = 15, isPaid = false },
            new Bill { Id = 7, Name = "Insurance", Price = 1200.0, Requisites = "INS-987", Days = 20, isPaid = true },
            new Bill { Id = 8, Name = "TV Subscription", Price = 250.0, Requisites = "TV-555", Days = 8, isPaid = false },
            new Bill { Id = 9, Name = "Parking", Price = 400.0, Requisites = "PK-999", Days = 12, isPaid = true },
            new Bill { Id = 10, Name = "Gym", Price = 350.0, Requisites = "GYM-321", Days = 6, isPaid = false }
        };
        
        Bills.Clear();
        
        foreach (var bill in allBills.Where(b => b.isPaid == false))
        {
            Bills.Add(bill);
        }
    }
    
    private void CheckInfoClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Bill bill)
        {
            var checkInfoBill = new CheckInfoBill(bill);
            checkInfoBill.Show();
        }
    }

    private void ChangeClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Bill bill)
        {
            var changeBill = new ChangeBillWindow(bill);
            changeBill.Show();
            Close();
        }
    }

    private async void DeleteClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Bill bill)
        {
            try
            {
                // Видаляємо рахунок з колекції
                Bills.Remove(bill);

                MessageBox.Show("Ви успішно видалили рахунок", "Видалення рахунку", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private async void PaidClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Bill bill)
        {
            try
            {
                bill.isPaid = true;

                MessageBox.Show("Ви успішно оплатили рахунок", "Оплата рахунку", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    
    
    private void DynamicListView(object sender, SizeChangedEventArgs e)
    {
        var listView = sender as ListView;
        if (listView != null && listView.View is GridView gridView)
        {
            // Загальна ширина з урахуванням вертикальної смуги прокрутки
            double totalWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth;

            // Пропорції колонок: 1 частина для Group Name і 5 частин для Actions
            double groupNameColumnWidth = totalWidth / 5; // 1 частина
            double actionsColumnWidth = groupNameColumnWidth * 4; // 5 частин

            // Задаємо ширину колонок
            if (gridView.Columns.Count > 0)
            {
                gridView.Columns[0].Width = groupNameColumnWidth; // Колонка "Group Name"
            }
            if (gridView.Columns.Count > 1)
            {
                gridView.Columns[1].Width = actionsColumnWidth; // Колонка "Actions"
            }
        }
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