using FeedbackDAL.DataAccessLayer;
using FeedbackDAL.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Hosting;

namespace FeedbackApplication.Service
{
    [ExcludeFromCodeCoverage]
    public static class MailService
    {

        public static void SendMail(List<SendersList> senders)
        {
            foreach(SendersList sender in senders)
            {
                SendMail(sender);
            }
        }

        private static void SendMail(SendersList sender)
        {
            var fromAddress = new MailAddress("");
            var fromPassword = "";
            var toAddress = new MailAddress("");


            string subject = "Feedback-OutReach Team";
            string body = "body";

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient
            {
                Host = "smtp-mail.outlook.com",
                //Host = "smtp.office365.com"
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)

            };

            //HostingEnvironment.MapPath("~/ Content / css")


            if (sender.MailType == MailType.Thank)
            {
                var path = HostingEnvironment.MapPath("~/Templates/ThankTemplate.html");
                body = System.IO.File.ReadAllText(path);
            }
            else
            {
                var path = HostingEnvironment.MapPath("~/Templates/FeedbackTemplate.html");
                //var path2 = System.Web.HttpContext.Current.Server.MapPath("~/Templates/FeedbackTemplate.html");
                body = System.IO.File.ReadAllText(path);

                body = body.Replace("{{:feedbackUrl}}", "http://localhost:4200");
                body = body.Replace("{{:EventDate}}", sender.EventDate);
            }

            var mail = new MailMessage(fromAddress, toAddress);

            mail.Subject = subject;
            // Body = body,
            mail.IsBodyHtml = true;
            mail.Body = body;
            smtp.Send(mail);

            using (FeedbackContext dbcontext = new FeedbackContext())
            {
                try
                {
                    dbcontext.MailLogs.Add(new MailLog()
                    {
                        EmployeeID = sender.EmployeeID,
                        EventID = sender.EventID.ToString(),
                        IsMailSent = true,
                        MailSentDateTime = DateTime.Now
                    });

                    dbcontext.SaveChanges();
                }
                catch { }
            }
               

        }

        private static void SendMail1(string to)
        {
            var fromAddress = new MailAddress("");
            var fromPassword = "";
            var toAddress = new MailAddress("");

            string subject = "subject";
            string body = "body";

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient
            {
                Host = "smtp-mail.outlook.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)

            };

            body = System.IO.File.ReadAllText(@"C:\Users\karthik\Source\Repos\ConsoleApp3\ConsoleApp3\mailtemplate.html");


            //create Alrternative HTML view
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");

            string imgPath1 = @"C:\Users\karthik\Source\Repos\ConsoleApp3\ConsoleApp3\smileys\smiley-1.png";
            string imgPath2 = @"C:\Users\karthik\Source\Repos\ConsoleApp3\ConsoleApp3\smileys\smiley-2.png";
            string imgPath3 = @"C:\Users\karthik\Source\Repos\ConsoleApp3\ConsoleApp3\smileys\smiley-3.png";
            string imgPath4 = @"C:\Users\karthik\Source\Repos\ConsoleApp3\ConsoleApp3\smileys\smiley-4.jpg";
            string imgPath5 = @"C:\Users\karthik\Source\Repos\ConsoleApp3\ConsoleApp3\smileys\smiley-5.png";

            //Add Image
            LinkedResource img1 = new LinkedResource(imgPath1, "image/bmp");
            img1.ContentId = "smiley1";

            LinkedResource img2 = new LinkedResource(imgPath2, "image/bmp");
            img2.ContentId = "smiley2";

            LinkedResource img3 = new LinkedResource(imgPath3, "image/bmp");
            img3.ContentId = "smiley3";

            LinkedResource img4 = new LinkedResource(imgPath4, "image/bmp");
            img4.ContentId = "smiley4";

            LinkedResource img5 = new LinkedResource(imgPath5, "image/bmp");
            img5.ContentId = "smiley5";

            //Add the Image to the Alternate view
            htmlView.LinkedResources.Add(img1);
            htmlView.LinkedResources.Add(img2);
            htmlView.LinkedResources.Add(img3);
            htmlView.LinkedResources.Add(img4);
            htmlView.LinkedResources.Add(img5);

            //Add view to the Email Message
            // mail.AlternateViews.Add(htmlView);

            htmlView.TransferEncoding = TransferEncoding.QuotedPrintable;

            var mail = new MailMessage(fromAddress, toAddress);

            mail.Subject = subject;
            // Body = body,
            mail.IsBodyHtml = true;

            mail.AlternateViews.Add(htmlView);


            //Attachment att1 = new Attachment(imgPath1);
            //att1.ContentId = "smiley1";
            //att1.ContentDisposition.Inline = true;
            //att1.ContentDisposition.DispositionType = DispositionTypeNames.Inline; 

            //Attachment att2 = new Attachment(imgPath2);
            //att2.ContentId = "smiley2";
            //att2.ContentDisposition.Inline = true;
            //att2.ContentDisposition.DispositionType = DispositionTypeNames.Inline;

            //Attachment att3 = new Attachment(imgPath3);
            //att3.ContentId = "smiley3";
            //att3.ContentDisposition.Inline = true;
            //att3.ContentDisposition.DispositionType = DispositionTypeNames.Inline;

            //Attachment att4 = new Attachment(imgPath4);
            //att4.ContentId = "smiley4";
            //att4.ContentDisposition.Inline = true;
            //att4.ContentDisposition.DispositionType = DispositionTypeNames.Inline;

            //Attachment att5 = new Attachment(imgPath5);
            //att5.ContentId = "smiley5";
            //att5.ContentDisposition.Inline = true;
            //att5.ContentDisposition.DispositionType = DispositionTypeNames.Inline;

            //mail.Attachments.Add(att1);
            //mail.Attachments.Add(att2);
            //mail.Attachments.Add(att3);
            //mail.Attachments.Add(att4);
            //mail.Attachments.Add(att5);

            //mail.Body = body;
            smtp.Send(mail);

            //var contentID = "Image";
            //var inlineLogo = new Attachment(@"C:\Desktop\Image.jpg");
            //inlineLogo.ContentId = contentID;
            //inlineLogo.ContentDisposition.Inline = true;
            //inlineLogo.ContentDisposition.DispositionType = DispositionTypeNames.Inline;

            //msg.IsBodyHtml = true;
            //msg.Attachments.Add(inlineLogo);
            //msg.Body = "<htm><body> <img src=\"cid:" + contentID + "\"> </body></html>";
        }
    }
}
