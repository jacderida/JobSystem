using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobSystem.DataModel.Repositories;
using Rhino.Mocks;
using JobSystem.DataModel.Entities;
using System.Collections;

namespace JobSystem.TestHelpers.RepositoryHelpers
{
	public static class TestStandardRepositoryHelper
	{
		public static ITestStandardRepository GetTestStandardRepository_StubsGetById_ReturnsTestStandards(IList<Guid> testStandardIds)
		{
			var testStandardRepositoryStub = MockRepository.GenerateStub<ITestStandardRepository>();
			foreach (var id in testStandardIds)
			{
				if (id != Guid.Empty)
					testStandardRepositoryStub.Stub(x => x.GetById(id)).Return(GetTestStandard(id));
				else
					testStandardRepositoryStub.Stub(x => x.GetById(id)).Return(null);
			}
			return testStandardRepositoryStub;
		}

		public static ITestStandardRepository GetTestStandardRepository_StubsGetById_ReturnsTestStandard(Guid testStandardId)
		{
			var testStandardRepositoryStub = MockRepository.GenerateStub<ITestStandardRepository>();
			testStandardRepositoryStub.Stub(x => x.GetById(testStandardId)).Return(GetTestStandard(testStandardId));
			return testStandardRepositoryStub;
		}

		public static ITestStandardRepository GetTestStandardRepository_StubsGetById_ReturnsNull(Guid testStandardId)
		{
			var testStandardRepositoryStub = MockRepository.GenerateStub<ITestStandardRepository>();
			testStandardRepositoryStub.Stub(x => x.GetById(testStandardId)).Return(null);
			return testStandardRepositoryStub;
		}

		private static TestStandard GetTestStandard(Guid testStandardId)
		{
			return new TestStandard
			{
				Id = testStandardId,
				Description = "some test standard",
				SerialNo = "some serial no",
				CertificateNo = "some cert no"
			};
		}
	}
}