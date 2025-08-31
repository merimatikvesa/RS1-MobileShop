using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [ForeignKey("AccountID")]
        public Account Account { get; set; }
        public DateTime TimeGenerated { get; set; }

    }
}
