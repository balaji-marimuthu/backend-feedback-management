using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FeedbackDAL.Models
{
    public class FeedbackData
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EventID { get; set; }
        public string EventName { get; set; }
        public string EventDate { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }

        public string Location { get; set; }
        public string Project { get; set; }

        public List<string> FeedbackOptions { get; set; }
        public string SelectedFeedbackOption { get; set; }
        public List<string> FeedbackQuestions { get; set; }
        public List<string> FeedbackAnswers { get; set; }

        public string Story { get; set; }
        public bool IsFeedbackCollected
        {
            get
            {
                return Rating > 0 || !string.IsNullOrWhiteSpace(SelectedFeedbackOption);
            }
        }

        [NotMapped]
        public string EnrollmentType
        {
            get;set;
        }
    }
}