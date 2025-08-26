using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Favorites
    {
        [Key]
        public int FavoriteId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
}
