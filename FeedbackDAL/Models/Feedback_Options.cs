using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FeedbackDAL.Models
{
    public class Feedback_Options
    {
        [Key]
        public int FeedbackOptionID { get; set; }
        public string FeedbackOptions { get; set; }
        public virtual ICollection<Feedbacks> Feedbacks { get; set; }
    }
}