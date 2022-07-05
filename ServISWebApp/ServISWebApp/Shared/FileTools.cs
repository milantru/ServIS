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

		private static async Task<MemoryStream> GetMemoryStreamAsync(IBrowserFile file)
		{
			var stream = file.OpenReadStream();

			var ms = new MemoryStream();
			await stream.CopyToAsync(ms);

			return ms;
		}
	}
}
