using Microsoft.AspNetCore.Components;
using ServISData.Interfaces;

namespace ServISWebApp.Components.Forms
{
	public abstract class FormBase<ItemType> : ComponentBase where ItemType : class, IItem
	{
		[Parameter]
		public ItemType Item { get; set; } = null!;

		[Parameter]
		public EventCallback<ItemType> ItemChanged { get; set; }

		[Parameter]
		public EventCallback OnSave { get; set; }

		[Parameter]
		public Func<Task> AfterSaveAsync { get; set; } = null!;

		public abstract Task SaveItemAsync();

		public abstract Task ResetAsync();
	}
}
