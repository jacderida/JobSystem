using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace JobSystem.Admin.Mvc.Models
{
	public class TenantModel
	{
		public Guid Id { get; set; }
		[Required(ErrorMessage = "A name is required for the company.")]
		[StringLength(50, ErrorMessage = "The company name cannot exceed 255 characters")]
		public string TenantName { get; set; }

		public Guid CompanyId { get; set; }
		[Required(ErrorMessage = "A name is required for the company.")]
		[StringLength(255, ErrorMessage = "The company name cannot exceed 255 characters")]
		public string Name { get; set; }
		[StringLength(255, ErrorMessage = "The address line cannot exceed 255 characters")]
		public string Address1 { get; set; }
		[StringLength(255, ErrorMessage = "The address line cannot exceed 255 characters")]
		public string Address2 { get; set; }
		[StringLength(255, ErrorMessage = "The address line cannot exceed 255 characters")]
		public string Address3 { get; set; }
		[StringLength(255, ErrorMessage = "The address line cannot exceed 255 characters")]
		public string Address4 { get; set; }
		[StringLength(255, ErrorMessage = "The address line cannot exceed 255 characters")]
		public string Address5 { get; set; }
		[Required(ErrorMessage = "A telephone number is required.")]
		[StringLength(50, ErrorMessage = "The telephone cannot exceed 50 characters")]
		public string Telephone { get; set; }
		[Required(ErrorMessage = "A fax number is required.")]
		[StringLength(50, ErrorMessage = "The telephone cannot exceed 50 characters")]
		public string Fax { get; set; }
		[StringLength(50, ErrorMessage = "The reg no. cannot exceed 50 characters")]
		public string RegNo { get; set; }
		[StringLength(50, ErrorMessage = "The VAT reg no. cannot exceed 50 characters")]
		public string VatRegNo { get; set; }
		[Required(ErrorMessage = "An email is required.")]
		[StringLength(50, ErrorMessage = "The email address cannot exceed 50 characters")]
		public string Email { get; set; }
		[Required(ErrorMessage = "A URL is required.")]
		[StringLength(50, ErrorMessage = "The URL cannot exceed 255 characters")]
		public string Www { get; set; }
		[Required(ErrorMessage = "The terms and conditions are required for the company.")]
		[StringLength(2000, ErrorMessage = "The terms and conditions cannot exceed 2000 characters.")]
		public string TermsAndConditions { get; set; }

		public Guid BankDetailsId { get; set; }
		[Required(ErrorMessage = "A short name is required for the bank account.")]
		[StringLength(50, ErrorMessage = "The short name cannot exceed 255 characters")]
		public virtual string BankShortName { get; set; }
		[Required(ErrorMessage = "An account number is required for the bank account.")]
		[StringLength(50, ErrorMessage = "The account number cannot exceed 50 characters.")]
		public virtual string AccountNo { get; set; }
		[Required(ErrorMessage = "A sort code is required for the bank account.")]
		[StringLength(50, ErrorMessage = "The sort code cannot exceed 50 characters.")]
		public virtual string SortCode { get; set; }
		[Required(ErrorMessage = "A name is required for the bank account.")]
		[StringLength(255, ErrorMessage = "The name cannot exceed 50 characters.")]
		public virtual string BankName { get; set; }
		[StringLength(50, ErrorMessage = "The address line cannot exceed 50 characters")]
		public virtual string BankAddress1 { get; set; }
		[StringLength(50, ErrorMessage = "The address line cannot exceed 50 characters")]
		public virtual string BankAddress2 { get; set; }
		[StringLength(50, ErrorMessage = "The address line cannot exceed 50 characters")]
		public virtual string BankAddress3 { get; set; }
		[StringLength(50, ErrorMessage = "The address line cannot exceed 50 characters")]
		public virtual string BankAddress4 { get; set; }
		[StringLength(50, ErrorMessage = "The address line cannot exceed 50 characters")]
		public virtual string BankAddress5 { get; set; }
		[StringLength(50, ErrorMessage = "The IBAN cannot exceed 50 characters")]
		public virtual string Iban { get; set; }

		public int JobSeed { get; set; }
		public int ConsignmentSeed { get; set; }
		public int QuoteSeed { get; set; }
		public int OrderSeed { get; set; }
		public int CertificateSeed { get; set; }
		public int DeliverySeed { get; set; }
		public int InvoiceSeed { get; set; }
	}
}