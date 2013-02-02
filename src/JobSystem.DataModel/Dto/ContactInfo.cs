using System;

namespace JobSystem.DataModel.Dto
{
    public class ContactInfo
    {
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Contact1 { get; set; }
        public string Contact2 { get; set; }

        public static ContactInfo GetContactInfoFromDetails(
            string telephone, string fax, string email, string contact1, string contact2)
        {
            return new ContactInfo
            {
                Telephone = !String.IsNullOrEmpty(telephone) ? telephone : String.Empty,
                Fax = !String.IsNullOrEmpty(fax) ? fax : String.Empty,
                Email = !String.IsNullOrEmpty(email) ? email : String.Empty,
                Contact1 = !String.IsNullOrEmpty(contact1) ? contact1 : String.Empty,
                Contact2 = !String.IsNullOrEmpty(contact2) ? contact2 : String.Empty
            };
        }
    }
}