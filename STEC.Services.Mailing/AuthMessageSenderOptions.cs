using System;

namespace STEC.Services.Mailing
{
    public class AuthMessageSenderOptions : EmailOptionsBase
    {
        public string SendGridUser { get; set; }


        public string SendGridKey { get; set; }
    }
}