namespace PropertyMataaz.Models.ViewModels
{
    public class MediaView
    {
        public int Id { get; set;}
        public string Url { get; set; }
        public bool IsImage { get; set; }
        public bool IsVideo { get; set; }
        public bool IsDocument { get; set; }
    }
}