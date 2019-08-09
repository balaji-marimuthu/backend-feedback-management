using FeedbackDAL.DataAccessLayer;
using FeedbackDAL.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Web;

namespace FeedbackApplication.Service
{
    [ExcludeFromCodeCoverage]
    public class FileService
    {
        public static FeedbackContext db = new FeedbackContext();
        public static void ProcessInputData(string fileName = "", string path = "")
        {
            FileInfo filePath = new System.IO.FileInfo(path);

            switch (fileName)
            {
                case "OutReach Event Information.xlsx":
                    ExcelService.ProcessInputData<ParticipatedDetails>(filePath);
                    break;
                case "Outreach Events Summary.xlsx":
                    ExcelService.ProcessInputData<POCDetails>(filePath);
                    break;

                case "Volunteer_Enrollment Details_Not_Attend.xlsx":
                    ExcelService.ProcessInputData<EnrollmentInformation>(filePath);
                    break;

                case "Volunteer_Enrollment Details_Unregistered.xlsx":
                    var data = ExcelService.ProcessInputData<EnrollmentInformation>(filePath);
                   // data.ForEach((t) => db.EnrollmentInformations.Add(t));
                    //db.SaveChangesAsync();
                    break;
            }
        }
    }
}