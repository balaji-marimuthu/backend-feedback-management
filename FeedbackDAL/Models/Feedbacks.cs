using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FeedbackDAL.Models
{
    public class Feedbacks
    {
        private DateTime?  dateCreated = null;

        [Key]
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string EventID { get; set; }
        public string EventName { get; set; }
        public string EmployeeName { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
        public int FeedbackOptionID { get; set; }
        public string Story { get; set; }
        public string Location { get; set; }
        public string Project { get; set; }

        public string EventDate { get; set; }
        public DateTime CreatedDateTime
        {
            get
            {
                return this.dateCreated.HasValue
                        ? this.dateCreated.Value
                        : DateTime.Now;
            }
            set
            {
                dateCreated = value;
            }
        }
        public virtual Feedback_Options Feedback_Options { get; set; }
    }
}