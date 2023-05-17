using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace ServISWebApp.CssProviders
{
    public class CustomCssClassProvider<ProviderType> : ComponentBase where ProviderType : FieldCssClassProvider, new()
    {
        [EditorRequired, CascadingParameter]
        EditContext CurrentEditContext { get; set; } = null!;
        public ProviderType Provider { get; set; } = new ProviderType();

        protected override void OnInitialized()
        {
            CurrentEditContext.SetFieldCssClassProvider(Provider);
        }
    }
}
