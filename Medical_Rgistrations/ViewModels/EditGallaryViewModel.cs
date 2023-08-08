using System.ComponentModel.DataAnnotations;

namespace Medical_Rgistrations.ViewModels
{
    

    public class EditGallaryViewModel:GallaryViewModel
    {
        public EditGallaryViewModel()
        {
            this.ImageGroups = new List<string>();
        }

    }

}
