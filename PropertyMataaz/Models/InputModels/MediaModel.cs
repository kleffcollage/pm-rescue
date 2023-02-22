namespace PropertyMataaz.Models.InputModels
{
    public class MediaModel
    {
        public string Name { get; set; }
        public string Extention { get; set;}
        public string Base64String {get;set;}
        public int PropertyId {get;set;}
        public bool IsImage { get; set; }
        public bool IsVideo { get; set; }
        public bool IsDocument {get;set;}
        public string Url { get; set;}
    }
}