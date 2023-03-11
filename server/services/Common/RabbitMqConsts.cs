namespace Common
{
    public class RabbitMqConsts
    {
        public const int Port = 5672;
        public const string HostName = "localhost";
        public const string RabbitMqRootUri = "rabbitmq://localhost:5672";
        public const string RabbitMqUri = "rabbitmq://localhost/";
        public const string UserName = "guest";
        public const string Password = "guest";
        public const string NotificationServiceQueue = "notification.service";
    }
}