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

        // GET: Events/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Organizator,CurrentlyReserved,MaxCapacity,Image,Price,Description,EventDate")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@event);
        }

        // GET: Events/Edit/5
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
        public ActionResult Edit([Bind(Include = "Id,Name,Organizator,CurrentlyReserved,MaxCapacity")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@event);
        }

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

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
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
            return RedirectToAction("Index");
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
            Reservation newReservation = new Reservation(selectedEvent, user, viewModel.NoOfTickets);
            selectedEvent.Reservations.Add(newReservation);
            user.Reservations.Add(newReservation);
            selectedEvent.CurrentlyReserved += viewModel.NoOfTickets; 
            db.Reservations.Add(newReservation);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = viewModel.SelectedEvent.Id }); 
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
            user.Reservations.Remove(reservation);
            selectedEvent.Reservations.Remove(reservation);
            selectedEvent.CurrentlyReserved -= reservation.NoOfTickets;
            db.Reservations.Remove(reservation);
            db.SaveChanges();
            return RedirectToAction("MyReservations");
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
