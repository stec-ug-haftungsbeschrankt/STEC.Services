using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace STEC.Services.Mailing
{
    public class SendgridEmailSender : IEmailSender
    {
        public SendgridEmailSender(IOptions<AuthMessageSenderOptions> options, ILogger<SendgridEmailSender> logger)
        {
            if (options != null)
            {
                _options = options.Value;
            }
            _logger = logger;
        }

        private readonly ILogger _logger;

        private readonly AuthMessageSenderOptions _options;

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await Execute(_options.SendGridKey, subject, htmlMessage, email).ConfigureAwait(false);
        }

        private async Task<Response> Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                //From = new EmailAddress("info@stecug.de", Options.SendGridUser),
                From = new EmailAddress(_options.FromEmail, _options.FromName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            _logger.LogInformation("Sending E-Mail to {Email}", email);
            return await client.SendEmailAsync(msg).ConfigureAwait(false);
        }
    }
}