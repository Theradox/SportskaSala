using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReservationApp.Models
{
    public class Event
    {
        public int Id { get; set; }
        [Required]
        [StringLength(80)]
        //IMAGE
        public string Name { get; set; }
        [Required]
        [StringLength(80)]
        public string Organizator { get; set; }
        //PRICE
        public int CurrentlyReserved { get; set; }
        [Required]
        [Range(0,30000)]
        [Display(Name = "Maximum capacity")]
        public int MaxCapacity { get; set; }
        public virtual List<Reservation> Reservations { get; set; }
        public Event()
        {
            CurrentlyReserved = 0;
            Reservations = new List<Reservation>();
        }
    }
}