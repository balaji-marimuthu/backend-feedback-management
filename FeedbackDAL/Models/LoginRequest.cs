using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;

namespace FeedbackDAL.Models
{
    [ExcludeFromCodeCoverage]
    public class LoginRequest
    {
        public int UserName { get; set; }
        public string Password { get; set; }
    }
}