namespace ServISWebApp.BackgroundServices
{
    /// <summary>
    /// Service for executing operations every second.
    /// </summary>
    public class EverySecondTimerService : TimerService
	{
		private static event Func<Task>? updateEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="EverySecondTimerService"/> class.
        /// </summary>
        public EverySecondTimerService() : base(1000)
		{

		}

        /// <summary>
        /// Registers an event handler to be executed every second.
        /// </summary>
        /// <param name="eventHandler">The event handler to register.</param>
        public static void RegisterEventHandler(Func<Task> eventHandler)
		{
			updateEvent += eventHandler;
		}

        /// <summary>
        /// Unregisters an event handler from being executed every second.
        /// </summary>
        /// <param name="eventHandler">The event handler to unregister.</param>
        public static void UnregisterEventHandler(Func<Task> eventHandler)
		{
			updateEvent -= eventHandler;
		}

		protected override Func<Task>? GetEventHandlers() => updateEvent;
	}
}
