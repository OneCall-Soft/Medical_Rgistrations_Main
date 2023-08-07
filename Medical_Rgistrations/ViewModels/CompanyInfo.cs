using System.ComponentModel.DataAnnotations;

namespace Medical_Rgistrations.ViewModels
{
    public class CompanyInfo
    {
        public CompanyInfo()
        {
            
        }
        public string? Id { get; set; }
        [Display(Name = "Company")]
        public string? Name { get; set; }
        public string? Address { get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name= "Email")]
        public string? Email { get; set; }
    }
}
