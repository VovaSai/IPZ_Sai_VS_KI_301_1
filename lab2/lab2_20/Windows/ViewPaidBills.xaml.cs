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
        var allBills = new List<Bill>
        {
            new Bill { Id = 1, Name = "Electricity", Price = 120.5, Requisites = "1234567890", Days = 5, isPaid = true },
            new Bill { Id = 2, Name = "Water", Price = 80.75, Requisites = "9876543210", Days = 3, isPaid = true },
            new Bill { Id = 3, Name = "Internet", Price = 300.0, Requisites = "5555555555", Days = 10, isPaid = true },
            new Bill { Id = 4, Name = "Gas", Price = 150.0, Requisites = "111222333", Days = 7, isPaid = true },
            new Bill { Id = 5, Name = "Phone", Price = 60.0, Requisites = "444555666", Days = 2, isPaid = true },
            new Bill { Id = 6, Name = "Rent", Price = 5000.0, Requisites = "A-100", Days = 15, isPaid = true },
            new Bill { Id = 7, Name = "Insurance", Price = 1200.0, Requisites = "INS-987", Days = 20, isPaid = true },
            new Bill { Id = 8, Name = "TV Subscription", Price = 250.0, Requisites = "TV-555", Days = 8, isPaid = true },
            new Bill { Id = 9, Name = "Parking", Price = 400.0, Requisites = "PK-999", Days = 12, isPaid = true },
            new Bill { Id = 10, Name = "Gym", Price = 350.0, Requisites = "GYM-321", Days = 6, isPaid = true }
        };
        
        Bills = new ObservableCollection<Bill>(allBills.Where(b => b.isPaid));
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