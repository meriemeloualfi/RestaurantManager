namespace RestaurantManager.Models
{
    public class CommandeRequest
    {
        public required string Type { get; set; }
        public required List<ArticleRequest> Articles { get; set; }
    }

    public class ArticleRequest
    {
        public int PlatId { get; set; }
        public int Qte { get; set; }
        public required string Note { get; set; }
    }
}
