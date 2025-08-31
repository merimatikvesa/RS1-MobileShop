using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Phone
    {
        [Key]
        public int PhoneId { get; set; }
        public int OSId { get; set; }
        public OS OS { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int WarrantyId { get; set; }
        public Warranty Warranty { get; set; }

        public Specs Specs { get; set; }
    }
}
