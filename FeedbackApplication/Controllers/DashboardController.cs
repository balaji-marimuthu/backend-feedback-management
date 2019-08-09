using FeedbackDAL.DataAccessLayer;
using FeedbackDAL.Models;
using FeedbackApplication.Service;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;


namespace FeedbackApplication.Controllers
{
    //[Authorize]

    [RoutePrefix("api/Dashboard")]
    public class DashboardController : ApiController
    {
        private FeedbackContext db = new FeedbackContext();

        public DashboardController() { }
        public DashboardController(FeedbackContext context)
        {
            db = context;
        }

        // GET: api/Dashboard
        public OutReachData Get()
        {
            var participatedDetails = db.ParticipatedDetails.ToList();
            var pocDetails = db.POCDetails.ToList();
            var unregistered = db.EnrollmentInformations.Where(s => s.IsRegistered == false).ToList();
            var notattended = db.EnrollmentInformations.Where(s => s.IsRegistered == true && s.IsParticipated == false).ToList();

            return new OutReachData
            {
                ParticipatedDetails = participatedDetails,
                POCDetails = pocDetails,
                NotAttendedVolunteers = notattended,
                UnRegisteredVolunteers = unregistered
            };

        }

        // GET: api/Dashboard/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        [Route("ExportToExcel")]
        [HttpGet]
        public HttpResponseMessage ExcelExport(string name = "")
        {
            
            dynamic collection = null;
            switch (name.ToLower())
            {
                case "participateddetails":
                    collection = GetParticipatedDetails();
                    break;
                case "pocdetails":
                    collection = GetPOCDetails();
                    break;
                case "unregisteredvolunteers":
                    collection = GetUnRegisteredDetails();
                    break;
                case "notattendedvolunteers":
                    collection = GetNotAttendedVolunteerDetails();
                    break;
                case "feedbackdetails":
                    collection = new FeedbackController().Get();
                    break;
                default:
                    collection = new List<ParticipatedDetails>();
                    break;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                LogService.Logger().Debug("File Name should be specified/Incorrect name provided to export");
                return Request.CreateErrorResponse(HttpStatusCode.NotImplemented, "File Name should be specified/Incorrect name provided to export");
            }

            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(name);
                ws.Cells["A1"].LoadFromCollection(collection, true);
                ws.Cells[ws.Dimension.Address].AutoFitColumns();

                var ms = new System.IO.MemoryStream();

                var stream = new MemoryStream(pck.GetAsByteArray());
                // processing the stream.

                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(stream.ToArray())
                };
                result.Content.Headers.ContentDisposition =
                    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = name + ".xlsx"
                    };
                result.Content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");

                return result;
            }
        }


        // POST: api/Dashboard
        //public void Post([FromBody]string value)
        //{
        //}

        [Route("Import")]
        public HttpResponseMessage Post()
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath("~/ImportFiles/Input/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    FileService.ProcessInputData(postedFile.FileName, filePath);

                    var destfilePath = HttpContext.Current.Server.MapPath("~/ImportFiles/Processed/" + postedFile.FileName);
                    File.Move(filePath, destfilePath);
                    File.Delete(destfilePath);
                }
                result = Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return result;
        }

        //// PUT: api/Dashboard/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Dashboard/5
        //public void Delete(int id)
        //{
        //}

        [Route("GetParticipatedData")]
        public List<ParticipatedDetails> GetParticipatedDetails()
        {
            return db.ParticipatedDetails.ToList();
        }

        [Route("GetPOCDetails")]
        public List<POCDetails> GetPOCDetails()
        {
            return db.POCDetails.ToList();
        }

        [Route("GetUnRegisteredVolunteers")]
        public List<EnrollmentInformation> GetUnRegisteredDetails()
        {
            return db.EnrollmentInformations.Where(s => s.IsRegistered == false).ToList();
        }

        [Route("GetNotAttendedVolunteers")]
        public List<EnrollmentInformation> GetNotAttendedVolunteerDetails()
        {
            return db.EnrollmentInformations.Where(s => s.IsRegistered == true && s.IsParticipated == false).ToList();
        }

    }
}
