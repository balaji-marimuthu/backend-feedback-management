using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;

namespace FeedbackDAL.Models
{
    [ExcludeFromCodeCoverage]
    public class MailLog
    {
        [Key]
        public int MailID { get; set; }
        public int EmployeeID { get; set; }
        public string AssociateName { get; set; }
        public bool IsMailSent { get; set; }
        public string EventID { get; set; }
        public DateTime MailSentDateTime { get; set; }
        public MailType MailType { get; set; }

        public string MailContent { get; set; }
    }

    public enum MailType
    {
        None,
        Feedback,
        Reply,
        Thank
    }
}