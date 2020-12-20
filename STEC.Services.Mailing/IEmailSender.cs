using System;
using System.Threading.Tasks;

namespace STEC.Services.Mailing
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
