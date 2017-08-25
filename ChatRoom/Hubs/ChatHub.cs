using System.Text;
using ChatRoom.Common.RequestModel;
using ChatRoom.Common.Utils;
using ChatRoom.Filter;
using ChatRoom.Hubs.Base;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace ChatRoom.Hubs
{
    public class ChatHub : HubBase
    {
        private readonly ConnectionFactory _connection;
        public ChatHub()
        {
            _connection = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
        }
        [HubAuth(Deny = "visitor")]
        public void SendMessage(TransMessageModel message)
        {
            do
            {
                if (message.Content == null)
                    break;
                message.Content = message.Content.EncodeEmjoy(ConfigurationHelper.EncodeEmjoyTemplate);
                if (message.EventType == "systemBroadcast"&& UserAuthContxt.User.UserType == "admin")
                {
                    Clients.All.broadcastMessage(message);
                    break;
                }
                if (message.EventType == "systemNotify" && UserAuthContxt.User.UserType != "admin")
                {
                    Clients.All.broadcastMessage(message);
                    break;
                }
                if (message.RelayToId.HasValue)
                {
                    if (message.RelayToId == ConfigurationHelper.DefultGroupId)
                    {
                        Clients.All.broadcastMessage(message);
                    }
                    else
                    {
                        Clients.Group(message.RelayToId.ToString()).broadcastMessage(message);
                    }
                    break;
                }
            } while (false);
            using (var connection = _connection.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "test-exchange",
                        type: "fanout",
                        durable: true);

                    var json = JsonConvert.SerializeObject(message);
                    var body = Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish(exchange: "test-exchange",
                        routingKey: "",
                        basicProperties: null,
                        body: body);
                }
            }
        }
        public void OnlineNotify(string state)
        {
            Clients.All.userStateNotify(new TransMessageModel()
            {
                RelayFromId = UserAuthContxt.User.Id,
                EventType = "notify",
                MsgType = "user_state_change_notify",
                ContentType = "text",
                Content = state
            });
        }
    }
}