using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;

namespace FeedbackDAL.Models
{
    [ExcludeFromCodeCoverage]
    public class ErrorLogs
    {
        [Key]
        public int ID { get; set; }
        public string  ClassName { get; set; }
        public string MethodName { get; set; }
        public string StackTrace { get; set; }
        public DateTime LoggedOnTime { get; set; }
    }
}