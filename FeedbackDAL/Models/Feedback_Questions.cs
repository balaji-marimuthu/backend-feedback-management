using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FeedbackDAL.Models
{
    public class Feedback_Questions
    {
        [Key]
        public int ID { get; set; }
        public string FeedbackQuestions { get; set; }
        //public string MailContent { get; set; }

        public virtual ICollection<Feedback_Answers> Feedback_Answers { get; set; }
    }
}