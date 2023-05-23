using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace ServISWebApp.CssProviders
{
    /// <summary>
    /// Provides a custom CSS class provider for fields in a Blazor EditContext.
    /// </summary>
    /// <typeparam name="ProviderType">The type of the field CSS class provider to use.</typeparam>
    public class CustomCssClassProvider<ProviderType> : ComponentBase where ProviderType : FieldCssClassProvider, new()
    {
        /// <summary>
        /// Gets or sets the current EditContext for which the CSS class provider is applied.
        /// </summary>
        [EditorRequired, CascadingParameter]
        EditContext CurrentEditContext { get; set; } = null!;

        /// <summary>
        /// Gets or sets the instance of the field CSS class provider.
        /// </summary>
        public ProviderType Provider { get; set; } = new ProviderType();

        protected override void OnInitialized()
        {
            CurrentEditContext.SetFieldCssClassProvider(Provider);
        }
    }
}
