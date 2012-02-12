using System;
using System.Collections.Generic;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class JobItemRepository : RepositoryBase<JobItem>, IJobItemRepository
	{
		public void EmitItemHistory(UserAccount createdBy, Guid jobItemId, int workTime, int overTime, string report, ListItemType workStatus, ListItemType workType, ListItemType workLocation)
		{
			var jobItem = CurrentSession.Get<JobItem>(jobItemId);
			var status = CurrentSession.Query<ListItem>().Where(li => li.Type == workStatus).Single();
			var location = CurrentSession.Query<ListItem>().Where(li => li.Type == workLocation).Single();
			var workTypeItem = CurrentSession.Query<ListItem>().Where(li => li.Type == workType).Single();
			var itemHistory = new ItemHistory
			{
				Id = Guid.NewGuid(),
				DateCreated = AppDateTime.GetUtcNow(),
				JobItem = jobItem,
				User = createdBy,
				WorkTime = workTime,
				OverTime = overTime,
				Report = report,
				Status = status,
				WorkLocation = location,
				WorkType = workTypeItem
			};
			CurrentSession.Save(itemHistory);
		}

		public IEnumerable<JobItem> GetJobItems(Guid jobId)
		{
			return CurrentSession.Query<JobItem>().Where(ji => ji.Job.Id == jobId);
		}
	}
}