using Microsoft.AspNetCore.Components.Forms;

namespace ServISWebApp.Shared
{
	public static class FileTools
	{
		public static async Task<byte[]?> GetDataBytesAsync(this IBrowserFile file)
		{
			var memoryStream = await GetMemoryStreamAsync(file);

			var dataBytes = memoryStream.ToArray();

			return dataBytes;
		}

		public static string GetDataUrlBase64String(byte[] data, string format)
		{
			return $"data:{format};base64,{Convert.ToBase64String(data)}";
		}

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
			else if (imageFile.Size > 50_000)
			{
				errorMessage = "Súbor je príliš veľký"; // file is too large
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
