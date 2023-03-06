using ServISData.Models;

namespace ServISWebApp.Auth
{
	public class UserLocalStorage
	{
		public User User { get; set; }

		public UserLocalStorage(User user)
		{
			User = user;
		}
	}
}
