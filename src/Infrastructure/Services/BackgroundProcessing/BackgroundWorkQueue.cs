using System.Threading.Channels;

namespace Infrastructure.Services.BackgroundProcessing;

public class BackgroundWorkQueue
{
	private readonly Channel<Func<CancellationToken, ValueTask>> _queue;

	public BackgroundWorkQueue(int capacity)
	{
		var options = new BoundedChannelOptions(capacity)
		{
			FullMode = BoundedChannelFullMode.Wait
		};
		_queue = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(options);
	}

	public async ValueTask EnqueueAsync(Func<CancellationToken, ValueTask> job)
	{
		if (job is null)
		{
			throw new ArgumentNullException(nameof(job));
		}
		await _queue.Writer.WriteAsync(job);
	}

	public async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken) =>
		await _queue.Reader.ReadAsync(cancellationToken);
}