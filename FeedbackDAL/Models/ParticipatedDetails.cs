using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FeedbackDAL.Models
{
    public class ParticipatedDetails
    {
        public string EventID { get; set; }
        public string BaseLocation { get; set; }
        public string BeneficiaryName { get; set; }
        public string CouncilName { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public string EventDate { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public double VolunteerHours { get; set; }
        public double TravelHours { get; set; }
        public int LivesImpacted { get; set; }
        public string BusinessUnit { get; set; }
        public string Status { get; set; }
        public string IiepCategory { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
    }
}