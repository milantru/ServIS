using Syncfusion.Blazor.Popups;

namespace ServISWebApp.Shared
{
	/// <summary>
	/// Provides methods for displaying modal windows.
	/// </summary>
	public class Modals
	{
		private readonly SfDialogService dialogService;

		/// <summary>
		/// Initializes a new instance of the <see cref="Modals"/> class.
		/// </summary>
		/// <param name="dialogService">The dialog service used to display modal windows.</param>
		public Modals(SfDialogService dialogService)
        {
			this.dialogService = dialogService;
        }

		/// <summary>
		/// Displays a confirmation dialog asynchronously.
		/// </summary>
		/// <param name="confirmationMessage">The message to display in the confirmation dialog.</param>
		/// <param name="title">The title of the confirmation dialog (optional).</param>
		/// <returns>
		/// A task representing the asynchronous operation. The task result is <c>true</c> if the user confirms, <c>false</c> otherwise.
		/// </returns>
		public async Task<bool> DisplayConfirmationAsync(string confirmationMessage, string? title = null)
		{
			var dialogButtonOptions = new DialogButtonOptions { Content = "Zrušiť" };

			var dialogOptions = new DialogOptions
			{
				ShowCloseIcon = true,
				CancelButtonOptions = dialogButtonOptions
			};

			var isConfirmed = await dialogService.ConfirmAsync(
				content: confirmationMessage, 
				title: title, 
				options: dialogOptions
			);

			return isConfirmed;
		}
	}
}
