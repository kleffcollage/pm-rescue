using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using PropertyMataaz.Utilities.Abstrctions;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PropertyMataaz.Utilities
{
    public class EmailHandler : IEmailHandler
    {
        private readonly Globals _globals;
        private readonly IWebHostEnvironment _env;
        private readonly ISendGridClient _sendGridClient;

        public EmailHandler(IOptions<Globals> globals, IWebHostEnvironment env, ISendGridClient sendGridClient)
        {
            _globals = globals.Value;
            _env = env;
            _sendGridClient = sendGridClient;
        }


        // This is the code used to pass parameters into the compose method
        //List<KeyValuePair<string, string>> EmailParameters = new List<KeyValuePair<string, string>>();
        //EmailParameters.Add(new KeyValuePair<string, string>("AuthorizationCode", AuthorizationCode.ToUpper()));

        /// <summary>
        /// Send Email Via Send grid Rest API Service
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendEmail(string email, string subject, string message)
        {
            try
            {
                
                var SendGridMessage = new SendGridMessage
                {
                    From = new EmailAddress(_globals.SenderEmail,_globals.SendersName),
                    
                    Subject = subject,
                };

                SendGridMessage.AddTo(new EmailAddress(email));

                SendGridMessage.AddContent(MimeType.Html, message);

                var Response = await _sendGridClient.SendEmailAsync(SendGridMessage).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }
        }

        public string ComposeFromTemplate(string name, List<KeyValuePair<string, string>> customValues)
        {
            var emailTemplate = string.Empty;
            Logger.Info(Directory.GetCurrentDirectory());
            Logger.Info(Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()));
            var dirs = Directory.GetDirectories(Directory.GetCurrentDirectory());
            foreach (var dir in dirs)
            {
                Logger.Info(dir);
            }

            Logger.Info(Directory.GetParent(Directory.GetCurrentDirectory()).FullName);

            var emailFolder = Path.Combine(_env.WebRootPath, "EmailTemplates");

            if (!Directory.Exists(emailFolder))
            {
                Directory.CreateDirectory(emailFolder);

            }
            
            var path = Path.Combine(emailFolder, name ?? "EmailTemplate.html");

            using var TemplateFile = new FileStream(path, FileMode.Open);
            using StreamReader Reader = new StreamReader(TemplateFile);
            emailTemplate = Reader.ReadToEnd();
            

            if (customValues != null)
            {
                foreach (KeyValuePair<string, string> pair in customValues)
                {
                    emailTemplate = emailTemplate.Replace(pair.Key, pair.Value);
                }
            }
            emailTemplate = emailTemplate.Replace("Year", DateTime.Now.Year.ToString());
            emailTemplate = emailTemplate.Replace("LogoURL", _globals.LogoURL);
            emailTemplate = emailTemplate.Replace("Instagram", _globals.Instagram);
            emailTemplate = emailTemplate.Replace("Twitter", _globals.Twitter);
            emailTemplate = emailTemplate.Replace("Facebook", _globals.Facebook);

            return emailTemplate;
        }
    }
}
