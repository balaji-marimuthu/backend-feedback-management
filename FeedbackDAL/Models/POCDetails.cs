using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeedbackDAL.Models
{
    public class POCDetails
    {
        public string EventID { get; set; }
        public string Month { get; set; }
        public string BaseLocation { get; set; }
        public string BeneficiaryName { get; set; }
        public string VenueAddress { get; set; }
        public string CouncilName { get; set; }
        public string Project { get; set; }
        public string Category { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public string EventDate { get; set; }
        public int TotalNumberOfVolunteers { get; set; }
        public double TotalVolunteerHours { get; set; }
        public double TotalTravelHours { get; set; }
        public double OverallVolunteeringHours { get; set; }
        public int LivesImpacted { get; set; }
        public int ActivityType { get; set; }
        public string Status { get; set; }
        public string POCID { get; set; }
        public string POCName { get; set; }
        public string POCContactNumber { get; set; }
    }
}
