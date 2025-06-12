using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using Shiftly.Models;

namespace Shiftly.Services;

public class NotificationService : IAsyncDisposable
{
    private readonly ServiceBusSender _sender;
    private readonly ServiceBusClient _client;

    public NotificationService(IOptions<ServiceBusSettings> cfg)
    {
        _client = new ServiceBusClient(cfg.Value.ConnectionString);
        _sender = _client.CreateSender(cfg.Value.QueueName);
    }

    public async Task SendLeaveRequestEventAsync(LeaveRequest lr)
    {
        var msg = new ServiceBusMessage(System.Text.Json.JsonSerializer.Serialize(lr))
        {
            Subject = $"LeaveRequest:{lr.Status}"
        };
        await _sender.SendMessageAsync(msg);
    }

    public async ValueTask DisposeAsync()
    {
        await _sender.DisposeAsync();
        await _client.DisposeAsync();
    }
}