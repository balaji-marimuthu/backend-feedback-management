using FeedbackDAL.Models;
using FeedbackApplication.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FeedbackApplication.Controllers
{
    [RoutePrefix("api/Mail")]
    public class MailController : ApiController
    {
        [Route("GetMailLogs")]
        [HttpGet]
        public string Get()
        {
            return "value";
        }

        // GET: api/Mail/5
        [Route("GetMailLog/{EventID}/{EmployeeID}")]
        [HttpGet]
        public string GetMail(string EventID, int EmployeeID)
        {
            // get mail details from 
            return "value";
        }

        // POST: api/Mail
        [Route("SendMail")]
        [HttpPost]
        public HttpResponseMessage SendMail([FromBody]List<SendersList> senders)
        {
            try
            {
                if(senders.Count >= 1)
                {
                    var sendersList = senders.Take(1).ToList();

                    foreach (SendersList item in senders)
                    {
                        item.EmployeeID = 123490;
                    }

                    // update the db.MailLog
                    MailService.SendMail(sendersList);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        //[Route("SendCustomMail")]
        //public void SendCustomMail([FromBody]List<SendersList> value, string MailContent)
        //{

        //}

    }
}
