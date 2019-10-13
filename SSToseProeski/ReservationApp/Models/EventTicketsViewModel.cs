using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReservationApp.Models
{
    public class EventTicketsViewModel
    {
        public Event SelectedEvent { get; set; }
        [Required]
        [Range(1, 6)]
        public int NoOfTickets { get; set; }
        public EventTicketsViewModel() { }
        public EventTicketsViewModel(Event selectedEvent)
        {
            SelectedEvent = selectedEvent;
            NoOfTickets = 0;
        }
    }
}