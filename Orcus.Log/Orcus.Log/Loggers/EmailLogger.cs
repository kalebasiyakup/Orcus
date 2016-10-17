using System.Net.Mail;

namespace Orcus.Log
{
    public class EmailLogger : Logger
    {
        string _from, _to, _subject, _body;
        SmtpClient _smtpClient;

        public EmailLogger(string from, string to, string subject, string body, SmtpClient smtpClient)
        {
            _from = from;
            _to = to;
            _subject = subject;
            _body = body;
            _smtpClient = smtpClient;
        }

        protected override void WriteLogImpl(string message)
        {
            _smtpClient.Send(_from, _to, _subject, _body);
        }
    }
}
