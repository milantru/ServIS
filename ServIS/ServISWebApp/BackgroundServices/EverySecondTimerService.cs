namespace ServISWebApp.BackgroundServices
{
	public class EverySecondTimerService : TimerService
	{
		private static event Func<Task>? updateEvent;

		public EverySecondTimerService() : base(1000)
		{

		}

		public static void RegisterEventHandler(Func<Task> eventHandler)
		{
			updateEvent += eventHandler;
		}

		public static void UnregisterEventHandler(Func<Task> eventHandler)
		{
			updateEvent -= eventHandler;
		}

		protected override Func<Task>? GetEventHandlers() => updateEvent;
	}
}
