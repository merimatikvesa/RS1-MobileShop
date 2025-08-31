using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace backend.Models
{
    public class Specs
    {
        [Key, ForeignKey("Phone")]
        public int PhoneId { get; set; }
        public string RAM { get; set; }
        public string Storage { get; set; }
        public string MainCamera { get; set; }
        public string FrontCamera { get; set; }
        public string BatteryCapacity { get; set; }
        public string Color {  get; set; }

        public Phone Phone { get; set; }
    }
}
