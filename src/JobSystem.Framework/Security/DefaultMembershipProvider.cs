// <copyright company="Gael Limited">
// Copyright (c) 2010 All Right Reserved
// </copyright>
// <author></author>
// <email></email>
// <date>2010</date>
// <summary>
//	Complying with all copyright laws is the responsibility of the 
//	user. Without limiting rights under copyrights, neither the 
//	whole nor any part of this document may be reproduced, stored 
//	in or introduced into a retrieval system, or transmitted in any 
//	form or by any means (electronic, mechanical, photocopying, 
//	recording, or otherwise), or for any purpose without express 
//	written permission of Gael Limited.
// </summary>

using System.Web.Security;

namespace JobSystem.Framework.Security
{
	/// <summary>
	/// An IMembershipProvider that wraps the default configured provider (normally the SqlMembershipProvider from )
	/// </summary>
	public class DefaultMembershipProvider : IMembershipProvider
	{
		private readonly MembershipProvider _provider;

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultMembershipProvider"/> class.
		/// </summary>
		public DefaultMembershipProvider()
		{
			_provider = Membership.Provider;
		}

		private static DefaultMembershipProvider _instance = null;
		private static readonly object Padlock = new object();

		public static DefaultMembershipProvider Provider
		{
			get
			{
				lock (Padlock)
				{
					if (_instance == null) _instance = new DefaultMembershipProvider();
				}
				return _instance;
			}
		}

		/// <summary>
		/// Adds a new membership user to the data source.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Web.Security.MembershipUser"/> object populated with the information for the newly created user.
		/// </returns>
		/// <param name="username">The user name for the new user. </param><param name="password">The password for the new user. </param><param name="email">The e-mail address for the new user.</param><param name="passwordQuestion">The password question for the new user.</param><param name="passwordAnswer">The password answer for the new user</param><param name="isApproved">Whether or not the new user is approved to be validated.</param><param name="providerUserKey">The unique identifier from the membership data source for the user.</param><param name="status">A <see cref="T:System.Web.Security.MembershipCreateStatus"/> enumeration value indicating whether the user was created successfully.</param>
		public MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
		{
			return _provider.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);
		}

		/// <summary>
		/// Processes a request to update the password question and answer for a membership user.
		/// </summary>
		/// <returns>
		/// true if the password question and answer are updated successfully; otherwise, false.
		/// </returns>
		/// <param name="username">The user to change the password question and answer for. </param><param name="password">The password for the specified user. </param><param name="newPasswordQuestion">The new password question for the specified user. </param><param name="newPasswordAnswer">The new password answer for the specified user. </param>
		public bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
		{
			return _provider.ChangePasswordQuestionAndAnswer(username, password, newPasswordQuestion, newPasswordAnswer);
		}

		/// <summary>
		/// Gets the password for the specified user name from the data source.
		/// </summary>
		/// <returns>
		/// The password for the specified user name.
		/// </returns>
		/// <param name="username">The user to retrieve the password for. </param><param name="answer">The password answer for the user. </param>
		public string GetPassword(string username, string answer)
		{
			return _provider.GetPassword(username, answer);
		}

		/// <summary>
		/// Processes a request to update the password for a membership user.
		/// </summary>
		/// <returns>
		/// true if the password was updated successfully; otherwise, false.
		/// </returns>
		/// <param name="username">The user to update the password for. </param><param name="oldPassword">The current password for the specified user. </param><param name="newPassword">The new password for the specified user. </param>
		public bool ChangePassword(string username, string oldPassword, string newPassword)
		{
			return _provider.ChangePassword(username, oldPassword, newPassword);
		}

		/// <summary>
		/// Resets a user's password to a new, automatically generated password.
		/// </summary>
		/// <returns>
		/// The new password for the specified user.
		/// </returns>
		/// <param name="username">The user to reset the password for. </param><param name="answer">The password answer for the specified user. </param>
		public string ResetPassword(string username, string answer)
		{
			return _provider.ResetPassword(username, answer);
		}

		/// <summary>
		/// Updates information about a user in the data source.
		/// </summary>
		/// <param name="user">A <see cref="T:System.Web.Security.MembershipUser"/> object that represents the user to update and the updated information for the user. </param>
		public void UpdateUser(MembershipUser user)
		{
			_provider.UpdateUser(user);
		}

		/// <summary>
		/// Verifies that the specified user name and password exist in the data source.
		/// </summary>
		/// <returns>
		/// true if the specified username and password are valid; otherwise, false.
		/// </returns>
		/// <param name="username">The name of the user to validate. </param><param name="password">The password for the specified user. </param>
		public bool ValidateUser(string username, string password)
		{
			return _provider.ValidateUser(username, password);
		}

		/// <summary>
		/// Clears a lock so that the membership user can be validated.
		/// </summary>
		/// <returns>
		/// true if the membership user was successfully unlocked; otherwise, false.
		/// </returns>
		/// <param name="userName">The membership user whose lock status you want to clear.</param>
		public bool UnlockUser(string userName)
		{
			return _provider.UnlockUser(userName);
		}

		/// <summary>
		/// Gets user information from the data source based on the unique identifier for the membership user. Provides an option to update the last-activity date/time stamp for the user.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Web.Security.MembershipUser"/> object populated with the specified user's information from the data source.
		/// </returns>
		/// <param name="providerUserKey">The unique identifier for the membership user to get information for.</param><param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
		public MembershipUser GetUser(object providerUserKey, bool userIsOnline)
		{
			return _provider.GetUser(providerUserKey, userIsOnline);
		}

		/// <summary>
		/// Gets information from the data source for a user. Provides an option to update the last-activity date/time stamp for the user.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Web.Security.MembershipUser"/> object populated with the specified user's information from the data source.
		/// </returns>
		/// <param name="username">The name of the user to get information for. </param><param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user. </param>
		public MembershipUser GetUser(string username, bool userIsOnline)
		{
			return _provider.GetUser(username, userIsOnline);
		}

		/// <summary>
		/// Gets the user name associated with the specified e-mail address.
		/// </summary>
		/// <returns>
		/// The user name associated with the specified e-mail address. If no match is found, return null.
		/// </returns>
		/// <param name="email">The e-mail address to search for. </param>
		public string GetUserNameByEmail(string email)
		{
			return _provider.GetUserNameByEmail(email);
		}

		/// <summary>
		/// Removes a user from the membership data source. 
		/// </summary>
		/// <returns>
		/// true if the user was successfully deleted; otherwise, false.
		/// </returns>
		/// <param name="username">The name of the user to delete.</param><param name="deleteAllRelatedData">true to delete data related to the user from the database; false to leave data related to the user in the database.</param>
		public bool DeleteUser(string username, bool deleteAllRelatedData)
		{
			return _provider.DeleteUser(username, deleteAllRelatedData);
		}

		/// <summary>
		/// Gets a collection of all the users in the data source in pages of data.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Web.Security.MembershipUserCollection"/> collection that contains a page of <paramref name="pageSize"/><see cref="T:System.Web.Security.MembershipUser"/> objects beginning at the page specified by <paramref name="pageIndex"/>.
		/// </returns>
		/// <param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex"/> is zero-based.</param><param name="pageSize">The size of the page of results to return.</param><param name="totalRecords">The total number of matched users.</param>
		public MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
		{
			return _provider.GetAllUsers(pageIndex, pageSize, out totalRecords);
		}

		/// <summary>
		/// Gets the number of users currently accessing the application.
		/// </summary>
		/// <returns>
		/// The number of users currently accessing the application.
		/// </returns>
		public int GetNumberOfUsersOnline()
		{
			return _provider.GetNumberOfUsersOnline();
		}

		/// <summary>
		/// Gets a collection of membership users where the user name contains the specified user name to match.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Web.Security.MembershipUserCollection"/> collection that contains a page of <paramref name="pageSize"/><see cref="T:System.Web.Security.MembershipUser"/> objects beginning at the page specified by <paramref name="pageIndex"/>.
		/// </returns>
		/// <param name="usernameToMatch">The user name to search for.</param><param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex"/> is zero-based.</param><param name="pageSize">The size of the page of results to return.</param><param name="totalRecords">The total number of matched users.</param>
		public MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			return _provider.FindUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords);
		}

		/// <summary>
		/// Gets a collection of membership users where the e-mail address contains the specified e-mail address to match.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Web.Security.MembershipUserCollection"/> collection that contains a page of <paramref name="pageSize"/><see cref="T:System.Web.Security.MembershipUser"/> objects beginning at the page specified by <paramref name="pageIndex"/>.
		/// </returns>
		/// <param name="emailToMatch">The e-mail address to search for.</param><param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex"/> is zero-based.</param><param name="pageSize">The size of the page of results to return.</param><param name="totalRecords">The total number of matched users.</param>
		public MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			return _provider.FindUsersByEmail(emailToMatch, pageIndex, pageSize, out totalRecords);
		}

		/// <summary>
		/// Gets a value indicating whether the membership provider is configured to allow users to retrieve their passwords.
		/// </summary>
		/// <returns>
		/// true if the membership provider is configured to support password retrieval; otherwise, false. The default is false.
		/// </returns>
		public bool EnablePasswordRetrieval
		{
			get { return _provider.EnablePasswordRetrieval; }
		}

		/// <summary>
		/// Gets a value indicating whether the membership provider is configured to allow users to reset their passwords.
		/// </summary>
		/// <returns>
		/// true if the membership provider supports password reset; otherwise, false. The default is true.
		/// </returns>
		public bool EnablePasswordReset
		{
			get { return _provider.EnablePasswordReset; }
		}

		/// <summary>
		/// Gets a value indicating whether the membership provider is configured to require the user to answer a password question for password reset and retrieval.
		/// </summary>
		/// <returns>
		/// true if a password answer is required for password reset and retrieval; otherwise, false. The default is true.
		/// </returns>
		public bool RequiresQuestionAndAnswer
		{
			get { return _provider.RequiresQuestionAndAnswer; }
		}

		/// <summary>
		/// Gets or sets the name of the application using the custom membership provider.
		/// </summary>
		/// <returns>
		/// The name of the application using the custom membership provider.
		/// </returns>
		public string ApplicationName
		{
			get { return _provider.ApplicationName; }
			set { _provider.ApplicationName = value; }
		}

		/// <summary>
		/// Gets the number of invalid password or password-answer attempts allowed before the membership user is locked out.
		/// </summary>
		/// <returns>
		/// The number of invalid password or password-answer attempts allowed before the membership user is locked out.
		/// </returns>
		public int MaxInvalidPasswordAttempts
		{
			get { return _provider.MaxInvalidPasswordAttempts; }
		}

		/// <summary>
		/// Gets the number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.
		/// </summary>
		/// <returns>
		/// The number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.
		/// </returns>
		public int PasswordAttemptWindow
		{
			get { return _provider.PasswordAttemptWindow; }
		}

		/// <summary>
		/// Gets a value indicating whether the membership provider is configured to require a unique e-mail address for each user name.
		/// </summary>
		/// <returns>
		/// true if the membership provider requires a unique e-mail address; otherwise, false. The default is true.
		/// </returns>
		public bool RequiresUniqueEmail
		{
			get { return _provider.RequiresUniqueEmail; }
		}

		/// <summary>
		/// Gets a value indicating the format for storing passwords in the membership data store.
		/// </summary>
		/// <returns>
		/// One of the <see cref="T:System.Web.Security.MembershipPasswordFormat"/> values indicating the format for storing passwords in the data store.
		/// </returns>
		public MembershipPasswordFormat PasswordFormat
		{
			get { return _provider.PasswordFormat; }
		}

		/// <summary>
		/// Gets the minimum length required for a password.
		/// </summary>
		/// <returns>
		/// The minimum length required for a password. 
		/// </returns>
		public int MinRequiredPasswordLength
		{
			get { return _provider.MinRequiredPasswordLength; }
		}

		/// <summary>
		/// Gets the minimum number of special characters that must be present in a valid password.
		/// </summary>
		/// <returns>
		/// The minimum number of special characters that must be present in a valid password.
		/// </returns>
		public int MinRequiredNonAlphanumericCharacters
		{
			get { return _provider.MinRequiredNonAlphanumericCharacters; }
		}

		/// <summary>
		/// Gets the regular expression used to evaluate a password.
		/// </summary>
		/// <returns>
		/// A regular expression used to evaluate a password.
		/// </returns>
		public string PasswordStrengthRegularExpression
		{
			get { return _provider.PasswordStrengthRegularExpression; }
		}

	}
}
