using System.Collections.Generic;
using global::NHibernate;

namespace JobSystem.DataAccess.NHibernate
{
	public interface ISessionStorage
	{
		IEnumerable<ISession> GetAllSessions();
		ISession GetSessionForKey(string factoryKey);
		void SetSessionForKey(string factoryKey, ISession session);
	}
}