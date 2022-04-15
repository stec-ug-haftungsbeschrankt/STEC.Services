using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using MailKit;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace STEC.Services.Mailing
{
    public class SmtpEmailSender : IEmailSender
    {
        public SmtpEmailSender(IOptions<SmtpOptions> options, ILogger<SmtpEmailSender> logger)
        {
            if (options != null)
            {
                _options = options.Value;
            }
            _logger = logger;
            ValidateOptions();
        }

        private readonly ILogger _logger;

        private readonly SmtpOptions _options;

        private static SmtpClient InitializeSmtpClient(SmtpOptions options)
        {
            return new SmtpClient(options.Host)
            {
                Port = options.Port,
                Credentials = new NetworkCredential(options.Username, options.Password),
                EnableSsl = true,
            };
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            await ExecuteMailKit(subject, message, email).ConfigureAwait(false);
        }


        public async Task ExecuteMailKit(string subject, string message, string email)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add (new MailboxAddress(_options.FromName, _options.FromEmail));
            mailMessage.To.Add(new MailboxAddress("", email));
            mailMessage.Subject = subject;

            mailMessage.Body = new TextPart(TextFormat.Html) {
                Text = message
            };

            using var client = new MailKit.Net.Smtp.SmtpClient();
            //client.ServerCertificateValidationCallback += RemoteCertificateValidationCallback;

            await client.ConnectAsync(_options.Host, _options.Port, true).ConfigureAwait(false);

            // Note: only needed if the SMTP server requires authentication
            await client.AuthenticateAsync(_options.Username, _options.Password).ConfigureAwait(false);

            _logger.LogInformation("Sending E-Mail to {Email}", email);
            await client.SendAsync(mailMessage).ConfigureAwait(false);
            await client.DisconnectAsync(true).ConfigureAwait(false);
        }


        /* 
         * Disabled by default, for debugging purposes only
         */
        private bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            _logger.LogInformation("Certificate Subject: {Subject}", certificate.Subject);
            _logger.LogInformation("Certificate Issuer: {Issuer}", certificate.Issuer);

            return true; // Accept everything
        }

        private void ValidateOptions()
        {
            if (string.IsNullOrEmpty(_options.FromEmail) ||
                string.IsNullOrEmpty(_options.FromName) ||
                string.IsNullOrEmpty(_options.Host) ||
                string.IsNullOrEmpty(_options.Username) ||
                string.IsNullOrEmpty(_options.Password))
            {
                throw new ArgumentException("Options object is invalid or null.");
            }
        }

        /*
         * Do not use! The old Smtp Client is obsolete
         */
        public async Task ExecuteSmtp(string subject, string message, string email)
        {
            using var smtpClient = InitializeSmtpClient(_options);
            using var mailMessage = new MailMessage
            {
                From = new MailAddress(_options.FromEmail, _options.FromName),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            _logger.LogInformation("Sending E-Mail to {Email}", email);
            await smtpClient.SendMailAsync(mailMessage).ConfigureAwait(false);
        }
    }
}