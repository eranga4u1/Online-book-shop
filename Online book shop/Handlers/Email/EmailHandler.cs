using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web;
namespace Online_book_shop.Handlers.Email
{
    public class EmailHandler
    {
        public static string Email(string htmlString,string From,string To, string Subject, string displayName= "MusesBooks.com")
        {
            try
            {
                string host = System.Configuration.ConfigurationManager.AppSettings["EmailHost"];
                string user = System.Configuration.ConfigurationManager.AppSettings["EmailUser"];
                string pwd = System.Configuration.ConfigurationManager.AppSettings["EmailPwd"];
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(user, displayName);
                message.To.Add(new MailAddress(To));
                message.Subject = Subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = htmlString;
                smtp.Port = 25;
                smtp.Host = host; //for gmail host  
                smtp.EnableSsl = false;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(user, pwd);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                return "Done";
            }
            catch (Exception ex) { return ex.Message; }
        }
        public static string ReadHtmlTemplate(string fileName)
        {
            try
            {
                return System.IO.File.ReadAllText(Path.Combine(HttpContext.Current.Server.MapPath("\\EmailTemplates\\"))+"/"+fileName);
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        public static string SetEmailParameter(string filename, string[] paras)
        {
            try
            {
                var template = ReadHtmlTemplate(filename);
                int arrayPosition = 0;
                foreach(string para in paras)
                {
                    template= template.Replace("["+ arrayPosition + "]", para);
                    arrayPosition = arrayPosition + 1;
                }
                return template;
            }
            catch(Exception ex)
            {
                return "";
            }
        }
        public static string SendMailWithAttachment(string htmlString, string From, string To, string Subject,string fileLocation,string fileName, string displayName = "MusesBooks.com")
        {
            try
            {
                string host = System.Configuration.ConfigurationManager.AppSettings["EmailHost"];
                string user = System.Configuration.ConfigurationManager.AppSettings["EmailUser"];
                string pwd = System.Configuration.ConfigurationManager.AppSettings["EmailPwd"];
                MailMessage mail = new MailMessage();
                mail.To.Add(To);
                //mail.To.Add("amit_jain_online@yahoo.com");
                mail.From = new MailAddress(user, displayName);
                mail.Subject = Subject;
                mail.Body = htmlString;
                mail.IsBodyHtml = true;

                //Attach file using FileUpload Control and put the file in memory stream
                if (fileName != null)
                {
                    mail.Attachments.Add(new Attachment(Path.Combine(HttpContext.Current.Server.MapPath(fileLocation), fileName)));
                }
                SmtpClient smtp = new SmtpClient();
                smtp.Host = host; //Or Your SMTP Server Address
                smtp.Credentials = new System.Net.NetworkCredential
                   (user, pwd);
                //Or your Smtp Email ID and Password
                smtp.EnableSsl = false;
                smtp.Send(mail);
                return "Done";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            
        }
    }
}