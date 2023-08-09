using Medical_Rgistrations.APIModels;

namespace Medical_Rgistrations.ViewModels
{
    public class MyHtmlContent
    {

        public MyHtmlContent()
        {
            this.PagesGroup = new List<string>();
            this.DownloadLinks = new List<AncorLink>();
            this.ImportentNotification = new List<AncorLink>();
        }
        public Guid Id { get; set; }
        public string HtmlData { get; set; }
        public string Page { get; set; }
        public bool Active { get; set; }
        public string TemplateName { get; set; }
        public List<string> PagesGroup { get; set; }
        public string? GallaryGroup { get; set; }
        public Gallary? imgages { get; set; }
        public List<AncorLink>? DownloadLinks { get; set; }
        public List<AncorLink>? ImportentNotification { get; set; }
        public List<AncorLink>? Footer1 { get; set; }
        public List<AncorLink>? Footer2 { get; set; }

    }


    public class AncorLink
    {
        public string Name { get; set; }
        public string link { get; set; }
    }

}
