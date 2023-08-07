using Medical_Rgistrations.ViewModels;

namespace Medical_Rgistrations.APIModels
{
    //public class Registrations
    //{
    //    public string referenceNbr { get; set; }
    //    public int? rid { get; set; }
    //    public string contraction { get; set; }
    //    public string name { get; set; }
    //    public string fname { get; set; }
    //    public string address { get; set; }
    //    public string cor_Address { get; set; }
    //    public string email { get; set; }
    //    public string mobile { get; set; }
    //    public string qualification { get; set; }
    //}

    public class Registrations
    {
        public string RegistrationId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int GenderId { get; set; }
        public int QualificationId { get; set; }
        public string RferenceNbr { get; set; }
        public DateTime DOB { get; set; }
        public string Nationality { get; set; }
        public string InstituteName { get; set; }
        public string Year { get; set; }
        public bool ReadTermsCondition { get; set; }
        public DateTime CompletePart { get; set; }
        public string RCPCode { get; set; }
        public DateTime CreatedOn { get; set; }
        public object ModifiedOn { get; set; }
        public Genders Genders { get; set; }
        public Qualification Qualification { get; set; }
    }
}
