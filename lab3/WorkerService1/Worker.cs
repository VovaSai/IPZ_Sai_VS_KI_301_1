using WorkerService1.service;

namespace test;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }
    
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            scope.ServiceProvider.GetRequiredService<BillService>();
            var tcpServer = scope.ServiceProvider.GetRequiredService<TCPServer>();
            await tcpServer.StartAsync(stoppingToken);
        }
    }

}