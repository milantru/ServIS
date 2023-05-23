using System.Timers;
using Timer = System.Timers.Timer;

namespace ServISWebApp.BackgroundServices
{
    /// <summary>
    /// Base abstract class for a timer-based background service.
    /// </summary>
    public abstract class TimerService : BackgroundService
	{
		private readonly Timer timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerService"/> class with the specified interval in milliseconds.
        /// </summary>
        /// <param name="interval">The interval between timer ticks in milliseconds.</param>
        public TimerService(double interval)
		{
			timer = new(interval);
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerService"/> class with the specified time span interval.
        /// </summary>
        /// <param name="interval">The time span representing the interval between timer ticks.</param>
        public TimerService(TimeSpan interval)
		{
			timer = new(interval.TotalMilliseconds);
		}

        /// <summary>
        /// Executes the background service asynchronously.
        /// </summary>
        /// <param name="stoppingToken">The cancellation token to stop the service.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			InitTimer(stoppingToken);
			timer.Start();

			await Task.CompletedTask;
		}

        /// <summary>
        /// Gets the event handlers for the timer elapsed event.
        /// </summary>
        /// <returns>The event handlers as a delegate that returns a <see cref="Task"/>.</returns>
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
