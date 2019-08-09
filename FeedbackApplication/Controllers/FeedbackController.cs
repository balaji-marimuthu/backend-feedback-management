using FeedbackDAL.DataAccessLayer;
using FeedbackDAL.Models;
using FeedbackApplication.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FeedbackApplication.Controllers
{
    [RoutePrefix("api/Feedback")]
    public class FeedbackController : ApiController
    {
        public FeedbackController() { }
        private FeedbackContext db = new FeedbackContext();
        private ModifyState modifyState = new ModifyState();
        public FeedbackController(FeedbackContext context, ModifyState _modifyState)
        {
            db = context;
            modifyState = _modifyState;
        }

        // GET: api/Feedback
        [Route("GetAllFeedbackDetails")]
        public IEnumerable<FeedbackData> Get()
        {
            var details = new List<FeedbackData>();

            var feedbacks = db.Feedbacks.Include(x => x.Feedback_Options).ToList();

            foreach (var feedback in feedbacks)
            {
                details.Add(getFeedbackById(feedback.EmployeeID, feedback));
            }

            return details;
        }

        // GET: api/Feedback/5
        [Route("GetFeedbackDetailById/{EmployeeID}")]
        [HttpGet]
        public FeedbackData Get(int EmployeeID)
        {
            try
            {
                var feedback = db.Feedbacks.Include(x => x.Feedback_Options)
                .Where(s => s.EmployeeID == EmployeeID).FirstOrDefault();


                // if feedback collected - feedback object has a value

                if (feedback != null)
                {
                    var feedbackData = getFeedbackById(EmployeeID, feedback);
                    return feedbackData;
                }
                else
                {
                    var fb = getFeedbackInformation(EmployeeID);
                    return fb;
                }
            }
            catch (Exception ex)
            {

            }


            return new FeedbackData();
        }

        private FeedbackData getFeedbackInformation(int employeeID)
        {
            var options = db.Feedback_Options.Select(s => s.FeedbackOptions).ToList<string>();
            var questions = db.Feedback_Questions.Select(s => s.FeedbackQuestions).ToList();
            var answers = new List<string>();
            var fb = new FeedbackData();
            fb.EmployeeID = employeeID;
            fb.FeedbackOptions = options;
            fb.FeedbackQuestions = questions;
            fb.FeedbackAnswers = answers;

            var eventInfo = db.ParticipatedDetails.Where(s => s.EmployeeID == employeeID.ToString()).FirstOrDefault();

            var enrollInfo = db.EnrollmentInformations.Where(s => s.EmployeeID == employeeID).FirstOrDefault();

            

            if(enrollInfo != null)
            {
                var unregister = enrollInfo.IsParticipated == false && enrollInfo.IsRegistered == false;

                fb.EventName = enrollInfo.EventName;
                fb.EventID = enrollInfo.EventID;
                fb.EventDate = enrollInfo.EventDate;
                fb.Location = enrollInfo.BaseLocation;
                fb.EnrollmentType = unregister ? "Unregistered" : "Notparticipated";

            }

            if (eventInfo != null)
            {
                fb.EventName = eventInfo.EventName;
                fb.EventID = eventInfo.EventID;
                fb.EventDate = eventInfo.EventDate;
                fb.Location = eventInfo.BaseLocation;
                fb.EnrollmentType = "Participated";
                var pocInfo = db.POCDetails.Where(s => s.EventID == eventInfo.EventID).FirstOrDefault();
                if (pocInfo != null)
                {
                    fb.Project = pocInfo.Project;
                }
            }

            return fb;
        }

        private FeedbackData getFeedbackById(int EmployeeID, Feedbacks feedback)
        {
            var options = db.Feedback_Options.Select(s => s.FeedbackOptions).ToList<string>();

            var items = db.Feedback_Answers.Include(x => x.Feedback_Questions).Where(s => s.EmployeeID == EmployeeID).ToList();

            var questions = items.Select(s => s.Feedback_Questions.FeedbackQuestions).ToList();
            var answers = items.Select(s => s.FeedbackAnswers).ToList();

            //var selectedOption = db.Feedback_Options.
            //    Where(s => s.FeedbackOptionID == feedback.FeedbackOptionID).FirstOrDefault();

            var obj = new FeedbackData()
            {
                EventID = feedback.EventID,
                EmployeeID = feedback.EmployeeID,
                EventName = feedback.EventName,
                EmployeeName = feedback.EmployeeName,
                Rating = feedback.Rating,
                Comments = feedback.Comments,
                FeedbackOptions = options,
                SelectedFeedbackOption = feedback.Feedback_Options.FeedbackOptions,
                FeedbackQuestions = questions,
                FeedbackAnswers = answers,
                Story = feedback.Story,
                EventDate = feedback.EventDate,
                Location = feedback.Location,
                Project = feedback.Project
            };
            return obj;
        }

        // POST: api/Feedback
        [Route("CollectFeedback")]
        [HttpPost]
        public void Post([FromBody]FeedbackData value)
        {
            var selectedOption = db.Feedback_Options.Where(s => s.FeedbackOptions == value.SelectedFeedbackOption).FirstOrDefault();

            db.Feedbacks.Add(
                new Feedbacks()
                {
                    EventID = value.EventID,
                    EmployeeID = value.EmployeeID,
                    EventName = value.EventName,
                    EmployeeName = value.EmployeeName,
                    Rating = value.Rating,
                    Comments = value.Comments,
                    FeedbackOptionID = selectedOption != null ? selectedOption.FeedbackOptionID : 1,
                    Story = value.Story,
                    EventDate = value.EventDate,
                    Location = value.Location,
                    Project = value.Project
                });

            var questions = db.Feedback_Questions.ToList();
            foreach (var ans in value.FeedbackAnswers)
            {
                int questionId = 1;
                var indx = value.FeedbackAnswers.IndexOf(ans);
                var id = questions.FirstOrDefault(s => s.FeedbackQuestions == value.FeedbackQuestions[indx]).ID;

                var question = questions.FirstOrDefault(s => s.FeedbackQuestions == value.FeedbackQuestions[indx]);

                if (question != null)
                {
                    questionId = question.ID;
                }

                db.Feedback_Answers.Add(new Feedback_Answers()
                {
                    EmployeeID = value.EmployeeID,
                    FeedbackAnswers = ans,
                    FeedbackQuestionID = questionId
                });
            }

            db.SaveChanges();

            var senderList1 = new SendersList { EmployeeID = 123480, MailType = MailType.Thank };
            var senderList2 = new SendersList { EmployeeID = 123490, MailType = MailType.Thank };

            MailService.SendMail(new List<SendersList>() { senderList1, senderList2 });
        }

       
    }
}
