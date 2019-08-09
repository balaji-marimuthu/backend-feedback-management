using FeedbackDAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackDAL.DataAccessLayer
{
    [ExcludeFromCodeCoverage]

    public class FeedbackContext : DbContext
    {
        public FeedbackContext() : base("FeedbackContext")
        {
            Database.SetInitializer<FeedbackContext>(new CustomInitializer<FeedbackContext>());
        }

        public virtual DbSet<EnrollmentInformation> EnrollmentInformations { get; set; }
        public virtual DbSet<ParticipatedDetails> ParticipatedDetails { get; set; }

        public virtual DbSet<POCDetails> POCDetails { get; set; }

        public virtual DbSet<AssociateRole> AssociateRoles { get; set; }

        public virtual DbSet<RolesMaster> RolesMaster { get; set; }

        public virtual DbSet<MailLog> MailLogs { get; set; }

        public virtual DbSet<Feedbacks> Feedbacks { get; set; }

        public virtual DbSet<Feedback_Options> Feedback_Options { get; set; }

        public virtual DbSet<Feedback_Questions> Feedback_Questions { get; set; }

        public virtual DbSet<Feedback_Answers> Feedback_Answers { get; set; }

        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            // Database.SetInitializer<FeedbackContext>(null);

            modelBuilder.Entity<POCDetails>()
                .HasKey(c => new { c.EventID, c.POCID });

            modelBuilder.Entity<ParticipatedDetails>()
               .HasKey(c => new { c.EventID, c.EmployeeID });

            //modelBuilder.Entity<RolesMaster>()

            //   .HasOptional(s => s.AssociateRole) // Mark Address property optional in Student entity
            //   .WithRequired(ad => ad.RolesMaster)

            //   ;
            modelBuilder.Entity<Feedback_Options>()
                .HasMany(e => e.Feedbacks)
                .WithRequired(e => e.Feedback_Options)
                .HasForeignKey(e => e.FeedbackOptionID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Feedback_Questions>()
                .HasMany(e => e.Feedback_Answers)
                .WithRequired(e => e.Feedback_Questions)
                .HasForeignKey(e => e.FeedbackQuestionID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RolesMaster>()
                .HasMany(e => e.AssociateRoles)
                .WithRequired(e => e.RolesMaster)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }


    }


    [ExcludeFromCodeCoverage]
    public class CustomInitializer<T> : DropCreateDatabaseAlways<FeedbackContext>
    {
        public override void InitializeDatabase(FeedbackContext context)
        {
            try
            {
                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction
               , string.Format("ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", context.Database.Connection.Database));

            }
            catch { }
            base.InitializeDatabase(context);
        }

        protected override void Seed(FeedbackContext context)
        {
            // Seed code goes here...

            IList<Feedback_Options> feedback_Options = new List<Feedback_Options>();

            feedback_Options.Add(new Feedback_Options() { FeedbackOptions = "None" });

            feedback_Options.Add(new Feedback_Options() { FeedbackOptions = "Unexpected personal commitment" });
            feedback_Options.Add(new Feedback_Options() { FeedbackOptions = "Unexpected official work" });
            feedback_Options.Add(new Feedback_Options() { FeedbackOptions = "Event not what I expected" });
            feedback_Options.Add(new Feedback_Options() { FeedbackOptions = "Did not receive further information about event" });
            feedback_Options.Add(new Feedback_Options() { FeedbackOptions = "Incorrectly registered" });
            feedback_Options.Add(new Feedback_Options() { FeedbackOptions = "Do not wish to disclose" });


            context.Feedback_Options.AddRange(feedback_Options);

            IList<Feedback_Questions> _Questions = new List<Feedback_Questions>();
            _Questions.Add(new Feedback_Questions() { FeedbackQuestions = "What did you like about this volunteering activity?" });
            _Questions.Add(new Feedback_Questions() { FeedbackQuestions = "What can be improved in this volunteering activity?" });

            context.Feedback_Questions.AddRange(_Questions);


            IList<RolesMaster> rolesMasters = new List<RolesMaster>();
            IList<AssociateRole> roles = new List<AssociateRole>();


            rolesMasters.Add(new RolesMaster()
            {
                RoleTitle = "admin",
                Description = "Access to all dashboards",
                AssociateRoles = new List<AssociateRole>()
                {
                    new AssociateRole(){EmployeeID = 123480}
                }
            });
            rolesMasters.Add(new RolesMaster()
            {
                RoleTitle = "PMO",
                Description = "Access to all events",
                AssociateRoles = new List<AssociateRole>()
                {
                    new AssociateRole(){EmployeeID = 123456}
                }
            });
            rolesMasters.Add(new RolesMaster()
            {
                RoleTitle = "POC",
                Description = "Access to assigned events",
                AssociateRoles = new List<AssociateRole>()
                {
                    new AssociateRole(){EmployeeID = 123457}
                }
            });
            context.RolesMaster.AddRange(rolesMasters);




            //roles.Add(new AssociateRole()
            //{
            //    EmployeeID = 123480,
            //    RoleID = 1
            //});

            //roles.Add(new AssociateRole()
            //{
            //    EmployeeID = 123490,
            //    RoleID = rolesMasters.Where(s => s.RoleTitle == "PMO").Select(s => s.RoleID).First()
            //});

            context.AssociateRoles.AddRange(roles);


            IList<Users> users = new List<Users>();
            users.Add(new Users() { Name = "Administrator", UserName = "123480", Password = "password" });
            users.Add(new Users() { Name = "Associate", UserName = "123490", Password = "password" });
            users.Add(new Users() { Name = "PMO", UserName = "123456", Password = "password" });
            users.Add(new Users() { Name = "POC", UserName = "123457", Password = "password" });

            context.Users.AddRange(users);


            base.Seed(context);
        }


    }

}