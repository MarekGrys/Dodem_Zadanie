using Dodem_Zadanie.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Dodem_Zadanie.Models;
using System.Reflection;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace Dodem_Zadanie.Services
{
    public class DodemService: IDodemService
    {
        private readonly DodemDbContext _dbContext;
        private readonly SmtpSettings _smtpSettings;

        public DodemService(DodemDbContext dbContext, IOptions<SmtpSettings> smtpOptions)
        {
            _dbContext = dbContext;
            _smtpSettings = smtpOptions.Value;
        }

        public string SendEmail(string recipient, int templateID, PlaceholderBase model)
        {

            if (model == null)
                throw new Exception("Nieprawidłowe dane!");

            //Używane przy utworzonej bazie danych:
            var template = _dbContext.MailTemplates.FirstOrDefault(x => x.ID == templateID);

            //Używane w przypadku braku bazy danych:
            //var template = TestData().FirstOrDefault(x => x.ID == templateID);

            if (template == null)
                throw new Exception("Nieprawidłowe ID!");


            string result = template.Content;
            PropertyInfo[] properties = model.GetType().GetProperties();

            foreach (var property in properties)
            {
                string placeholder = $"{{{{{property.Name.ToLower()}}}}}";
                string value = property.GetValue(model)?.ToString() ?? "";
                if(value != "")
                {
                    result = result.Replace(placeholder, value);
                }  
            }

            if (result.Contains("{{"))
                throw new Exception("Nieprawidłowa liczba danych!");


            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("EduTech", _smtpSettings.Email));
            message.To.Add(MailboxAddress.Parse(recipient));
            message.Subject = template.Subject;
            message.Body = new TextPart("plain")
            {
                Text = result
            };

            using (var client = new SmtpClient())
            {
                var secureOption = _smtpSettings.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls;
                client.Connect(_smtpSettings.Server, _smtpSettings.Port, secureOption);
                client.Authenticate(_smtpSettings.Email, _smtpSettings.Password);
                client.Send(message);
                client.Disconnect(true);
            }

            return result;
        }

        public List<MailTemplate> GetTemplates()
        {
            //Używane przy utworzonej bazie danych:
            var templates = _dbContext.MailTemplates.ToList();

            //Używanie w przypadku braku bazy danych
            //var templates = TestData();

            if (templates == null)
                throw new Exception("Brak szablonów!");
            return templates;
        }

        public List<MailTemplate> TestData()
        {
         
            return new List<MailTemplate>()
            {

                new MailTemplate()
                {
                    ID = 7,
                    Name = "courseEnrollmentConfirmation",
                    Subject = "Potwierdzenie zapisu na kurs",
                    Content = "Cześć {{imie}},\r\n\r\nPotwierdzamy Twój zapis na kurs: {{coursetitle}}.\r\nData rozpoczęcia kursu: {{startdate}}.\r\nSzczegóły kursu znajdziesz na naszej stronie: {{courselink}}.\r\n\r\nPowodzenia,\r\nZespół EduTech"
                },
                new MailTemplate()
                {
                    ID = 8,
                    Name = "courseCompletionCertificate",
                    Subject = "Gratulacje z ukończenia kursu",
                    Content = "Cześć {{imie}},\r\n\r\nGratulujemy ukończenia kursu: {{coursetitle}}!\r\nKliknij poniższy link, aby pobrać swój certyfikat: {{certificatelink}}.\r\n\r\nŻyczymy dalszych sukcesów,\r\nZespół EduTech"
                }
            };
        }
    }
}
