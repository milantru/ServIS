using Microsoft.AspNetCore.Components.Forms;

namespace ServISWebApp.CssProviders
{
	public class BootstrapFieldCssClassProvider : FieldCssClassProvider
	{
		public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
		{
			var isModified = editContext.IsModified(fieldIdentifier);
			var isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();

			return (isModified, isValid) switch
			{
				(true, true) => "form-control modified is-valid",
				(true, false) => "form-control modified is-invalid",
				(false, true) => "form-control",
				(false, false) => "form-control"
			};
		}
	}
}
