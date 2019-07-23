using System;
using System.Collections.Generic;
using System.Text;
using BeetleX;
using BeetleX.Clients;

namespace Captura.Base
{
    public class Client
    {
        public void Start()
        {
            AsyncTcpClient client = SocketFactory.CreateClient<AsyncTcpClient>("127.0.0.1", 9091);
            //SSL
            //AsyncTcpClient client = SocketFactory.CreateSslClient<AsyncTcpClient>("127.0.0.1", 9090, "serviceName");
            client.ClientError = (o, e) =>
            {
                Console.WriteLine("client error {0}@{1}", e.Message, e.Error);
            };
            client.DataReceive = (o, e) =>
            {
                Console.WriteLine(e.Stream.ToPipeStream().ReadLine());
            };
            var pipestream = client.Stream.ToPipeStream();
            pipestream.WriteLine("hello henry");
            client.Stream.Flush();
            //Console.Read();
        }
        private static void Write(string value)
        {
            lock (typeof(Console))
            {
                Console.Write(value);
            }
        }
    }
}
