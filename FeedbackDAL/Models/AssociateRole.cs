using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FeedbackDAL.Models
{
    public class AssociateRole
    {
        private DateTime? dateCreated = null;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EmployeeID { get; set; }
        public int RoleID { get; set; }
        public System.DateTime Created_Date
        {
            get
            {
                return this.dateCreated.HasValue
                        ? this.dateCreated.Value
                        : DateTime.Now;
            }
            set
            {
                dateCreated = value;
            }
        }

        public virtual RolesMaster RolesMaster { get; set; }
    }
}