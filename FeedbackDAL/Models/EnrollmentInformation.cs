using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FeedbackDAL.Models
{
    public class EnrollmentInformation
    {
        public string EventID { get; set; }
        public string EventName { get; set; }
        public string BeneficiaryName { get; set; }
        public string BaseLocation { get; set; }
        public string EventDate { get; set; }
        public int EmployeeID { get; set; }

        public bool IsParticipated { get; set; }
        public bool IsRegistered { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

    }
}