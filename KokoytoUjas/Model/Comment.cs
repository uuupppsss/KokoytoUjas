namespace KokoytoUjas.Model
{
    public class Comment
    {
        public int id { get; set; } 
        public int document_id { get; set; }
        public string text { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_updated { get; set; }
        public Author author { get; set; }

    }

    public class Author
    {
        public string name { get; set; }
        public string position { get; set; }
    }
}
