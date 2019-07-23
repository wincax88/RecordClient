using System;
using System.Collections.Generic;
using System.Text;
using NamedPipeWrapper;

namespace Captura.Base
{
    public class NamedPipeClient
    {
        private static NamedPipeClient<string> _clientr;
        public void Start()
        {
            _clientr = new NamedPipeClient<string>("alldream.recorder.parent");
            _clientr.ServerMessage += OnServerMessage;
            _clientr.Error += OnError;
            _clientr.Start();

        }

        public void Stop()
        {
            _clientr.Stop();
        }

        private void OnServerMessage(NamedPipeConnection<string, string> connection, string message)
        {
            Console.WriteLine("Server says: {0}", message);
        }

        private void OnError(Exception exception)
        {
            Console.Error.WriteLine("ERROR: {0}", exception);
        }
    }
}
