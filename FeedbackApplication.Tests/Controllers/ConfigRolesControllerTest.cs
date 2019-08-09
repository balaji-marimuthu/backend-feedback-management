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
    
    public class ConfigRolesControllerTest
    {
        private ConfigRolesController controller;
        private Mock<FeedbackContext> mockContext;
        private Mock<DbSet<RolesMaster>> mockRolesMaster;
        private Mock<DbSet<AssociateRole>> mockAssociateRoles;
        private Mock<ModifyState> mockModifyState;

        public ConfigRolesControllerTest()
        {
            InitializeData();

        }

        [TestMethod]
        public void Get()
        {
            // Act
            IEnumerable<string> result = controller.Get();
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual("admin", result.ElementAt(0));
            Assert.AreEqual("PMO", result.ElementAt(1));
            Assert.AreEqual("POC", result.ElementAt(2));
        }

        [TestMethod]
        public void GetById()
        {
            // Act
            string result = controller.Get(123480);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("admin", result);
           
        }

        [TestMethod]
        public void Post()
        {
            controller.Post(new RoleData() { Id = 123490, Role = "PMO"});

            mockAssociateRoles.Verify(m => m.Add(It.IsAny<AssociateRole>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void Put()
        {
            var item = new RoleData() { Id = 123490, Role = "POC" };
            controller.Put(item);

            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void Delete()
        {

            // controller.Post(new RoleData() { Id = 123490, Role = "PMO" });
            controller.Delete(123490);

            mockAssociateRoles.Verify(m => m.Remove(It.IsAny<AssociateRole>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        public void InitializeData()
        {
            var rolesMasters = new List<RolesMaster>();

            rolesMasters.Add(new RolesMaster()
            {
                RoleID = 1,
                RoleTitle = "admin",
                Description = "Access to all dashboards",
                AssociateRoles = new List<AssociateRole>()
                {
                    new AssociateRole(){EmployeeID = 123480}
                }
            });
            rolesMasters.Add(new RolesMaster() { RoleID = 2, RoleTitle = "PMO", Description = "Access to all events" });
            rolesMasters.Add(new RolesMaster() {RoleID =3, RoleTitle = "POC", Description = "Access to assigned events" });


            mockRolesMaster = GetMockDbSet(rolesMasters.AsQueryable());

            var associateRoles = new List<AssociateRole>();
            associateRoles.Add(new AssociateRole() { EmployeeID = 123480,RoleID=1,
                RolesMaster = new RolesMaster()
                {
                    RoleTitle = "admin",
                    Description = "Access to all dashboards"
                }
            });
            associateRoles.Add(new AssociateRole()
            {
                EmployeeID = 123490,
                RoleID = 2,
                RolesMaster = new RolesMaster()
                {
                    RoleTitle = "PMO",
                }
            });

            mockAssociateRoles = GetMockDbSet(associateRoles.AsQueryable());


            mockContext = new Mock<FeedbackContext>();
            mockContext.Setup(c => c.RolesMaster).Returns(mockRolesMaster.Object);
            mockContext.Setup(c => c.AssociateRoles).Returns(mockAssociateRoles.Object);
            mockAssociateRoles.Setup(m => m.Include(It.IsAny<string>())).Returns(mockAssociateRoles.Object);

             mockModifyState = new Mock<ModifyState>();

            mockModifyState.Setup(c => c.SetEntityStateModified<AssociateRole>(It.IsAny<FeedbackContext>(),It.IsAny<AssociateRole>()));

            controller = new ConfigRolesController(mockContext.Object, mockModifyState.Object);
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
