using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace JobSystem.BuildTasks
{
	public class CreateJobSystemUser : Task
	{
		[Required] public string DatabaseServer { get; set; }
		[Required] public string DatabaseUserName { get; set; }
		[Required] public string DatabasePassword { get; set; }
		[Required] public string DatabaseName { get; set; }
		[Required] public string EmailAddress { get; set; }
		[Required] public string Name { get; set; }
		[Required] public string JobTitle { get; set; }
		[Required] public string Password { get; set; }

		public override bool Execute()
		{
			var conn = SqlServerHelper.GetDatabaseConnection(DatabaseServer, DatabaseName, DatabaseUserName, DatabasePassword);
			try
			{
				conn.Open();
				var cmd = GetCreateUserCommand(conn);
				cmd.ExecuteNonQuery();
				return true;
			}
			catch (SqlException ex)
			{
				Log.LogErrorFromException(ex, true);
				return false;
			}
			finally
			{
				conn.Close();
			}
		}

		private SqlCommand GetCreateUserCommand(SqlConnection conn)
		{
			var salt = GenerateSalt();
			var cmd =
				new SqlCommand(
					"INSERT INTO UserAccounts(Id, EmailAddress, Name, JobTitle, PasswordHash, PasswordSalt) VALUES(@Id, @EmailAddress, @Name, @JobTitle, @PasswordHash, @PasswordSalt)", conn);
			cmd.Parameters.Add(new SqlParameter("@Id", Guid.NewGuid()));
			cmd.Parameters.Add(new SqlParameter("@EmailAddress", EmailAddress));
			cmd.Parameters.Add(new SqlParameter("@Name", Name));
			cmd.Parameters.Add(new SqlParameter("@JobTitle", JobTitle));
			cmd.Parameters.Add(new SqlParameter("@PasswordHash", ComputeHash(Password + salt)));
			cmd.Parameters.Add(new SqlParameter("@PasswordSalt", salt));
			return cmd;
		}

		private string GenerateSalt()
		{
			var rng = new RNGCryptoServiceProvider();
			var buffer = new byte[64];
			rng.GetBytes(buffer);
			return Convert.ToBase64String(buffer);
		}

		private string ComputeHash(string value)
		{
			var algorithm = SHA512.Create();
			byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
			return Convert.ToBase64String(hash);
		}
	}
}