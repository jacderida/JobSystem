using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IJobItemRepository : IReadWriteRepository<JobItem, Guid>
	{
		/// <summary>
		/// Emits a piece of item history for a job item.
		/// </summary>
		/// <param name="createdBy">The user who performed the action..</param>
		/// <param name="jobItemId">The ID of the job item that the action relates to.</param>
		/// <param name="workTime">The work time spent on the action.</param>
		/// <param name="overTime">The overtime spent on the action.</param>
		/// <param name="report">A report for the action.</param>
		/// <param name="workStatus">The status of the job item after the action was performed.</param>
		/// <param name="workType">The type of work performed on the action.</param>
		/// <remarks>
		/// It is the responsibility of callers of this interface to ensure that valid and sensible values are passed in to the implementing repository.
		/// 
		/// I made a choice not to make a public interface in the business logic for creating item history, as this would potentially allow callers to
		/// put the system into a completely nonsensical state, by passing in arbitrary values for any of the arguments.
		/// 
		/// Item history should be emitted as a result of actions that can change the status of a job item, such as, when the job item
		/// is consigned to a supplier, which should happen in a public method on a consignment item service.
		/// 
		/// That public method wouldn't expose any details whatsoever about the item history; it determines what should be emitted.
		/// 
		/// This means that when you test that method, you can mock out this repository and verify that this method is called and the correct,
		/// sensible values for whatever particular context are passed in as arguments.
		/// 
		/// As well as callers making sure that sensible values are passed for status, location and type, it is also them who should make sure
		/// that there is a test to catch out the case when a bad job item ID gets passed in, and throw an exception at the calling level. Implementors
		/// of this interface should be free to assume that no null reference exception will result. If it does, it's the fault of the caller.
		/// </remarks>
		void EmitItemHistory(UserAccount createdBy, Guid jobItemId, int workTime, int overTime, string report, ListItemType workStatus, ListItemType workType);
		ConsignmentItem GetLatestConsignmentItem(Guid jobItemId);
		PendingConsignmentItem GetPendingConsignmentItem(Guid jobItemId);
		IEnumerable<JobItem> GetJobItems(Guid jobId);
		IEnumerable<JobItem> SearchByKeyword(string keyword);
	}
}