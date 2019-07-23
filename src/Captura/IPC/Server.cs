using System;
using System.Collections.Generic;
using System.Text;
using BeetleX;
using BeetleX.EventArgs;
using Newtonsoft.Json;
using static Captura.Base.CommandData;
using Captura.ViewModels;
using System.Net.Http;

namespace Captura.Base
{
    public class Server : ServerHandlerBase
    {
        private static IServer server;
        readonly Captura.ViewModels.RecordingViewModel _RecordingViewModel;
        
            _RecordingViewModel=  ServiceProvider.Get<RecordingViewModel>();

        public Server()
        {

        }

        public void Start()
        {
            server = SocketFactory.CreateTcpServer<Server>();
            server.Options.AddListen(9092);
            Console.Write(server.Options.Encoding);
            
            //server.Options.DefaultListen.CertificateFile = "text.pfx";
            //server.Options.DefaultListen.SSL = true;
            //server.Options.DefaultListen.CertificatePassword = "123456";
            server.Open();
            Console.Write(server);
            //Console.Read();
        }

        public void Stop()
        {
            server.Dispose();
        }
        public override void SessionReceive(IServer server, SessionReceiveEventArgs e)
        {
            // Console.WriteLine("SessionReceive : " + e);
            string message = e.Stream.ToPipeStream().ReadToEnd(); // ReadUTF ReadLine ReadToEnd
            Console.WriteLine(message);
            // parse message
            // {"type":"command","data":"startRecord"}
            ParseMessage(message, e);

            //e.Session.Stream.ToPipeStream().WriteLine("hello " + name);
            //e.Session.Stream.ToPipeStream().WriteLine("hello");
            //e.Session.Stream.Flush();
            base.SessionReceive(server, e);
        }

        public override void Connected(IServer server, ConnectedEventArgs e)
        {
            Console.WriteLine("Connected");
        }
        public override void Connecting(IServer server, ConnectingEventArgs e)
        {
            Console.WriteLine("Connecting");
        }
        public override void Disconnect(IServer server, SessionEventArgs e)
        {
            Console.WriteLine("Disconnect");
        }
        public override void Error(IServer server, ServerErrorEventArgs e)
        {
            Console.WriteLine("Error");
        }
        public override void Log(IServer server, ServerLogEventArgs e)
        {
            Console.WriteLine("Log");
        }
        public override void SessionDetection(IServer server, SessionDetectionEventArgs e)
        {
            Console.WriteLine("SessionDetection");
        }
        public override void SessionPacketDecodeCompleted(IServer server, PacketDecodeCompletedEventArgs e)
        {
            Console.WriteLine("SessionPacketDecodeCompleted");
        }

        protected override void OnLogToConsole(IServer server, ServerLogEventArgs e)
        {
            Console.WriteLine("OnLogToConsole");
        }
        protected override void OnReceiveMessage(IServer server, ISession session, object message)
        {
            Console.WriteLine("OnReceiveMessage : " + message);
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

        private void ParseMessage(string message, SessionReceiveEventArgs e)
        {
            CommandData data = GetData(message);
            if (data.type == "command")
            {
                // {"type":"command","data":"startRecord"}
                if (data.data == "startRecord")
                {
                    // start record
                    // ServiceProvider.
                    CommandData answer = new CommandData();
                    answer.type = "command";
                    answer.data = "started";
                    string result = GetString(answer);
                    Console.WriteLine(result);
                    e.Session.Stream.ToPipeStream().WriteLine(result);
                    e.Session.Stream.Flush();
                }
            }
        }
    }
}
