using System.ComponentModel.DataAnnotations;

namespace Medical_Rgistrations.ViewModels
{
    

    public class GallaryViewModel
    {
        public GallaryViewModel()
        {
            this.ImageGroups = new List<string>();
        }

        
        [Display(Name ="Group Name")]
        public string groupName { get; set; }
        [Display(Name = "Gallery Template Name")]
        public string GalleryName { get; set; }

        [Display(Name = "Images")]
        public List<IFormFile> imageFiles { get; set; }
        [Display(Name = "Active")]
        public bool active { get; set; }

        public List<string> ImageGroups { get; set; }

    }

}
