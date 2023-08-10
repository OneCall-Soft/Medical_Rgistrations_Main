using System.ComponentModel.DataAnnotations;

namespace Medical_Rgistrations.ViewModels
{

    public class FacultyViewModel
    {
        public FacultyViewModel()
        {
            this.Faculties = new List<Faculty>();
        }

        public string? FacultyId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Profile is required")]
        [Display(Name ="Profile Image")]
        public IFormFile ProfileImg { get; set; }
        [Required(ErrorMessage = "Faculty Type is required")]
        [Display(Name = "Faculty Type")]
        public string FacultyType { get; set; }
        public string? ProfileName { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Institute Name")]
        public string InstituteName { get; set; }
        [Display(Name = "Active")]
        public bool Active { get; set; }
        [Required(ErrorMessage = "Facebook Link is required")]
        [Display(Name = "Facebook Link")]
        public string FacebookLink { get; set; }
        [Required(ErrorMessage = "Insta Link is required")]
        [Display(Name = "Insta Link")]
        public string InstaLink { get; set; }
        [Required(ErrorMessage = "LinkedIn Link is required")]
        [Display(Name = "LinkedIn Link")]
        public string LinkedInLink { get; set; }
        [Required(ErrorMessage = "Twitter Link is required")]
        [Display(Name = "Twitter Link")]
        public string TwitterLink { get; set; }

        public List<Faculty>? Faculties { get; set; }

        public string? msg { get; set; }

    }
}
