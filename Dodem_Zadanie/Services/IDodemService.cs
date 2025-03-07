using Dodem_Zadanie.Entities;
using Dodem_Zadanie.Models;

namespace Dodem_Zadanie.Services
{
    public interface IDodemService
    {
        string SendEmail(string recipient, int templateID, PlaceholderBase model);
        List<MailTemplate> GetTemplates();
    }
}
