using ServISData.Models;

namespace ServISWebApp.Auth
{
	public class UserSessionStorage
	{
		public User User { get; set; }

		public UserSessionStorage(User user)
		{
			User = user;
		}
	}
}
