using JobSystem.DataModel;

namespace JobSystem.DataAccess.NHibernate
{
	/// <summary>
	/// A factory for creating NHibernate Sessions
	/// </summary>
	public class RepositorySessionFactory : IRepositorySessionFactory	
	{
		/// <summary>
		/// Provides access to the database session
		/// </summary>
		private readonly DalConfiguration _dalConfiguration;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:RepositorySessionFactory"/> class.
		/// </summary>
		/// <param name="dalConfiguration">Provides access to the database session</param>
		public RepositorySessionFactory(DalConfiguration dalConfiguration)
		{
			_dalConfiguration = dalConfiguration;
		}

		/// <summary>
		/// Creates a new Nhibernate Session.
		/// </summary>
		/// <returns>A new NHibernate Session</returns>
		public IRepositorySessionScope NewSessionScope()
		{
			return new RepositorySessionScope(_dalConfiguration);
		}
	}
}