using FeedbackApplication.Controllers;
using FeedbackDAL.DataAccessLayer;
using FeedbackDAL.Models;
using FeedbackApplication.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace FeedbackApplication.Tests.Controllers
{
    [TestClass]
    public class DashboardControllerTest : ExcelService
    {
        private Mock<FeedbackContext> mockContext;
        private Mock<DbSet<EnrollmentInformation>> mockEnrollmentInformations;
        private Mock<DbSet<POCDetails>> mockPOCDetails;
        private Mock<DbSet<ParticipatedDetails>> mockParticipatedDetails;
        private InputController controller;
        private DashboardController dashboardCtrl;

        private Mock<FilePath> filePath;

        [TestMethod]
        public void ProcessData()
        {
            //InitializeData();
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            controller.ProcessData();

            mockEnrollmentInformations.Verify(m => m.Add(It.IsAny<EnrollmentInformation>()), Times.AtLeastOnce());
            mockParticipatedDetails.Verify(m => m.Add(It.IsAny<ParticipatedDetails>()), Times.AtLeastOnce());
            mockPOCDetails.Verify(m => m.Add(It.IsAny<POCDetails>()), Times.AtLeastOnce());

            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce());

        }


        [TestMethod]
        public void Get()
        {
            var result = dashboardCtrl.Get();

            Assert.IsNotNull(result.ParticipatedDetails);
            Assert.IsNotNull(result.NotAttendedVolunteers);
            Assert.IsNotNull(result.POCDetails);
            Assert.IsNotNull(result.UnRegisteredVolunteers);
        }

        [TestMethod]
        public void ParticipatedData()
        {
            var result = dashboardCtrl.GetParticipatedDetails();

            Assert.AreEqual(result.Count, mockParticipatedDetails.Object.Count());
        }

        [TestMethod]
        public void POCData()
        {
            var result = dashboardCtrl.GetPOCDetails();

            Assert.AreEqual(result.Count, mockPOCDetails.Object.Count());
        }

        [TestMethod]
        public void UnNotAttendVolunteers()
        {
            var result = dashboardCtrl.GetNotAttendedVolunteerDetails();

            Assert.AreEqual(result.Count, mockEnrollmentInformations.Object.Count());
        }

        [TestMethod]
        public void UnRegisteredVolunteers()
        {
            var result = dashboardCtrl.GetUnRegisteredDetails();

            Assert.AreEqual(result.Count, mockEnrollmentInformations.Object.Count());
        }

        private void InitializeData()
        {
            mockContext = new Mock<FeedbackContext>();
            mockEnrollmentInformations = GetMockDbSet(new List<EnrollmentInformation>().AsQueryable());
            mockParticipatedDetails = GetMockDbSet(new List<ParticipatedDetails>().AsQueryable());
            mockPOCDetails = GetMockDbSet(new List<POCDetails>().AsQueryable());

            mockContext.Setup(c => c.EnrollmentInformations).Returns(mockEnrollmentInformations.Object);
            mockContext.Setup(c => c.POCDetails).Returns(mockPOCDetails.Object);
            mockContext.Setup(c => c.ParticipatedDetails).Returns(mockParticipatedDetails.Object);

            var mockResponseMessage = new Mock<HttpResponseMessage>();
            mockResponseMessage.Object.StatusCode = HttpStatusCode.OK;

            filePath = new Mock<FilePath>();
            controller = new InputController(mockContext.Object);
            dashboardCtrl = new DashboardController(mockContext.Object);

            var root = AppDomain.CurrentDomain.BaseDirectory;
            filePath.Setup(s => s.GetFilePath("NotAttended")).Returns(root + "\\InputFiles\\Volunteer_Enrollment Details_Not_Attend.xlsx");
            filePath.Setup(s => s.GetFilePath("NotParticipated")).Returns(root + "\\InputFiles\\Volunteer_Enrollment Details_Unregistered.xlsx");
            filePath.Setup(s => s.GetFilePath("POC")).Returns(root + "\\InputFiles\\Outreach Events Summary.xlsx");
            filePath.Setup(s => s.GetFilePath("Participated")).Returns(root + "\\InputFiles\\OutReach Event Information.xlsx");

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

        public override FeedbackContext GetDbContext()
        {
            InitializeData();
            dbcontext = mockContext.Object;
            return dbcontext;
        }


    }
}
