using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FeedbackDAL.Models
{
    public class Feedback_Answers
    {
        [Key]
        public int FeedbackAnswerID { get; set; }
        public int EmployeeID { get; set; }
        public int FeedbackQuestionID { get; set; }
        public string FeedbackAnswers { get; set; }
        public virtual Feedback_Questions Feedback_Questions { get; set; }
    }
}