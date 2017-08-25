using System;
using System.Text;
using Autofac;
using ChatRoom.Common;
using ChatRoom.Common.RequestModel;
using ChatRoom.Common.Utils;
using ChatRoom.Interface;
using ChatRoom.Interface.IBuiness.Message;
using ChatRoom.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ChatRoom.Model.Message;

namespace ChatRoom
{
    /// <summary>
    /// Represent RabbitMQ Bus
    /// http://localhost:15672/#/connections
    /// </summary>
    public class MessageListener
    {
        private IConnection _connection;
        private IModel _channel;
        private IMessageBuiness _messageBll;

        #region Singleton
        private static readonly Lazy<MessageListener> _instance = new Lazy<MessageListener>(() => new MessageListener());
        public static MessageListener Instance() => _instance.Value;
        #endregion

        public void Start()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            this._messageBll=ChatRoomEnv.Container.Resolve<IMessageBuiness>();
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "test-exchange",
                type: "fanout",
                durable: true);

            var queueName = _channel.QueueDeclare().QueueName;

            _channel.QueueBind(queue: queueName,
                exchange: "test-exchange",
                routingKey: "");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += ConsumerOnReceived;

            _channel.BasicConsume(queue: queueName,
                autoAck: true,
                consumer: consumer);
        }

        public void Stop()
        {
            _channel.Close(200, "Goodbye");
            _connection.Close();
        }

        private void ConsumerOnReceived(object sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            
            var json = Encoding.UTF8.GetString(body);
            try
            {
                var message = JsonConvert.DeserializeObject<TransMessageModel>(json);
                if (!message.RelayFromId.HasValue)
                    return;
                this._messageBll.AddMessage(message.RelayFromId.Value, message.RelayToId.Value, message.EventType, message.MsgType,message.ContentType, message.Content);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(GetType(),ex);
            }
            
        }
    }
}