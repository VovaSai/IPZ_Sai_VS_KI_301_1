namespace WorkerService1.model;

public class Bill
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string Requisites { get; set; }
    public int Days { get; set; }
    public bool isPaid { get; set; }
}