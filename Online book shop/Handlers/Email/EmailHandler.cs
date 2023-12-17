using MimeKit;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using MailKit;
namespace Online_book_shop.Handlers.Email
{
    public class EmailHandler
    {
        public static string Email3(string htmlString,string From,string To, string Subject, string displayName= "MusesBooks.com", int emailtype=0)
        {
            try
            {
                string host = System.Configuration.ConfigurationManager.AppSettings["EmailHost"];
                string user = System.Configuration.ConfigurationManager.AppSettings["EmailUser"];
                string pwd = System.Configuration.ConfigurationManager.AppSettings["EmailPwd"];
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("eranga.kdy.home@gmail.com", displayName);
                message.ReplyTo = new MailAddress("eranga.kdy.home@gmail.com", displayName);
                message.To.Add(new MailAddress(To));
                message.To.Add(new MailAddress("online@musespublishers.com"));
                message.Subject = Subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = htmlString;
                smtp.Port = 2587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("eranga.kdy.home@gmail.com", "Ranga@google");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                return "Done";
            }
            catch (Exception ex) { 
                return ex.Message; 
            }
        }
        public static string Email2(string htmlString, string From, string To, string Subject, string displayName = "MusesBooks.com")
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
                message.To.Add(new MailAddress("online@musespublishers.com"));

                message.Subject = Subject;
                message.IsBodyHtml = true;
                message.Body = htmlString;

                smtp.Port = 587;
                smtp.Host = host; // Use the value from your configuration
                smtp.EnableSsl = true;

                // Use the values from your configuration
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(user, pwd);

                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtp.Send(message);

                return "Done";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
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
        //public static string Email(string htmlString, string From, string To, string Subject, string displayName = "MusesBooks.com", int emailtype = 0)
        //{
        //    try
        //    {
        //        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
        //        var baseAddress = "https://api.zeptomail.com/v1.1/email";

        //        var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
        //        http.Accept = "application/json";
        //        http.ContentType = "application/json";
        //        http.Method = "POST";
        //        http.PreAuthenticate = true;
        //        http.Headers.Add("Authorization", "Zoho-enczapikey wSsVR61w+RKmC6wuzmCpdLw5mQxWUQigQRx50FTy4nb/TfnF88c5wxWaBQWnSPZKRTM9QmMT8r4rzRwEhDQGjdt/n1xTDSiF9mqRe1U4J3x17qnvhDzNWm1YlBqIKI8PxARpn2dnFMkl+g==");
        //        JObject parsedContent = JObject.Parse("{'from': { 'address': 'noreply@online.musespublishers.com'},'to': [{'email_address': {'address': '" + To+"','name': 'MusesBooks.com'}}],'subject':'"+ Subject + "','htmlbody':'"+ htmlString + "'}");
        //        Console.WriteLine(parsedContent.ToString());
        //        ASCIIEncoding encoding = new ASCIIEncoding();
        //        Byte[] bytes = encoding.GetBytes(parsedContent.ToString());

        //        Stream newStream = http.GetRequestStream();
        //        newStream.Write(bytes, 0, bytes.Length);
        //        newStream.Close();

        //        var response = http.GetResponse();

        //        var stream = response.GetResponseStream();
        //        var sr = new StreamReader(stream);
        //        var content = sr.ReadToEnd();
        //        Console.WriteLine(content);
        //        return "Done";
        //    }
        //    catch(Exception ex)
        //    {
        //        return ex.Message;
        //    }


        //}
        public static string Email(string htmlString, string From, string To, string Subject, string displayName = "MusesBooks.com", int emailtype = 0)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("noreply", "noreply@online.musespublishers.com"));
            message.To.Add(new MailboxAddress("MusesBooks.com", To));
            message.Subject = Subject;
            message.Body = new TextPart("html")
            {
                Text = htmlString
            };
            var client = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                client.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                client.Connect("smtp.zeptomail.com", 587, false);
                client.Authenticate("emailapikey", "wSsVR61w+RKmC6wuzmCpdLw5mQxWUQigQRx50FTy4nb/TfnF88c5wxWaBQWnSPZKRTM9QmMT8r4rzRwEhDQGjdt/n1xTDSiF9mqRe1U4J3x17qnvhDzNWm1YlBqIKI8PxARpn2dnFMkl+g==");
                client.Send(message);
                client.Disconnect(true);
                return "Done";
            }
            catch (Exception e)
            {
                return e.Message;
            }


        }
        public static void TestEmail2()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("noreply", "noreply@online.musespublishers.com"));
            message.To.Add(new MailboxAddress("MusesBooks.com", "eranga.kdy@gmail.com"));
            message.Subject = "Test Email";
            message.Body = new TextPart("html")
            {
                Text = "Test email sent successfully."
            };
            var client = new MailKit.Net.Smtp.SmtpClient(); 
            try
            {
                client.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                client.Connect("smtp.zeptomail.com", 587, false);
                client.Authenticate("emailapikey", "wSsVR61w+RKmC6wuzmCpdLw5mQxWUQigQRx50FTy4nb/TfnF88c5wxWaBQWnSPZKRTM9QmMT8r4rzRwEhDQGjdt/n1xTDSiF9mqRe1U4J3x17qnvhDzNWm1YlBqIKI8PxARpn2dnFMkl+g==");
                client.Send(message);
                client.Disconnect(true);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }

        }
    }
}