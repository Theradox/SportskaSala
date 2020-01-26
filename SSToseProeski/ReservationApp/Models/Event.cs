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
        [Display(Name = "Име на настанот")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Линк до слика")]
        public String Image { get; set; }
        [Required]
        [StringLength(80)]
        [Display(Name = "Организатор")]
        public string Organizator { get; set; }
        [Required]
        [Range(0, 12000)]
        [Display(Name = "Цена на билет")]
        public int Price { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Датум")]
        public DateTime? EventDate { get; set; }
        [Required]
        [Display(Name = "Почеток")]
        public String EventTime { get; set; }
        [Required]
        [StringLength(500)]
        [Display(Name = "Опис")]
        public String Description { get; set; }
        public int CurrentlyReserved { get; set; }
        [Required]
        [Range(0,30000)]
        public int MaxCapacity { get; set; }
        public virtual List<Reservation> Reservations { get; set; }
        public Event()
        {
            CurrentlyReserved = 0;
            Reservations = new List<Reservation>();
        }
    }
}