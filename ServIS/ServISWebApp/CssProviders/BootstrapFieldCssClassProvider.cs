using Microsoft.AspNetCore.Components.Forms;

namespace ServISWebApp.CssProviders
{
    /// <summary>
    /// Provides CSS class names for Bootstrap styling based on the state of the field in an EditContext.
    /// </summary>
    public class BootstrapFieldCssClassProvider : FieldCssClassProvider
	{
        /// <summary>
        /// Gets the CSS class(es) for the specified field in the given EditContext.
        /// </summary>
        /// <param name="editContext">The EditContext instance representing the form being edited.</param>
        /// <param name="fieldIdentifier">The identifier of the field.</param>
        /// <returns>The CSS class name(s) for the field.</returns>
        public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
		{
			string fieldCssClass;
			var isModified = editContext.IsModified(fieldIdentifier);
			var isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();

			switch (isModified, isValid)
			{
				case (true, true):
					fieldCssClass = "form-control modified is-valid";
					break;
				case (true, false):
					fieldCssClass = "form-control modified is-invalid";
					break;
				case (false, true):
					fieldCssClass = "form-control";
					break;
				case (false, false):
					fieldCssClass = "form-control";
					break;
			}

			return fieldCssClass;
		}
	}
}
