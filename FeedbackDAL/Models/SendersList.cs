using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;

namespace FeedbackDAL.Models
{
    [ExcludeFromCodeCoverage]
    public class SendersList
    {
        public string EventID { get; set; }
        public int EmployeeID { get; set; }
        public string  EventName { get; set; }

        public string EventDate { get; set; }

        public MailType MailType { get; set; }

        public string MailContent { get; set; }
    }
}