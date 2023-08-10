namespace Medical_Rgistrations.ViewModels
{
    

    public class GallaryViewModel
    {
        public GallaryViewModel()
        {
            this.ImageGroups = new List<string>();
        }

        public string? groupName { get; set; }
        public List<IFormFile> imageFiles { get; set; }        
        public bool active { get; set; }
        public List<string> ImageGroups { get; set; }

    }

}
