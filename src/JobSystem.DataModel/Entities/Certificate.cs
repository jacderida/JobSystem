using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JobSystem.Resources.Certificates;

namespace JobSystem.DataModel.Entities
{
    [Serializable]
    public class Certificate
    {
        public virtual Guid Id { get; set; }
        public virtual string CertificateNumber { get; set; }
        public virtual UserAccount CreatedBy { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual ListItem Type { get; set; }
        public virtual JobItem JobItem { get; set; }
        [StringLength(255, ErrorMessageResourceName = "ProcedureListTooLarge", ErrorMessageResourceType = typeof(Messages))]
        public virtual string ProcedureList { get; set; }
    }
}