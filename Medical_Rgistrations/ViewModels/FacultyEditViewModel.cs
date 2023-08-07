using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Medical_Rgistrations.ViewModels
{
    public class FacultyEditViewModel
    {
        public FacultyEditViewModel()
        {
        }

        public string? FacultyId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Display(Name = "Profile Image")]
        public IFormFile? ProfileImg { get; set; }
        public string? ProfileName { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Institute Name")]
        public string InstituteName { get; set; }
        [Display(Name = "Active")]
        public bool Active { get; set; }
    }
}
