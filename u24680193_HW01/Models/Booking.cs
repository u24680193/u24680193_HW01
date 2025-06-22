using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u24680193_HW01.Models
{
    public class Booking
    {
        public string BookingID { get; set; }
        public DateTime Date { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string PickupAddress { get; set; }
        public string PickupTime { get; set; }
        public string DropoffAddress { get; set; }
        public string ServiceType { get; set; }
        public Driver Driver { get; set; }
        public Vehicle Vehicle { get; set; }
        public bool IsSOS { get; set; }
    }
}
