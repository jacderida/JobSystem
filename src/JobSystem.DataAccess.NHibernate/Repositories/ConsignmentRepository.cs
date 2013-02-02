using System.Collections.Generic;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Linq;
using System;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
    public class ConsignmentRepository : RepositoryBase<Consignment>, IConsignmentRepository
    {
        public int GetConsignmentItemCount(Guid consignmentId)
        {
            return CurrentSession.Query<ConsignmentItem>().Where(ci => ci.Consignment.Id == consignmentId).Count();
        }

        public int GetConsigmentsCount()
        {
            return CurrentSession.Query<Consignment>().Count();
        }

        public IEnumerable<Consignment> GetConsignments()
        {
            return CurrentSession.Query<Consignment>();
        }

        public IEnumerable<ConsignmentItem> GetConsignmentItems(Guid consignmentId)
        {
            return CurrentSession.Query<ConsignmentItem>().Where(ci => ci.Consignment.Id == consignmentId);
        }
    }
}