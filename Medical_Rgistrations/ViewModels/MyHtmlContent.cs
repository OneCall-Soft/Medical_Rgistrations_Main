using Medical_Rgistrations.APIModels;

namespace Medical_Rgistrations.ViewModels
{
    public class MyHtmlContent
    {

        public MyHtmlContent()
        {
            this.PagesGroup = new List<string>();
        }
        public Guid Id { get; set; }
        public string HtmlData { get; set; }
        public string Page { get; set; }
        public bool Active { get; set; }
        public string TemplateName { get; set; }
        public List<string> PagesGroup { get; set; }
        public string? GallaryGroup { get; set; }
        public Gallary? imgages { get; set; }

    }


}
