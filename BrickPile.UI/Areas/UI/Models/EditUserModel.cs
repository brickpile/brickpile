namespace BrickPile.UI.Areas.UI.Models {
	public class EditUserModel {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
		public string Username { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
		public string Email { get; set; }
        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
		public string[] Roles { get; set; }
        /// <summary>
        /// Gets or sets the user roles.
        /// </summary>
        /// <value>
        /// The user roles.
        /// </value>
		public string[] UserRoles { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="EditUserModel"/> class.
        /// </summary>
		public EditUserModel() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="EditUserModel"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="email">The email.</param>
        /// <param name="roles">The roles.</param>
        /// <param name="userRoles">The user roles.</param>
		public EditUserModel(string username, string email, string[] roles, string[] userRoles) {
			this.Username = username;
			this.Email = email;
			this.Roles = roles;
			this.UserRoles = userRoles;
		}
	}
}
