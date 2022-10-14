using Syncfusion.Blazor;
using System.Resources;

namespace ServISWebApp.Shared
{
	public class SyncfusionLocalizer : ISyncfusionStringLocalizer
	{
		public ResourceManager ResourceManager => ServISWebApp.Resources.SfResources.ResourceManager;

		public string GetText(string key)
		{
			return this.ResourceManager.GetString(key) ?? "";
		}
	}
}
