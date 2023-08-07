namespace Medical_Rgistrations.APIModels
{
    public class Gallary
    {
        public Guid id { get; set; }
        public Guid groupId { get; set; }
        public string groupName { get; set; }
        public string fileName { get; set; }
        public int order { get; set; }
        public DateTime createdOn { get; set; }
        public bool active { get; set; }
    }
}
