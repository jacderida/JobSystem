using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobSystem.DataModel;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.Tests.Helpers
{
	public static class EntityIdProviderFactory
	{
		public static IEntityIdProvider GetEntityIdProviderFor<T>(string referenceToReturn)
		{
			var entityIdRepositoryStub = MockRepository.GenerateStub<IEntityIdProvider>();
			entityIdRepositoryStub.Stub(x => x.GetIdFor<T>()).Return(referenceToReturn);
			return entityIdRepositoryStub;
		}
	}
}