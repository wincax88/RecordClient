using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json;

namespace Captura.Core
{
    public class SocketIoClient
    {
        public void Start()
        {
           
        }
        public void send(string status, string filepath = "")
        {
            var socket = IO.Socket("http://localhost:9292");
            socket.On(Socket.EVENT_CONNECT, () =>
            {
                string json = GetString(status, filepath);
                Console.WriteLine(json);
                socket.Emit("recoding_status", json);

                socket.Disconnect();

            });
        }
        public void Stop()
        {
            
        }

        private string GetString(string status, string filepath)
        {
            StatusData data = new StatusData();
            data.status = status;
            data.filepath = filepath;
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.DefaultValueHandling = DefaultValueHandling.Ignore;
            return (JsonConvert.SerializeObject(data, Formatting.None, jsetting));
        }

    }
}
