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
        var bills = await GetBills.ResponseAsync();
        foreach (var bill in bills)
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
                var success = await DeleteBill.RequestAsync(bill.Id);
                if (success)
                {
                    MessageBox.Show("Ви успішно видалили рахунок", "Видалення рахунку");
                    var viewBills = new ViewBills();
                    viewBills.Show();
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка");
            }
        }
    }

    private async void PaidClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Bill bill)
        {
            try
            {
                var success = await PaidBill.RequestAsync(bill.Id);
                if (success)
                {
                    MessageBox.Show("Ви успішно оплатили рахунок", "Оплата рахунку");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка");
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