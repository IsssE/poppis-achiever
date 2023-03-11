namespace Common
{
    public class RabbitMqConsts
    {        
        public const string RabbitMqRootUri = "rabbitmq://localhost:5672";
        public const string RabbitMqHostName_local = "localhost";
        public const string RabbitMqHostName_container = "rabbitmq";
        public const string UserName = "guest";
        public const string Password = "guest";
        public const string NotificationServiceQueue = "notification.service";
    }
}