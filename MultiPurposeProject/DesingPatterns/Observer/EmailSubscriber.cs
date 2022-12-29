using MongoDB.Bson.Serialization.Serializers;
using System.Net.Mail;
using System.Net;
using System.Net.Http;

namespace MultiPurposeProject.DesingPatterns.Observer
{
    public class EmailSubscriber : ISubscriber
    {
        private string _email;
        private string _message;

        public EmailSubscriber(string email, string message)
        {
            _email = email;
            _message = message;
        }

        public void Update(string data)
        {
            var smtpClient = new SmtpClient("smtp.live.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("email", "password"),
                EnableSsl = true,
            };

            smtpClient.Send("email", _email, "Something has Changed", $"{_message} {data}");
        }

    }
}
