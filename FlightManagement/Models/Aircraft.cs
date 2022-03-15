using System.ComponentModel.DataAnnotations.Schema;

namespace FlightManagement.Models
{
    public class Aircraft
    {
        public int ID { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal ConsumptionPerKm { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal TakeOffDistance { get; set; }
        public int TakeOffEffort { get; set; }
    }
}
