using System;
using System.Collections.Generic;
using System.Text;
using NamedPipeWrapper;

namespace Captura.Base
{
    public class NamedPipeServer
    {
        private static NamedPipeServer<string> _server;
        public void Start()
        {
            _server = new NamedPipeServer<string>("\\\\.\\Pipe\\alldream.recorder.client");

            _server.ClientConnected += OnClientConnected;
            _server.ClientDisconnected += OnClientDisconnected;
            _server.ClientMessage += OnClientMessage;
            _server.Error += OnError;
            _server.Start();

        }

        public void Stop()
        {
            _server.Stop();
        }

        public static implicit operator NamedPipeServer(NamedPipeServer<NamedPipeServer> v)
        {
            throw new NotImplementedException();
        }
        private void OnClientConnected(NamedPipeConnection<string, string> connection)
        {
            Console.WriteLine("Client {0} is now connected!", connection.Id);
            connection.PushMessage("welcome");
        }

        private void OnClientDisconnected(NamedPipeConnection<string, string> connection)
        {
            Console.WriteLine("Client {0} disconnected", connection.Id);
        }

        private void OnClientMessage(NamedPipeConnection<string, string> connection, string message)
        {
            Console.WriteLine("Client {0} says: {1}", connection.Id, message);
        }

        private void OnError(Exception exception)
        {
            Console.Error.WriteLine("ERROR: {0}", exception);
        }
    }
}
