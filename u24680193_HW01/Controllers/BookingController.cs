using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using u24680193_HW01.Models;

namespace u24680193_HW01.Controllers
{
    public class BookingController : Controller
    {
        // GET: Booking
        public ActionResult SelectService()
        {
            return View();
        }

        public ActionResult BookingForm(string serviceType)
        {
            ViewBag.ServiceType = serviceType;

            // Get and filter drivers
            var driversJson = Session["Drivers"] as string ?? "[]";
            var allDrivers = JsonConvert.DeserializeObject<List<Driver>>(driversJson);
            var drivers = allDrivers.Where(d => d.ServiceType == serviceType).ToList();
            ViewBag.Drivers = new SelectList(drivers, "Id", "FirstName");

            // Get and filter vehicles
            var vehiclesJson = Session["Vehicles"] as string ?? "[]";
            var allVehicles = JsonConvert.DeserializeObject<List<Vehicle>>(vehiclesJson);
            ViewBag.Vehicles = new SelectList(allVehicles, "Id", "VehicleName");

            return View();
        }


        [HttpPost]
        public ActionResult BookingForm(string serviceType, string fullName, string phoneNumber, string pickupAddress, string dropoffAddress, string pickupTime, string reason, string driverId, string vehicleId)
        {
            var driversJson = Session["Drivers"] as string ?? "[]";
            var allDrivers = JsonConvert.DeserializeObject<List<Driver>>(driversJson);
            var driver = allDrivers.FirstOrDefault(d => d.Id == driverId);

            var vehiclesJson = Session["Vehicles"] as string ?? "[]";
            var allVehicles = JsonConvert.DeserializeObject<List<Vehicle>>(vehiclesJson);
            var vehicle = allVehicles.FirstOrDefault(v => v.Id == vehicleId);

            var booking = new Booking
            {
                BookingID = Guid.NewGuid().ToString(),
                Date = DateTime.Now,
                FullName = fullName,
                PhoneNumber = phoneNumber,
                PickupTime = pickupTime,
                PickupAddress = pickupAddress,
                DropoffAddress = dropoffAddress,
                ServiceType = serviceType,
                Driver = driver,
                Vehicle = vehicle,
                IsSOS = false
            };

            var bookingsJson = Session["Bookings"] as string ?? "[]";
            var bookings = JsonConvert.DeserializeObject<List<Booking>>(bookingsJson);
            bookings.Add(booking);
            Session["Bookings"] = JsonConvert.SerializeObject(bookings);

            return RedirectToAction("Confirmed", "Booking", new { id = booking.BookingID });
        }


        public ActionResult Confirmed(string id)
        {
            var bookingsJson = Session["Bookings"] as string ?? "[]";
            var bookings = JsonConvert.DeserializeObject<List<Booking>>(bookingsJson);
            var booking = bookings.FirstOrDefault(b => b.BookingID == id);

            if (booking == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(booking);
        }

        public ActionResult RideHistory()
        {
            var bookingsJson = Session["Bookings"] as string ?? "[]";
            var bookings = JsonConvert.DeserializeObject<List<Booking>>(bookingsJson);
            return View(bookings);
        }


    }


}