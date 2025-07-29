using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Phone
    {

            [Key]
            public int PhoneId { get; set; }
            public string Model { get; set; }
            public int BrandId { get; set; }
            public decimal Price { get; set; }
            public bool StockAvailability { get; set; }
            public int WarrantyId { get; set; }
            public int OSId { get; set; }
            public int SupplierId { get; set; }
        

    }
}
