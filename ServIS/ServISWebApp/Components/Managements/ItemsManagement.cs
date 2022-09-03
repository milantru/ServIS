using Microsoft.AspNetCore.Components;

namespace ServISWebApp.Components.Managements
{
	public abstract class ItemsManagement<ItemType> : ComponentBase where ItemType : class, new()
	{
		protected ItemsTableLister<ItemType> itemsLister = null!;

		public ItemType SelectedItem { get; protected set; } = new();

		public async Task ReloadItemsAsync()
		{
			await itemsLister.ReloadItemsAsync();
		}
	}
}
