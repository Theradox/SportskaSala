using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReservationApp.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public virtual Event Event { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int NoOfTickets { get; set; }
        public bool Paid { get; set; }
        public Reservation() { }
        public Reservation(Event selectedEvent, ApplicationUser user, int noOfTickets)
        {
            Event = selectedEvent;
            User = user;
            NoOfTickets = noOfTickets;
            Paid = false;
        }
    }
}