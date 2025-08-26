using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }
        public string SupplierName { get; set;}
        public string PhoneNumber { get; set;}
        public string Adress { get; set;}
    }
}
