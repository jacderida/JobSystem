using System;
using System.Linq;
using JobSystem.DataModel.Repositories;
using NHibernate;
using NHibernate.Linq;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class EntityIdLookupRepository : IEntityIdLookupRepository
	{
		private readonly static object Padlock = new object();

		private ISession CurrentSession
		{
			get
			{
				return NHibernateSession.Current;
			}
		}

		public string GetNextId(string typeName)
		{
			string result;
			lock (Padlock)
			{
				var lookup = CurrentSession.Query<EntityIdLookup>().SingleOrDefault(lu => lu.EntityTypeName == typeName) ??
					new EntityIdLookup
					{
						Id = Guid.NewGuid(),
						EntityTypeName = typeName,
						NextId = 1
					};
				result = lookup.NextId.ToString();
				if (!String.IsNullOrEmpty(lookup.Prefix))
					result = String.Format("{0}{1}", lookup.Prefix, result);
				lookup.NextId++;
				CurrentSession.SaveOrUpdate(lookup);
			}
			return result;
		}
	}
}