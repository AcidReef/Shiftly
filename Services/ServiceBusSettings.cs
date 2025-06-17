namespace Shiftly.Services
{
    public class ServiceBusSettings
    {
        public string ConnectionString { get; set; } = default!;
        public string QueueName { get; set; } = default!;
    }
}