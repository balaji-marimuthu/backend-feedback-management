using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeedbackDAL.Models
{
    public class OutReachData
    {
        public List<POCDetails> POCDetails { get;set;}
        public List<ParticipatedDetails> ParticipatedDetails { get; set; }
        public List<EnrollmentInformation> UnRegisteredVolunteers { get; set; }
        public List<EnrollmentInformation> NotAttendedVolunteers { get; set; }
    }
}