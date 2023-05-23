using Microsoft.AspNetCore.Components.Forms;

namespace ServISWebApp.Shared
{
    /// <summary>
    /// Provides utility methods for working with files.
    /// </summary>
    public static class FileTools
	{
        /// <summary>
        /// Asynchronously reads the data bytes from the specified <paramref name="file"/>.
        /// </summary>
        /// <param name="file">The file to read.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the data bytes.</returns>
        public static async Task<byte[]> GetDataBytesAsync(this IBrowserFile file)
		{
			var memoryStream = await GetMemoryStreamAsync(file);

			var dataBytes = memoryStream.ToArray();

			return dataBytes;
		}

        /// <summary>
        /// Converts the specified <paramref name="data"/> bytes to a data URL base64 string with the given <paramref name="format"/>.
		/// <para>
		/// <example>
		/// E.g. for the images it is possible to use format <c>"images/jpeg"</c>.
		/// </example>
		/// </para>
        /// </summary>
        /// <param name="data">The data bytes to convert.</param>
        /// <param name="format">The format of the data.</param>
        /// <returns>The data URL base64 string.</returns>
        public static string GetDataUrlBase64String(byte[] data, string format)
		{
			return $"data:{format};base64,{Convert.ToBase64String(data)}";
		}

        /// <summary>
        /// Checks if the specified <paramref name="imageFile"/> is a valid image file.
        /// </summary>
        /// <param name="imageFile">The image file to validate.</param>
        /// <param name="errorMessage">When this method returns, contains an error message if validation fails; 
		/// otherwise, <c>null</c>.</param>
        /// <returns><c>true</c> if the image file is valid; otherwise, <c>false</c>.</returns>
        public static bool IsValidImageFile(this IBrowserFile imageFile, out string? errorMessage)
		{
			if (imageFile == null)
			{
				errorMessage = "Chýba súbor"; // missing image file
				return false;
			}
			else if (imageFile.ContentType != "image/jpeg")
			{
				errorMessage = "Nepodporovaný typ súboru"; // unsupported file type
				return false;
			}
			else if (imageFile.Size > 1_000_000)
			{
				errorMessage = "Súbor je príliš veľký"; // file is too large (> 1MB)
				return false;
			}
			else
			{
				errorMessage = null;
				return true;
			}
		}

		private static async Task<MemoryStream> GetMemoryStreamAsync(IBrowserFile file)
		{
			var stream = file.OpenReadStream();

			var ms = new MemoryStream();
			await stream.CopyToAsync(ms);

			return ms;
		}
	}
}
