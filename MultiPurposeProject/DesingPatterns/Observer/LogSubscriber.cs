using MongoDB.Bson.Serialization.Serializers;

namespace MultiPurposeProject.DesingPatterns.Observer
{
    public class LogSubscriber : ISubscriber
    {
        private string _logPath;
        private string _message;

        public LogSubscriber(string logPath, string message)
        {
            _logPath = logPath;
            _message = message;
        }

        public void Update(string data)
        {
            File.WriteAllText(_logPath, $"{_message} {data}");
        }

    }
}
