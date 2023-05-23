using Syncfusion.Blazor;
using System.Resources;

namespace ServISWebApp.Shared
{
    /// <summary>
    /// Handles the localization of Syncfusion UI components.
    /// </summary>
    public class SyncfusionLocalizer : ISyncfusionStringLocalizer
	{
        /// <summary>
        /// Gets the resource manager used for localization.
        /// </summary>
        public ResourceManager ResourceManager => Resources.SfResources.ResourceManager;

        /// <summary>
        /// Retrieves the localized text for the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The name of the resource to retrieve.</param>
        /// <returns>The localized text if found; otherwise, an empty string.</returns>
        public string GetText(string key)
		{
			return ResourceManager.GetString(key) ?? "";
		}
	}
}
