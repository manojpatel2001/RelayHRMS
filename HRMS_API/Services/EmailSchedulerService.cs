namespace HRMS_API.Services
{
    public class EmailSchedulerService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public EmailSchedulerService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("🔥 Scheduler Started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var service = scope.ServiceProvider.GetRequiredService<EmailJobService>();

                    await service.ProcessJobsAdvanced();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
