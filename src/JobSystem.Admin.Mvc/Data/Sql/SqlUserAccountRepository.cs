using System.Configuration;
using System.Data.SqlClient;
using JobSystem.Framework.Security;
using System;

namespace JobSystem.Admin.Mvc.Data.Sql
{
	public class SqlUserAccountRepository : IUserAccountRepository
	{
		private readonly ICryptographicService _cryptographicService;
		private const string GetUserQuery = "SELECT PasswordHash, PasswordSalt FROM UserAccounts WHERE EmailAddress = @EmailAddress";

		public SqlUserAccountRepository(ICryptographicService cryptographicService)
		{
			_cryptographicService = cryptographicService;
		}

		public bool Login(string username, string password)
		{
			var conn = GetConnection();
			try
			{
				conn.Open();
				var cmd = conn.CreateCommand();
				cmd.CommandText = GetUserQuery;
				cmd.Parameters.Add(new SqlParameter("@EmailAddress", username));
				using (var reader = cmd.ExecuteReader())
				{
					if (!reader.HasRows)
						return false;
					reader.Read();
					var hash = reader.GetString(0);
					var salt = reader.GetString(1);
					return hash == _cryptographicService.ComputeHash(password, salt);
				}
			}
			catch (SqlException)
			{
				throw;
			}
			finally
			{
				conn.Close();
			}
		}

		private SqlConnection GetConnection()
		{
			return new SqlConnection(ConfigurationManager.ConnectionStrings["TenantList"].ConnectionString);
		}
	}
}