using FeedbackDAL.DataAccessLayer;
using FeedbackDAL.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FeedbackApplication.Service
{
    public class ExcelService
    {
        private static string FileName = string.Empty;
        public static FeedbackContext dbcontext = new FeedbackContext();

        public ExcelService()
        {
            dbcontext = GetDbContext();
        }

        public virtual FeedbackContext GetDbContext()
        {
            dbcontext = new FeedbackContext();
            return dbcontext;
        }

        public static List<T> ProcessInputData<T>(FileInfo fileInfo) where T : new()
        {
            FileName = fileInfo.Name;
            //sheetName = "Existing Event Details";
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                int totalRows = workSheet.Dimension.Rows;
                int colCount = workSheet.Dimension.Columns;

                var properties = typeof(T).GetProperties();

                List<T> resultSet = new List<T>();

                for (int i = 2; i <= totalRows; i++)
                {
                    //Get the properties of T
                    var tprops = (new T())
                        .GetType()
                        .GetProperties()
                        .ToList();

                    var instance = new T();

                    for (int col = 1; col <= colCount; col++)
                    {
                        string value = workSheet.Cells[i, col].Value.ToString();

                        var prop = tprops.First(p => p.Name == tprops[col - 1].Name);

                        switch (prop.PropertyType.FullName)
                        {

                            case "System.String":
                                prop.SetValue(instance, Convert.ToString(value));
                                break;
                            case "System.Int32":
                                prop.SetValue(instance, Convert.ToInt32(value));
                                break;
                            case "System.DateTime":
                                var date = DateTime.Now;
                                if (DateTime.TryParse((string)value, out date))
                                {
                                    prop.SetValue(instance, date);
                                }
                                break;
                            case "System.Double":
                                prop.SetValue(instance, Convert.ToDouble(value));
                                break;
                        }
                    }

                    resultSet.Add(instance);
                    AddToEntity(instance);
                }

                dbcontext.SaveChanges();

                try
                {
                    SendMailToParticipants<T>();
                }
                catch (Exception ex)
                {
                    LogService.Logger().Error(ex);
                }

                return resultSet;
            }
        }

        private static void SendMailToParticipants<T>() where T : new()
        {
            if (typeof(T).Name == "ParticipatedDetails")
            {
                var items = dbcontext.ParticipatedDetails.ToList();
                var sendersList = new List<SendersList>();

                if (items.Count() >= 1)
                {
                    var item = items.OrderBy(s => s.ID).Take(1).FirstOrDefault();

                    if (item != null)
                    {
                        sendersList.Add(new SendersList { EmployeeID = 123480, EventDate = item.EventDate, EventName = item.EventName, EventID = item.EventID, MailType = MailType.Feedback });
                    }
                }

                if (items.Count() >= 2)
                {
                    var item = items.OrderBy(s => s.ID).Skip(1).Take(1).FirstOrDefault();

                    if (item != null)
                    {
                        sendersList.Add(new SendersList { EmployeeID = 123490, EventDate = item.EventDate, EventName = item.EventName, EventID = item.EventID, MailType = MailType.Feedback });
                    }
                }
                MailService.SendMail(sendersList);
            }
        }

        private static void AddToEntity<T>(T instance) where T : new()
        {
            switch (typeof(T).Name)
            {
                case "EnrollmentInformation":
                    var enrollmentdata = instance as EnrollmentInformation;

                    if (FileName.Contains("Volunteer_Enrollment Details_Unregistered"))
                    {
                        enrollmentdata.IsRegistered = false;
                    }
                    if (FileName.Contains("Volunteer_Enrollment Details_Not_Attend"))
                    {
                        enrollmentdata.IsRegistered = true;
                        enrollmentdata.IsParticipated = false;
                    }
                    dbcontext.EnrollmentInformations.Add(enrollmentdata);
                    break;

                case "ParticipatedDetails":
                    var data = instance as ParticipatedDetails;
                    dbcontext.ParticipatedDetails.Add(data);
                    break;
                case "POCDetails":
                    var pocData = instance as POCDetails;

                    if (pocData.POCID.Contains(";"))
                    {
                        try
                        {
                            var ids = pocData.POCID.Split(';');
                            var names = pocData.POCName.Split(';');

                            for (var i = 0; i < ids.Count(); i++)
                            {
                                dbcontext.POCDetails.Add(new POCDetails
                                {
                                    ActivityType = pocData.ActivityType,
                                    BaseLocation = pocData.BaseLocation,
                                    BeneficiaryName = pocData.BeneficiaryName,
                                    Category = pocData.Category,
                                    CouncilName = pocData.CouncilName,
                                    EventDate = pocData.EventDate,
                                    EventDescription = pocData.EventDescription,
                                    EventID = pocData.EventID,
                                    EventName = pocData.EventName,
                                    VenueAddress = pocData.VenueAddress,
                                    LivesImpacted = pocData.LivesImpacted,
                                    Month = pocData.Month,
                                    OverallVolunteeringHours = pocData.OverallVolunteeringHours,
                                    Project = pocData.Project,
                                    POCContactNumber = pocData.POCContactNumber,
                                    Status = pocData.Status,
                                    TotalNumberOfVolunteers = pocData.TotalNumberOfVolunteers,
                                    TotalTravelHours = pocData.TotalTravelHours,
                                    TotalVolunteerHours = pocData.TotalVolunteerHours,
                                    POCID = ids[i],
                                    POCName = i < names.Count() ? names[i] : names[0]
                                }
                                );
                            }
                        }
                        catch
                        {
                            dbcontext.POCDetails.Add(pocData);

                        }
                    }
                    else
                    {
                        dbcontext.POCDetails.Add(pocData);
                    }

                    break;

            }

        }
    }
}