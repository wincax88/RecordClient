using System;
using System.Windows;
using System.Collections.Generic;
using System.Text;
using BeetleX;
using BeetleX.EventArgs;
using Newtonsoft.Json;
using static Captura.Base.CommandData;
using Captura.ViewModels;
using System.Net.Http;
using Captura.Base;

namespace Captura

{
    public class Server : ServerHandlerBase
    {
        private static IServer server;
        readonly Captura.ViewModels.RecordingViewModel _recordingViewModel;

        public Server()
        {
            _recordingViewModel = ServiceProvider.Get<RecordingViewModel>();
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
            CommandData data = new CommandData();
            try
            {
                data = (CommandData)Newtonsoft.Json.JsonConvert.DeserializeObject(jsonStr, typeof(CommandData));
            }
            catch
            {
            }
            return data;
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
                    Application.Current.Dispatcher.Invoke(
                        () =>
                        {
                            // start record
                            _recordingViewModel.StartRecordingEx();
                        });
                    
                }
                // {"type":"command","data":"startRecord"}
                else if (data.data == "stopRecord")
                {
                    CommandData answer = new CommandData();
                    answer.type = "command";
                    answer.data = "stoped";
                    string result = GetString(answer);
                    Console.WriteLine(result);
                    e.Session.Stream.ToPipeStream().WriteLine(result);
                    e.Session.Stream.Flush();
                    Application.Current.Dispatcher.Invoke(
                        () =>
                        {
                            // stop record
                            _recordingViewModel.StopRecordingEx();
                        });
                }
                // {"type":"command","data":"pauseRecord"}
                else if (data.data == "pauseRecord")
                {
                    CommandData answer = new CommandData();
                    answer.type = "command";
                    answer.data = "paused";
                    string result = GetString(answer);
                    Console.WriteLine(result);
                    e.Session.Stream.ToPipeStream().WriteLine(result);
                    e.Session.Stream.Flush();
                    Application.Current.Dispatcher.Invoke(
                        () =>
                        {
                            // stop record
                            _recordingViewModel.OnPauseExecuteEx();
                        });
                }
                // {"type":"command","data":"resumeRecord"}
                else if (data.data == "resumeRecord")
                {
                    CommandData answer = new CommandData();
                    answer.type = "command";
                    answer.data = "resumed";
                    string result = GetString(answer);
                    Console.WriteLine(result);
                    e.Session.Stream.ToPipeStream().WriteLine(result);
                    e.Session.Stream.Flush();
                    Application.Current.Dispatcher.Invoke(
                        () =>
                        {
                            // stop record
                            _recordingViewModel.OnPauseExecuteEx();
                        });
                }
            }
        }
    }
}
