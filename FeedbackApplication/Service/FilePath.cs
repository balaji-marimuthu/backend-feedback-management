using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FeedbackApplication.Service
{
    public class FilePath
    {
        public string InputFolderPath
        {
            get
            {
                string inputDir = @"C:\HackFSE\";
                if (!Directory.Exists(inputDir))
                {
                    Directory.CreateDirectory(inputDir);
                }

                return inputDir;
            }
        }

        public string ParticipatedFile
        {
            get
            {
                return InputFolderPath + "OutReach Event Information.xlsx";
            }
        }


        public string POCFile
        {
            get
            {
                return InputFolderPath + "Outreach Events Summary.xlsx";
            }
        }

        public string NotParticipatedFile
        {
            get
            {
                return InputFolderPath + "Volunteer_Enrollment Details_Unregistered.xlsx";
            }
        }

        public string NotAttendedFile
        {
            get
            {
                return InputFolderPath + "Volunteer_Enrollment Details_Not_Attend.xlsx";
            }
        }

        public virtual string GetFilePath(string fileName)
        {
            string result = string.Empty;
            switch (fileName)
            {
                case "NotAttended":
                    result = NotAttendedFile;
                    break;
                case "NotParticipated":
                    result = NotParticipatedFile;
                    break;
                case "POC":
                    result = POCFile;
                    break;
                case "Participated":
                    result = ParticipatedFile;
                    break;
            }

            return result;
        }
    }
}