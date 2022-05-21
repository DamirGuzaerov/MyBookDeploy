namespace MyBook.Entities
{
    public partial class DownloadLink
    {
        public int DownloadLinkId { get; set; }
        public string Format { get; set; } = null!;
        public string Url { get; set; } = null!;
        public int BookDescId { get; set; }
        public virtual BookDesc BookDesc { get; set; } = null!;
    }
}
