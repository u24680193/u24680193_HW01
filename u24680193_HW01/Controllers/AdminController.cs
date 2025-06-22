using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using u24680193_HW01.Models;

namespace u24680193_HW01.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Manage()
        {
            var driversJson = Session["Drivers"] as string ?? "[]";
            var vehiclesJson = Session["Vehicles"] as string ?? "[]";

            var drivers = JsonConvert.DeserializeObject<List<Driver>>(driversJson);
            var vehicles = JsonConvert.DeserializeObject<List<Vehicle>>(vehiclesJson);

            ViewBag.Drivers = drivers;
            ViewBag.Vehicles = vehicles;

            return View();
        }




        public ActionResult ExportVehicles()
        {
            var vehicles = JsonConvert.DeserializeObject<List<Vehicle>>(Session["Vehicles"] as string ?? "[]");

            var lines = vehicles.Select(v =>
                $"{v.VehicleName}, {v.RegistrationNumber}, {v.ServiceType}");

            var content = string.Join(Environment.NewLine, lines);
            var bytes = Encoding.UTF8.GetBytes(content);

            return File(bytes, "text/plain", "vehicles.txt");
        }


        [HttpGet]
        public ActionResult CreateDriver()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateDriver(Driver driver)
        {
            driver.Id = Guid.NewGuid().ToString();
            var drivers = JsonConvert.DeserializeObject<List<Driver>>(Session["Drivers"] as string ?? "[]");
            drivers.Add(driver);
            Session["Drivers"] = JsonConvert.SerializeObject(drivers);
            return RedirectToAction("Manage");
        }

        [HttpGet]
        public ActionResult EditDriver(string id)
        {
            var drivers = JsonConvert.DeserializeObject<List<Driver>>(Session["Drivers"] as string ?? "[]");
            var driver = drivers.FirstOrDefault(d => d.Id == id);
            return View(driver);
        }

        [HttpPost]
        public ActionResult EditDriver(Driver updatedDriver)
        {
            var drivers = JsonConvert.DeserializeObject<List<Driver>>(Session["Drivers"] as string ?? "[]");
            var index = drivers.FindIndex(d => d.Id == updatedDriver.Id);
            if (index >= 0)
            {
                drivers[index] = updatedDriver;
                Session["Drivers"] = JsonConvert.SerializeObject(drivers);
            }
            return RedirectToAction("Manage");
        }

        public ActionResult DeleteDriver(string id)
        {
            var drivers = JsonConvert.DeserializeObject<List<Driver>>(Session["Drivers"] as string ?? "[]");
            drivers = drivers.Where(d => d.Id != id).ToList();
            Session["Drivers"] = JsonConvert.SerializeObject(drivers);
            return RedirectToAction("Manage");
        }

        [HttpGet]
        public ActionResult CreateVehicle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateVehicle(Vehicle vehicle)
        {
            vehicle.Id = Guid.NewGuid().ToString();
            var vehicles = JsonConvert.DeserializeObject<List<Vehicle>>(Session["Vehicles"] as string ?? "[]");
            vehicles.Add(vehicle);
            Session["Vehicles"] = JsonConvert.SerializeObject(vehicle);
            return RedirectToAction("Manage");
        }

        [HttpGet]
        public ActionResult EditVehicle(string id)
        {
            var vehicles = JsonConvert.DeserializeObject<List<Vehicle>>(Session["Vehicles"] as string ?? "[]");
            var vehicle = vehicles.FirstOrDefault(d => d.Id == id);
            return View(vehicle);
        }

        [HttpPost]
        public ActionResult EditVehicle(Vehicle updatedVehicle)
        {
            var vehicles = JsonConvert.DeserializeObject<List<Vehicle>>(Session["Vehicles"] as string ?? "[]");
            var index = vehicles.FindIndex(d => d.Id == updatedVehicle.Id);
            if (index >= 0)
            {
                vehicles[index] = updatedVehicle;
                Session["Vehicles"] = JsonConvert.SerializeObject(vehicles);
            }
            return RedirectToAction("Manage");
        }

        public ActionResult DeleteVehicle(string id)
        {
            var vehicles = JsonConvert.DeserializeObject<List<Vehicle>>(Session["Vehicles"] as string ?? "[]");
            vehicles = vehicles.Where(d => d.Id != id).ToList();
            Session["Vehicles"] = JsonConvert.SerializeObject(vehicles);
            return RedirectToAction("Manage");
        }

        [HttpPost]
        public ActionResult SearchDrivers(string firstName, string serviceType)
        {
            var drivers = JsonConvert.DeserializeObject<List<Driver>>(Session["Drivers"] as string ?? "[]");

            if (!string.IsNullOrWhiteSpace(firstName))
                drivers = drivers.Where(d => d.FirstName.ToLower().Contains(firstName.ToLower())).ToList();

            if (!string.IsNullOrWhiteSpace(serviceType))
                drivers = drivers.Where(d => d.ServiceType == serviceType).ToList();

            var vehicles = JsonConvert.DeserializeObject<List<Vehicle>>(Session["Vehicles"] as string ?? "[]");

            ViewBag.Drivers = drivers;
            ViewBag.Vehicles = vehicles;

            return View("Manage");
        }


    }
}