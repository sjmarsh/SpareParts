namespace SpareParts.API.Infrastructure
{
    public class ConfigurationException : Exception
    {
        public ConfigurationException(string message) : base(message)
        {
        }

        public ConfigurationException(string message, Exception innerExeption) : base(message, innerExeption)
        {
        }
    }
}
