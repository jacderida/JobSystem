using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel.Entities;

namespace JobSystem.BusinessLogic.UnitTests
{
	[TestFixture]
	public class DeliveryItemServiceTests
	{
		private DomainValidationException _domainValidationException;
		private PendingDeliveryItem _savedPendingItem;

		[SetUp]
		public void Setup()
		{
			_domainValidationException = null;
			_savedPendingItem = null;
		}

		[Test]
		public void CreatePending_ValidItemDetails_PendingItemCreatedSuccessfully()
		{

		}

		private void CreatePending(Guid id, Guid customerId, Guid jobItemId, bool beyondEconomicRepair)
		{
			try
			{

			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}
	}
}