using ServISData.Models;

namespace ServISWebApp.Auth
{
    /// <summary>
    /// Represents the local storage for the authenticated user information.
    /// </summary>
    public class UserLocalStorage
	{
        /// <summary>
        /// Gets or sets the user information stored in the local storage.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserLocalStorage"/> class.
        /// </summary>
        /// <param name="user">The user information to be stored.</param>
        public UserLocalStorage(User user)
		{
			User = user;
		}
	}
}
