using System;

namespace FlightManagement.Models
{
    public class Flight
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime FlightTime { get; set; }
        public DateTime ModificationTime { get; set; }
        public Airport Destination { get; set; }
        public Airport Departure { get; set; }
        public Aircraft Aircraft { get; set; }
        public string UserId { get; set; }
    }
}
