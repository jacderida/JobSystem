using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Text;

namespace MSBuild.Community.Tasks.SqlServer
{
	public class CreateUserForLogin : Task
	{
		[Required] public string DatabaseServer { get; set; }
		[Required] public string DatabaseUserName { get; set; }
		[Required] public string DatabasePassword { get; set; }
		[Required] public string DatabaseName { get; set; }
		[Required] public string LoginName { get; set; }
		[Required] public string UserName { get; set; }

		/* A single string has been used rather than ITaskItem so that it's easier to pass a role list around when using the scripts.
		 * The list of user roles should be a comma separated value list. */
		[Required]
		public string UserRoles
		{
			get;
			set;
		}

		private List<string> _roles = new List<string> { "db_datareader" };
		private const string CreateUserQuery = "USE [{0}]; CREATE USER [{1}] FOR LOGIN [{2}];";

		public override bool Execute()
		{
			var conn = SqlServerHelper.GetDatabaseConnection(DatabaseServer, DatabaseName, DatabaseUserName, DatabasePassword);
			try
			{
				PopulateRoles();
				ValidateSuppliedRoles();
				conn.Open();
				CheckDatabaseExists(conn);
				CheckLoginExists(conn);
				LogExpectations();
				ExecuteCreateUserCommand(conn);
				ExecuteAddRoleCommands(conn);
				return true;
			}
			catch (Exception ex)
			{
				Log.LogErrorFromException(ex, true);
				return false;
			}
			finally
			{
				conn.Close();
			}
		}

		private void ValidateSuppliedRoles()
		{
			var invalidRoles = _roles.Where(t => !SqlServerHelper.ValidDatabaseLevelRoles.Contains(t.ToString())).ToList();
			if (invalidRoles.Count > 0)
			{
				var sb = new StringBuilder();
				foreach (var role in invalidRoles)
					sb.AppendFormat("{0}, ", role);
				var invalidRolesString = sb.ToString().TrimEnd(", ".ToCharArray());
				throw new InvalidOperationException(String.Format("The following of the supplied roles are invalid: {0}", invalidRolesString));
			}
		}

		private void PopulateRoles()
		{
			var roles = UserRoles.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
			roles.Where(r => !_roles.Contains(r)).ToList().ForEach(r => _roles.Add(r));
		}

		private void CheckDatabaseExists(SqlConnection conn)
		{
			if (!SqlServerHelper.DatabaseExists(conn, DatabaseName))
				throw new InvalidOperationException(String.Format("The database {0} does not exist on server {1}", DatabaseName, DatabaseUserName));
		}

		private void CheckLoginExists(SqlConnection conn)
		{
			if (!SqlServerHelper.LoginExists(conn, LoginName))
				throw new InvalidOperationException(String.Format("The login {0} does not exist on server {1}", LoginName, DatabaseUserName));
		}

		private void LogExpectations()
		{
			Log.LogMessage("Attempting to create user {0} for login {1} on server {2}", UserName, LoginName, DatabaseServer);
			Log.LogMessage("With the following roles:");
			foreach (var role in _roles)
				Log.LogMessage(role);
		}

		private void ExecuteCreateUserCommand(SqlConnection conn)
		{
			var cmd = GetCreateUserCommand(conn);
			cmd.ExecuteNonQuery();
		}

		private void ExecuteAddRoleCommands(SqlConnection conn)
		{
			foreach (var role in _roles)
			{
				var cmd = GetAddUserRoleCommand(conn, role);
				cmd.ExecuteNonQuery();
			}
		}

		private SqlCommand GetCreateUserCommand(SqlConnection conn)
		{
			var commandText = String.Format(CreateUserQuery, DatabaseName, UserName, LoginName);
			Log.LogMessage("Executing {0}", commandText);
			return new SqlCommand(commandText, conn);
		}

		private SqlCommand GetAddUserRoleCommand(SqlConnection conn, string role)
		{
			var cmd = new SqlCommand("sp_addrolemember", conn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(new SqlParameter("@rolename", role));
			cmd.Parameters.Add(new SqlParameter("@membername", UserName));
			return cmd;
		}
	}
}