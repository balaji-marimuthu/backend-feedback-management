using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FeedbackDAL.Models
{
    public class RolesMaster
    {
        public RolesMaster()
        {
            this.AssociateRoles = new HashSet<AssociateRole>();
        }

        [Key]
        public int RoleID { get; set; }
        public string RoleTitle { get; set; }
        public string Description { get; set; }
        public string Created_By { get; set; }
        public System.DateTime Created_Date { get {
                return DateTime.Now;
            } }
        public string Updated_By { get; set; }

        public System.DateTime Updated_Time { get { return DateTime.Now; } }
        public string Additional_Info { get; set; }

        public virtual ICollection<AssociateRole> AssociateRoles { get; set; }
    }
}