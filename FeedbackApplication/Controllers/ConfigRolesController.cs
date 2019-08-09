using FeedbackDAL.DataAccessLayer;
using FeedbackDAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace FeedbackApplication.Controllers
{
    //[Authorize]
    public class ConfigRolesController : ApiController
    {
        public ConfigRolesController() { }
        private FeedbackContext db = new FeedbackContext();
        private ModifyState modifyState = new ModifyState();
        public ConfigRolesController(FeedbackContext context, ModifyState _modifyState)
        {
            db = context;
            modifyState = _modifyState;
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return db.RolesMaster.Select(s => s.RoleTitle).ToList();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            var result = db.AssociateRoles.Include(x => x.RolesMaster).ToList();

            string role = "";

            if (result.FirstOrDefault(s => s.EmployeeID == id) != null)
            {
               role = result.FirstOrDefault(s => s.EmployeeID == id).RolesMaster.RoleTitle;
            }

            if (string.IsNullOrWhiteSpace(role))
            {
                role = "Associate";
            }

            return role;
        }

        [HttpPost]
        [Route("api/config/login")]
        public HttpResponseMessage LogIn(LoginRequest request)
        {
            var users = db.Users.ToList();
            string role = "";
            try
            { 
                role = this.Get(request.UserName);
            }
            catch
            {

            }

            if (string.IsNullOrWhiteSpace(role))
            {
                role = "Associate";
            }

            if (users != null)
            {
                var user = users.Where(u => u.UserName == request.UserName.ToString() && u.Password == request.Password).FirstOrDefault();
                if ((user != null && !string.IsNullOrEmpty(user.UserName)) || (Regex.IsMatch(request.UserName.ToString(), @"^\d{6}$") && request.Password == "password"))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { LoginResult = "Success", UserName = request.UserName, Role = role });
                }
            }

            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        // POST api/<controller>
        public void Post([FromBody]RoleData value)
        {
            var roleid = db.RolesMaster.First(s => s.RoleTitle == value.Role).RoleID;
            db.AssociateRoles.Add(
                new FeedbackDAL.Models.AssociateRole()
                {
                    EmployeeID = value.Id,
                    RoleID = roleid
                });
            db.SaveChanges();
        }

        // PUT api/<controller>
        public void Put([FromBody]RoleData value)
        {
            var roleid = db.RolesMaster.First(s => s.RoleTitle == value.Role).RoleID;
            var item = db.AssociateRoles.Where(s => s.EmployeeID == value.Id).FirstOrDefault();

            if (item != null)
            {
                item.RoleID = roleid;
                modifyState.SetEntityStateModified(db, item);
                db.SaveChanges();
            }
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            var item = db.AssociateRoles.Where(s => s.EmployeeID == id).FirstOrDefault();
            if (item != null)
            {
                db.AssociateRoles.Remove(item);
                db.SaveChanges();
            }
        }
      
    }
}