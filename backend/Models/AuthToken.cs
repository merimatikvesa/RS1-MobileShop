using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class AuthToken
    {
        [Key]
        public int AuthTokenId { get; set; }
        [Required]
        [MaxLength(512)]
        public string Value { get; set; }=string.Empty;
        [Required]
        public int AccountID { get; set; }
        public DateTime TimeGenerated { get; set; }

    }
}
