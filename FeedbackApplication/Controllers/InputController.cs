using FeedbackDAL.DataAccessLayer;
using FeedbackDAL.Models;
using FeedbackApplication.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Permissions;
using System.Web.Http;

namespace FeedbackApplication.Controllers
{
    //[Authorize]
    [RoutePrefix("api/input")]
    public class InputController : ApiController
    {
        private FeedbackContext db = new FeedbackContext();
        private FilePath path = new FilePath();

        public InputController() { }

        public InputController(FeedbackContext context)
        {
            db = context;
        }

        [Route("processData/{feedback:bool?}")]
        [HttpGet]
        public HttpResponseMessage ProcessData(bool feedback = false)
        {
            ProcessInputData(feedback);
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }

        [Route("ProcessFeedbackData")]
        [HttpGet]
        public HttpResponseMessage ProcessFeedbackData()
        {
            LoadTempFeedbackData();
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }

        public void ProcessInputData(bool feedback = false)
        {
            LoadParticipatedDetails();
            LoadPOCDetails();
            LoadUnRegisteredDetails();
            LoadNotAttendedVolunteerDetails();
            if (feedback)
            {
                LoadTempFeedbackData();
            } 
        }

        private void LoadTempFeedbackData()
        {
            IList<Feedbacks> details = new List<Feedbacks>();

            var participatedDetails = db.ParticipatedDetails.ToList();

            List<int> empIds = new List<int>()
            {
               123471, 123472, 123473,  123487,123462,  123490, 123491, 123492,123474,123498, 123499, 123500, 123450,  123454, 123483, 123484, 123485, 123455, 123456, 123457, 123458, 123459,
                123460, 123461,   123469,
                123470,  123475, 123476,123466, 123467, 123468,
               123482,  123486, 123463, 123464, 123465, 123488, 123489,
               123493, 123494, 123495, 123496, 123497,
                123451, 123452, 123453, 123477, 123478, 123479, 123480, 123481,
            };

            int i = 0;
            foreach (int empId in empIds)
            {
                i++;
                if (i < participatedDetails.Count())
                {
                    var item = participatedDetails[i];

                    var pocInfo = db.POCDetails.Where(s => s.EventID == item.EventID).FirstOrDefault();
                    Random generator = new Random();
                    Random randEmpId = new Random();

                    var randRating = generator.Next(1, 6);

                    details.Add(new Feedbacks()
                    {
                        EmployeeID = empId,
                        Rating = randRating > 5 ? 5 : randRating,
                        EventID = item.EventID,
                        Location = item.BaseLocation,
                        EventDate = item.EventDate,
                        EventName = item.EventName,
                        Project = pocInfo != null ? pocInfo.Project : "Community Program",
                        EmployeeName = item.EmployeeName,
                        FeedbackOptionID = 1,

                    });

                }
            }

            var uniqueData = details.GroupBy(x => x.EmployeeID)
                                  .Select(g => g.First())
                                  .ToList();

            db.Feedbacks.AddRange(uniqueData);

            db.SaveChanges();
            
        }

        private void LoadNotAttendedVolunteerDetails()
        {
            FileInfo filePath = new System.IO.FileInfo(path.GetFilePath("NotAttended"));
            var pocDetails = ExcelService.ProcessInputData<EnrollmentInformation>(filePath);
        }

        private void LoadUnRegisteredDetails()
        {
            FileInfo filePath = new System.IO.FileInfo(path.GetFilePath("NotParticipated"));
            var pocDetails = ExcelService.ProcessInputData<EnrollmentInformation>(filePath);
        }

        private void LoadPOCDetails()
        {
            FileInfo filePath = new System.IO.FileInfo(path.GetFilePath("POC"));
            var pocDetails = ExcelService.ProcessInputData<POCDetails>(filePath);
        }

        private void LoadParticipatedDetails()
        {
            FileInfo filePath = new System.IO.FileInfo(path.GetFilePath("Participated"));
            var participatedDetails = ExcelService.ProcessInputData<ParticipatedDetails>(filePath);
        }
    }
}
