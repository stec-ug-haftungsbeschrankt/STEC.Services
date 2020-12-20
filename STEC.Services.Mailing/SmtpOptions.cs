namespace STEC.Services.Mailing
{
    public class SmtpOptions : EmailOptionsBase
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}