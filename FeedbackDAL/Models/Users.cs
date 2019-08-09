using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;

namespace FeedbackDAL.Models
{
    [ExcludeFromCodeCoverage]
    public class Users
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}