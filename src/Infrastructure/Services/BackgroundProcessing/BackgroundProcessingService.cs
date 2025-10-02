using Microsoft.Extensions.Hosting;

namespace Infrastructure.Services.BackgroundProcessing;

public class BackgroundProcessingService : BackgroundService
{
	private readonly BackgroundWorkQueue _backgroundWorkQueue;

	public BackgroundProcessingService(BackgroundWorkQueue backgroundWorkQueue)
	{
		_backgroundWorkQueue = backgroundWorkQueue;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			var job = await _backgroundWorkQueue.DequeueAsync(stoppingToken);
			try
			{
				await job(stoppingToken);
			}
			catch (OperationCanceledException)
			{
				break;
			}
		}
	}
}