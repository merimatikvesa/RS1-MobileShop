using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Specs
    {
        [Key]
        public int PhoneId { get; set; }
        public string RAM { get; set; }
        public string Storage { get; set; }
        public string MainCamera { get; set; }
        public string FrontCamera { get; set; }
        public string BatteryCapacity { get; set; }
        public string Color {  get; set; }
    }
}
