using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using u24680193_HW01.Models;

namespace u24680193_HW01.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var drivers = new List<Driver>
{
            new Driver { Id = Guid.NewGuid().ToString(), FirstName = "Brad", LastName = "Simmons", PhoneNumber = "+27 76 567 7864", ServiceType = "ALS", ImagePath = "~/Content/images/dude.png" },
            new Driver { Id = Guid.NewGuid().ToString(), FirstName = "Jessica", LastName = "Clarkson", PhoneNumber = "+27 76 567 7864", ServiceType = "ALS", ImagePath = "~/Content/images/girl.png" },
            new Driver { Id = Guid.NewGuid().ToString(), FirstName = "Wayne", LastName = "Brown", PhoneNumber = "+27 76 111 2222", ServiceType = "BLS", ImagePath = "~/Content/images/dude.png" },
            new Driver { Id = Guid.NewGuid().ToString(), FirstName = "Lisa", LastName = "Green", PhoneNumber = "+27 38 999 3484", ServiceType = "Event", ImagePath = "~/Content/images/girl.png" },
            new Driver { Id = Guid.NewGuid().ToString(), FirstName = "Zanele", LastName = "Nkosi", PhoneNumber = "+27 72 123 4567", ServiceType = "Air Ambulance", ImagePath = "~/Content/images/girl.png" },
            new Driver { Id = Guid.NewGuid().ToString(), FirstName = "Michael", LastName = "Van der Merwe", PhoneNumber = "+27 78 654 3210", ServiceType = "Basic Life Support", ImagePath = "~/Content/images/dude.png" },
            new Driver { Id = Guid.NewGuid().ToString(), FirstName = "Fatima", LastName = "Desai", PhoneNumber = "+27 74 456 7890", ServiceType = "Patient Support", ImagePath = "~/Content/images/girl.png" },
            new Driver { Id = Guid.NewGuid().ToString(), FirstName = "David", LastName = "Mokoena", PhoneNumber = "+27 73 222 3333", ServiceType = "Event Medical Ambulance", ImagePath = "~/Content/images/dude.png" },
            new Driver { Id = Guid.NewGuid().ToString(), FirstName = "Thandi", LastName = "Ngubane", PhoneNumber = "+27 76 789 1234", ServiceType = "Medical Utility Vehicle", ImagePath = "~/Content/images/girl.png" },
            new Driver { Id = Guid.NewGuid().ToString(), FirstName = "Shaun", LastName = "Moodley", PhoneNumber = "+27 79 987 6543", ServiceType = "Advanced Life Support", ImagePath = "~/Content/images/dude.png" } };

            var vehicles = new List<Vehicle>
            {
             new Vehicle { Id = Guid.NewGuid().ToString(), VehicleName = "Helicopter", RegistrationNumber = "ALS1234", ServiceType = "ALS", ImagePath = "~/Content/images/heli.png" },
             new Vehicle { Id = Guid.NewGuid().ToString(), VehicleName = "Ambulance", RegistrationNumber = "ALS5678", ServiceType = "ALS", ImagePath = "~/Content/images/ambulance.png" }
            };

            Session["Drivers"] = JsonConvert.SerializeObject(drivers);
            Session["Vehicles"] = JsonConvert.SerializeObject(vehicles);

            return View();
        }

        [HttpPost]
        public ActionResult SOS()
        {
            var driversJson = Session["Drivers"] as string ?? "[]";
            var vehiclesJson = Session["Vehicles"] as string ?? "[]";

            var drivers = JsonConvert.DeserializeObject<List<Driver>>(driversJson);
            var vehicles = JsonConvert.DeserializeObject<List<Vehicle>>(vehiclesJson);

            if (drivers.Count == 0 || vehicles.Count == 0)
            {
                TempData["Error"] = "No drivers or vehicles available.";
                return RedirectToAction("Index");
            }

            var rand = new Random();
            var driver = drivers[rand.Next(drivers.Count)];
            var vehicle = vehicles[rand.Next(vehicles.Count)];

            var booking = new Booking
            {
                BookingID = Guid.NewGuid().ToString(),
                Date = DateTime.Now,
                FullName = "Emergency",
                PhoneNumber = "N/A",
                PickupAddress = "Accident scene",
                PickupTime = "ASAP",
                ServiceType = "Advanced Life Support",
                Driver = driver,
                Vehicle = vehicle,
                IsSOS = true
            };

            var bookingsJson = Session["Bookings"] as string ?? "[]";
            var bookings = JsonConvert.DeserializeObject<List<Booking>>(bookingsJson);
            bookings.Add(booking);
            Session["Bookings"] = JsonConvert.SerializeObject(bookings);

            return RedirectToAction("Confirmed", "Booking", new { id = booking.BookingID });
        }


    }
}