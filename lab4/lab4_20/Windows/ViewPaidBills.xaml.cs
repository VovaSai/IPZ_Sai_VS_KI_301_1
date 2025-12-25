using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using lab2_20.Entity;
using lab2_20.Response;

namespace lab2_20.Windows;

public partial class ViewPaidBills : Window
{
    public ObservableCollection<Bill> Bills { get; set; } = new ObservableCollection<Bill>();
    public ViewPaidBills()
    {
        InitializeComponent();
        LoadBills();
        DataContext = this;
    }

    private async void LoadBills()
    {
        var bills = await GetPaidBills.ResponseAsync();
        foreach (var bill in bills)
        {
            Bills.Add(bill);
        }
    }
    
    private void DynamicListView(object sender, SizeChangedEventArgs e)
    {
        var listView = sender as ListView;
        if (listView != null && listView.View is GridView gridView)
        {
            double totalWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth;
            double columnWidth = totalWidth / gridView.Columns.Count;

            foreach (var column in gridView.Columns)
            {
                column.Width = columnWidth;
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