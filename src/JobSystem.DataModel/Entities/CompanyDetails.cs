using System;

namespace JobSystem.DataModel.Entities
{
	[Serializable]
	public class CompanyDetails
	{
		public virtual Guid Id { get; set; }
		public virtual string Name { get; set; }
		public virtual string Address1 { get; set; }
		public virtual string Address2 { get; set; }
		public virtual string Address3 { get; set; }
		public virtual string Address4 { get; set; }
		public virtual string Address5 { get; set; }
		public virtual string Telephone { get; set; }
		public virtual string Fax { get; set; }
		public virtual string RegNo { get; set; }
		public virtual string VatRegNo { get; set; }
		public virtual string Email { get; set; }
		public virtual string Www{ get; set; }
		public virtual string TermsAndConditions { get; set; }
		public virtual Guid DefaultCurrencyId { get; set; }
		public virtual Guid DefaultTaxCodeId { get; set; }
		public virtual Guid DefaultPaymentTermsId { get; set; }
		public virtual Guid DefaultBankDetailsId { get; set; }
	}
}