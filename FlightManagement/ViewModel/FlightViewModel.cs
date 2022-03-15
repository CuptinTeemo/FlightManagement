using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlightManagement.Models
{
    public class FlightViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Flight Title is Required")]
        [Display(Name = "Flight Title")]
        public string Title { get; set; }
        [Display(Name = "Flight Time")]
        public DateTime FlightTime { get; set; }
        [Display(Name = "Destination")]
        public int DestinationId { get; set; }
        [Display(Name = "Departure")]
        public int DepartureId { get; set; }
        public List<SelectListItem> Airports { get; set; }
        public List<SelectListItem> Aircrafts { get; set; }
        public Airport Destination { get; set; }
        public Airport Departure { get; set; }
        [Display(Name = "Aircraft")]
        public int AircraftId { get; set; }
        public Aircraft Aircraft { get; set; }
        [Display(Name = "Distance")]
        public decimal Distance { get { if(Destination != null && Departure != null) return Math.Abs(Destination.GPS - Departure.GPS); return 0; } }
        public decimal Consumption { get; set; }
    }
}
