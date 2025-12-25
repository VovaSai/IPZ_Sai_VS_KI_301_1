using System.ComponentModel;

namespace lab2_20.Entity;

public class Bill
{
    public int Id { get; set; }
    private string _name { get; set; }
    private double _price { get; set; }
    private string _requisites { get; set; }
    private int _days { get; set; }
    public bool isPaid { get; set; }
    
    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }

    public double Price
    {
        get => _price;
        set
        {
            if (_price != value)
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }
    }

    public string Requisites
    {
        get => _requisites;
        set
        {
            if (_requisites != value)
            {
                _requisites = value;
                OnPropertyChanged(nameof(Requisites));
            }
        }
    }

    public int Days
    {
        get => _days;
        set
        {
            if (_days != value)
            {
                _days = value;
                OnPropertyChanged(nameof(Days));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}