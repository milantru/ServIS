using System.Timers;
using Timer = System.Timers.Timer;

namespace ServISWebApp.BackgroundServices
{
	public abstract class TimerService : BackgroundService
	{
		private readonly Timer timer;

		public TimerService(double interval)
		{
			timer = new(interval);
		}

		public TimerService(TimeSpan interval)
		{
			timer = new(interval.TotalMilliseconds);
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			InitTimer(stoppingToken);
			timer.Start();

			await Task.CompletedTask;
		}

		protected abstract Func<Task>? GetEventHandlers();

		private void InitTimer(CancellationToken stoppingToken)
		{
			// set stopping handler
			timer.Elapsed += (object? _, ElapsedEventArgs _) =>
			{
				if (stoppingToken.IsCancellationRequested)
				{
					timer.Stop();
					timer.Dispose();
				}
			};

			// set user handlers
			timer.Elapsed += async (object? _, ElapsedEventArgs _) =>
			{
				var userHandlers = GetEventHandlers();
				await (userHandlers?.Invoke() ?? Task.CompletedTask);
			};
		}
	}
}
