using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ReservationApp.Models;

namespace ReservationApp.Controllers
{
    public class EventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Events
        public ActionResult Index()
        {
            return View(db.Events.ToList());
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            EventTicketsViewModel viewModel = new EventTicketsViewModel(@event);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        // GET: Events/Create
        public ActionResult Create()
        {
            Event @event = new Event();
            return View(@event);
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Name,Organizator,CurrentlyReserved,MaxCapacity,Image,Price,Description,EventDate,EventTime")] Event @event)
        {
            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();
            if (ModelState.IsValid)
            {
                @event.CurrentlyReserved = 0;
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@event);
        }

        // GET: Events/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Name,Organizator,CurrentlyReserved,MaxCapacity,Image,Price,Description,EventDate,EventTime")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = @event.Id });
            }
            return View(@event);
        }

        /*
        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }
        */

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public void DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            for(int i = @event.Reservations.Count - 1; i >= 0; i--)
            {
                Reservation temp = @event.Reservations[i];
                db.Users.Find(temp.User.Id).Reservations.Remove(temp);
                db.Reservations.Remove(temp);
            }
            db.Events.Remove(@event);
            db.SaveChanges();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Reserve(EventTicketsViewModel viewModel)
        {
            if (viewModel == null || viewModel.NoOfTickets < 1 || viewModel.NoOfTickets > 6)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event selectedEvent = db.Events.Find(viewModel.SelectedEvent.Id);
            if (selectedEvent == null)
            {
                return HttpNotFound();
            }
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            foreach(Reservation res in user.Reservations)
            {
                if(res.Event.Id == selectedEvent.Id)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "You've already made a reservation pal.");
                }
            }
            if(selectedEvent.EventDate > DateTime.Now)
            {
                Reservation newReservation = new Reservation(selectedEvent, user, viewModel.NoOfTickets);
                selectedEvent.Reservations.Add(newReservation);
                user.Reservations.Add(newReservation);
                selectedEvent.CurrentlyReserved += viewModel.NoOfTickets;
                db.Reservations.Add(newReservation);
                db.SaveChanges();
            }
            return RedirectToAction("Index"); 
        }

        [Authorize]
        public ActionResult MyReservations()
        {
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            IEnumerable<Reservation> model = user.Reservations;
            return View(model);
        }

        [Authorize]
        public ActionResult DeleteReservation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ApplicationUser user = db.Users.Find(reservation.User.Id);
            if(!User.Identity.GetUserId().Equals(user.Id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event selectedEvent = db.Events.Find(reservation.Event.Id);
            if(reservation.Event.EventDate > DateTime.Now)
            {
                user.Reservations.Remove(reservation);
                selectedEvent.Reservations.Remove(reservation);
                selectedEvent.CurrentlyReserved -= reservation.NoOfTickets;
                db.Reservations.Remove(reservation);
                db.SaveChanges();
            }
            return RedirectToAction("MyReservations");
        }

        [Authorize (Roles = "Admin,Employee")]
        public ActionResult Plakjanje(int? id)
        {
            Reservation res = null;
            if (id != null)
            {
                res = db.Reservations.Find(id);
                if(res == null)
                {
                    ViewBag.Message = "Не е пронајден запис";
                }
            }
            return View(res);
        }

        [Authorize(Roles = "Admin,Employee")]
        public ActionResult Plati(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation res = db.Reservations.Find(id);
            if(res == null)
            {
                return HttpNotFound();
            }
            if(res.Event.EventDate > DateTime.Now)
            {
                res.Paid = true;
                db.SaveChanges();
            }
            int? temp = id;
            return RedirectToAction("Plakjanje", new { id = temp });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
