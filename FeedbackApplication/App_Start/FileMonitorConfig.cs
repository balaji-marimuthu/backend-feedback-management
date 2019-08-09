using FeedbackApplication.Controllers;
using FeedbackApplication.Service;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Web;

namespace FeedbackApplication
{
    [ExcludeFromCodeCoverage]
    public class FileMonitorConfig
    {
        private static DateTime lastRead = DateTime.MinValue;
     

        public static void RegisterWatcher()
        {
            FileSystemWatcher watcher = new FileSystemWatcher
            {
                Path = new FilePath().InputFolderPath,
                NotifyFilter = NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.FileName
                                     | NotifyFilters.DirectoryName,

                Filter = "*.xlsx",
                EnableRaisingEvents = true
            };
            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            DateTime lastWriteTime = File.GetLastWriteTime(e.FullPath);

            if (lastWriteTime != lastRead)
            {
                LogService.Logger().Debug(string.Format("File: {0} {1} \n", e.FullPath, e.ChangeType));

                lastRead = lastWriteTime;
                FileService.ProcessInputData(e.Name, e.FullPath);
               
            }

        }
    }
}