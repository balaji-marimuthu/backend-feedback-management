using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FeedbackApplication.Controllers;
using FeedbackDAL.DataAccessLayer;
using FeedbackDAL.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FeedbackApplication.Tests.Controllers
{
    [TestClass]
    public class FeedbackControllerTest
    {

        private FeedbackController controller;
        private Mock<FeedbackContext> mockContext;
        private Mock<DbSet<Feedback_Options>> mockFeedbackOptions;
        private Mock<DbSet<Feedback_Questions>> mockFeedbackQuestions;
        private Mock<DbSet<Feedback_Answers>> mockFeedbackAnswers;
        private Mock<DbSet<Feedbacks>> mockFeedbacks;

        public FeedbackControllerTest()
        {
            InitializeData();

        }

        [TestMethod]
        public void Get()
        {

            var result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Good", result.ElementAt(0).Comments);
            Assert.AreEqual(123480, result.ElementAt(0).EmployeeID);
            Assert.AreEqual("EVNT00047261", result.ElementAt(0).EventID);
            Assert.AreEqual("Bags of joy distribution", result.ElementAt(0).EventName);
            Assert.AreEqual(5, result.ElementAt(0).Rating);
            Assert.AreEqual("None", result.ElementAt(0).SelectedFeedbackOption);
            Assert.AreEqual(true, result.ElementAt(0).IsFeedbackCollected);

            Assert.AreEqual("Average", result.ElementAt(1).Comments);
            Assert.AreEqual(123490, result.ElementAt(1).EmployeeID);
            Assert.AreEqual("EVNT00046103", result.ElementAt(1).EventID);
            Assert.AreEqual("Improove in coordination", result.ElementAt(1).EventName);
            Assert.AreEqual(4, result.ElementAt(1).Rating);
            Assert.AreEqual("Unexpected personal commitment", result.ElementAt(1).SelectedFeedbackOption);
            Assert.AreEqual(true, result.ElementAt(1).IsFeedbackCollected);
        }

        [TestMethod]
        public void GetById()
        {

            var result = controller.Get(123480);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Good", result.Comments);
            Assert.AreEqual(123480, result.EmployeeID);
            Assert.AreEqual("EVNT00047261", result.EventID);
            Assert.AreEqual("Bags of joy distribution", result.EventName);
            Assert.AreEqual(5, result.Rating);
            Assert.AreEqual("None", result.SelectedFeedbackOption);
            Assert.AreEqual(true, result.IsFeedbackCollected);
        }

        [TestMethod]
        public void GetByIdNegative()
        {

            var result = controller.Get(572360);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Comments);
            Assert.AreEqual(0, result.EmployeeID);
            Assert.IsNull(result.EventID);
            Assert.AreEqual(false, result.IsFeedbackCollected);
        }


        //[TestMethod]
        public void Post()
        {
            var feedbackData = new FeedbackData()
            {
                Comments = "Good", EmployeeID = 572366, EventID = "EVN001", EmployeeName = "Raj", EventName = "Be a Teacher", Rating = 4, SelectedFeedbackOption = "None", FeedbackAnswers = new List<string>() { "first answer", "second answer"}, FeedbackQuestions = new List<string>() { "What did you like about this volunteering activity?", "What can be improved in this volunteering activity?" }
            };


            controller.Post(feedbackData);

            mockFeedbacks.Verify(m => m.Add(It.IsAny<Feedbacks>()), Times.Once());
            mockFeedbackAnswers.Verify(m => m.Add(It.IsAny<Feedback_Answers>()), Times.AtLeastOnce());

            mockContext.Verify(m => m.SaveChanges(), Times.Once());

        }
        public void InitializeData()
        {


            var feedback_Options = new List<Feedback_Options>();

            feedback_Options.Add(new Feedback_Options() { FeedbackOptions = "None" });

            feedback_Options.Add(new Feedback_Options() { FeedbackOptions = "Unexpected personal commitment" });
            feedback_Options.Add(new Feedback_Options() { FeedbackOptions = "Unexpected official work" });
            feedback_Options.Add(new Feedback_Options() { FeedbackOptions = "Event not what I expected" });
            feedback_Options.Add(new Feedback_Options() { FeedbackOptions = "Did not receive further information about event" });
            feedback_Options.Add(new Feedback_Options() { FeedbackOptions = "Incorrectly registered" });
            feedback_Options.Add(new Feedback_Options() { FeedbackOptions = "Do not wish to disclose" });



            var feedbackQuestions = new List<Feedback_Questions>();
            feedbackQuestions.Add(new Feedback_Questions() { FeedbackQuestions = "What did you like about this volunteering activity?", ID = 1 });
            feedbackQuestions.Add(new Feedback_Questions() { FeedbackQuestions = "What can be improved in this volunteering activity?", ID = 2 });


            var feedbacks = new List<Feedbacks>();
            feedbacks.Add(new Feedbacks() { EventID = "EVNT00047261", Comments = "Good", CreatedDateTime = DateTime.Now, EmployeeID = 123480, EmployeeName = "Balaji", EventName = "Bags of joy distribution", FeedbackOptionID = 1, Rating = 5, ID = 1, Feedback_Options = new Feedback_Options() { FeedbackOptions = "None"} });
            feedbacks.Add(new Feedbacks() { EventID = "EVNT00046103", Comments = "Average", CreatedDateTime = DateTime.Now, EmployeeID = 123490, EmployeeName = "Sundar", EventName = "Improove in coordination", FeedbackOptionID = 2, Rating = 4, ID = 2, Feedback_Options = new Feedback_Options() { FeedbackOptions = "Unexpected personal commitment" } });
            
            var feedbackAnswers = new List<Feedback_Answers>();
            feedbackAnswers.Add(new Feedback_Answers() { EmployeeID= 123480, FeedbackAnswers = "First answer", FeedbackQuestionID =1 , Feedback_Questions = feedbackQuestions.First() });
            feedbackAnswers.Add(new Feedback_Answers() { EmployeeID = 123480, FeedbackAnswers = "Second answer", FeedbackQuestionID = 2,  Feedback_Questions = feedbackQuestions.Last() });
            feedbackAnswers.Add(new Feedback_Answers() { EmployeeID = 123490, FeedbackAnswers = "Third answer", FeedbackQuestionID = 1,  Feedback_Questions = feedbackQuestions.First() });
            feedbackAnswers.Add(new Feedback_Answers() { EmployeeID = 123490, FeedbackAnswers = "Fourth answer", FeedbackQuestionID = 2,  Feedback_Questions = feedbackQuestions.Last() });


            mockFeedbackOptions = GetMockDbSet(feedback_Options.AsQueryable());
            mockFeedbackQuestions = GetMockDbSet(feedbackQuestions.AsQueryable());
            mockFeedbackAnswers = GetMockDbSet(feedbackAnswers.AsQueryable());
            mockFeedbacks = GetMockDbSet(feedbacks.AsQueryable());

            mockContext = new Mock<FeedbackContext>();
            mockContext.Setup(c => c.Feedbacks).Returns(mockFeedbacks.Object);
            mockContext.Setup(c => c.Feedback_Options).Returns(mockFeedbackOptions.Object);
            mockContext.Setup(c => c.Feedback_Answers).Returns(mockFeedbackAnswers.Object);
            mockContext.Setup(c => c.Feedback_Questions).Returns(mockFeedbackQuestions.Object);

            mockFeedbacks.Setup(m => m.Include(It.IsAny<string>())).Returns(mockFeedbacks.Object);
            mockFeedbackAnswers.Setup(m => m.Include(It.IsAny<string>())).Returns(mockFeedbackAnswers.Object);


            var mockModifyState = new Mock<ModifyState>();

            mockModifyState.Setup(c => c.SetEntityStateModified<AssociateRole>(It.IsAny<FeedbackContext>(), It.IsAny<AssociateRole>()));

            controller = new FeedbackController(mockContext.Object, mockModifyState.Object);
        }

        private Mock<DbSet<T>> GetMockDbSet<T>(IQueryable<T> entities) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(entities.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(entities.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(entities.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(entities.GetEnumerator());
            return mockSet;
        }
    }
}
