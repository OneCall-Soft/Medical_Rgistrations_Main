using Medical_Rgistrations.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Medical_Rgistrations.APIModels
{
    public class APIRegistration
    {
        public string? RegistrationId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string? RferenceNbr { get; set; }
        public DateTime DOB{get; set;}
        public string Nationality { get; set; }
        public string InstituteName { get; set; }
        public string Year { get; set; }
        public bool ReadTermsCondition { get; set; }
        public DateTime CompletePart{get; set;}
        public string RCPCode { get; set; }
        public int GenderId { get; set; }
        public int QualificationId { get; set; }
        public Genders? Genders { get; set; }
        public Qualification? Qualification { get; set; }

    }
}
