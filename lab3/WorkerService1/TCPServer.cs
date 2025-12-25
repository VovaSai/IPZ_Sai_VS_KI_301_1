using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using WorkerService1.service;

public class TCPServer
{
    private readonly ILogger _logger;

    private readonly BillService _billService;

    public TCPServer(
        BillService billService,
        ILogger<TCPServer> logger)
    {
        _billService = billService;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 5000);
        listener.Start();
        _logger.LogInformation("TCP Server started on port 5000.");

        while (!cancellationToken.IsCancellationRequested)
        {
            var client = await listener.AcceptTcpClientAsync();
            _ = HandleClientAsync(client);
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        _logger.LogInformation("Client connected.");
        using (var stream = client.GetStream())
        {
            var buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(request);
            var command = data?["command"];

            try
            {
                if (command == "add_bill")
                {
                    var name = data?["name"];
                    var price = double.Parse(data?["price"], CultureInfo.InvariantCulture);
                    var requisites = data?["requisites"];
                    var days = int.Parse(data?["days"]);
                    
                    var (success, message) = await _billService.AddBill(name, price, requisites, days);
                    var responseMessage = new { success = success, message = message };
                    var jsonResponse = JsonSerializer.Serialize(responseMessage);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "change_bill")
                {
                    var id = int.Parse(data?["id"]);
                    var name = data?["name"];
                    var price = double.Parse(data?["price"], CultureInfo.InvariantCulture);
                    var requisites = data?["requisites"];
                    var days = int.Parse(data?["days"]);
                    var (success, message) = await _billService.ChangeBill(id, name, price, requisites, days);
                    var responseMessage = new { success = success, message = message };
                    var jsonResponse = JsonSerializer.Serialize(responseMessage);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "get_bills")
                {
                    var (success, message, bills) = await _billService.GetBill();
                    var responseMessage = new { success = success, message = message, bills = bills };
                    var jsonResponse = JsonSerializer.Serialize(responseMessage);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "get_paid_bills")
                {
                    var (success, message, bills) = await _billService.GetPaidBill();
                    var responseMessage = new { success = success, message = message, bills = bills };
                    var jsonResponse = JsonSerializer.Serialize(responseMessage);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "paid_bill")
                {
                    var id = int.Parse(data?["id"]);
                    var (success, message) = await _billService.BillIsPaid(id);
                    var responseMessage = new { success = success, message = message };
                    var jsonResponse = JsonSerializer.Serialize(responseMessage);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "delete_bill")
                {
                    var id = int.Parse(data?["id"]);
                    var (success, message) = await _billService.DeleteBill(id);
                    var responseMessage = new { success = success, message = message };
                    var jsonResponse = JsonSerializer.Serialize(responseMessage);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
            }
            catch (Exception ex)
            {
                var response = new { success = false.ToString(), message = ex.Message };
                var jsonResponse = JsonSerializer.Serialize(response);
                await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
            }
        }

        client.Close();
    }
}