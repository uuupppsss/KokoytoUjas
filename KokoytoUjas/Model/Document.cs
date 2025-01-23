namespace KokoytoUjas.Model
{
    public class Document
    {
        public int id { get; set; }
        public string title { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_updated { get; set; }
        public string category { get; set; }
        public bool has_comments { get; set; }
    }
}
