using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeetleX;
using BeetleX.Clients;
using static Captura.Base.CommandData;
using Newtonsoft.Json;
using Captura.Base;

namespace Captura.Core
{
    public class SocketClient
    {
        private static TcpClient client;
        public void Start()
        {
            client = SocketFactory.CreateClient<TcpClient>("127.0.0.1", 9291);
            //SSL
            //AsyncTcpClient client = SocketFactory.CreateSslClient<AsyncTcpClient>("127.0.0.1", 9090, "serviceName");
            /*
            client.ClientError = (o, e) =>
            {
                Console.WriteLine("client error {0}@{1}", e.Message, e.Error);
            };
            client.DataReceive = (o, e) =>
            {
                Console.WriteLine(e.Stream.ToPipeStream().ReadToEnd());
            };
            client.Disconnected = (IClient c) =>
            {
                Console.WriteLine("Disconnected");
            };*/
            client.Connected = (IClient c) =>
            {
                Console.WriteLine("Connected");
            };

            //Console.Read();
        }
        public void send(string status)
        {
            client.Connect();

            if (!client.IsConnected)
            {
                return;
            }
            CommandData message = new CommandData();
            message.type = "status";
            message.data = status;
            string result = GetString(message);
            
            var pipestream = client.Stream.ToPipeStream();
            pipestream.WriteLine(result);
            client.Stream.Flush();
        }
        public void Stop()
        {
            client.DisConnect();
        }

        private CommandData GetData(string jsonStr)
        {
            return (CommandData)Newtonsoft.Json.JsonConvert.DeserializeObject(jsonStr, typeof(CommandData));
        }
        private string GetString(CommandData data)
        {
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.DefaultValueHandling = DefaultValueHandling.Ignore;
            return (JsonConvert.SerializeObject(data, Formatting.None, jsetting));
        }

    }
}
