using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Diagnostics;

namespace Captura
{
    public class ProcessMonitoring
    {
        //private static readonly ManagementEventWatcher ProcessStartWatch = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
        // private static readonly ManagementEventWatcher ProcessStopWatch = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));
        ManagementEventWatcher startWatch;
        ManagementEventWatcher stopWatch;

        public ProcessMonitoring()
        {
            
        }
        public void Initialize()
        {
            WaitForProcess();
            /*
            ProcessStartWatch.EventArrived += startWatch_EventArrived;
            ProcessStartWatch.Start();

            ProcessStopWatch.EventArrived += stopWatch_EventArrived;
            ProcessStopWatch.Start();*/
        }

        public void TearDown()
        {
            startWatch.Stop();
            stopWatch.Stop();

            // ProcessStopWatch.Stop();
            // ProcessStartWatch.Stop();
        }
        /*
        private void stopWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            // CloudSetup.SendMessagesAsync("Win32_ProcessStopTrace", e.NewEvent.Properties);
            Console.WriteLine("Process Stop: {0}:{1}", e.NewEvent.Properties["ProcessID"].Value, e.NewEvent.Properties["ProcessName"].Value);
        }

        private void startWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            // CloudSetup.SendMessagesAsync("Win32_ProcessStartTrace", e.NewEvent.Properties);
            Console.WriteLine("Process Start: {0}:{1}", e.NewEvent.Properties["ProcessID"].Value, e.NewEvent.Properties["ProcessName"].Value);
        }*/
         void WaitForProcess()
        {
            startWatch = new ManagementEventWatcher(
              new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
            startWatch.EventArrived
                                += new EventArrivedEventHandler(startWatch_EventArrived);
            startWatch.Start();

            stopWatch = new ManagementEventWatcher(
              new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));
            stopWatch.EventArrived
                                += new EventArrivedEventHandler(stopWatch_EventArrived);
            stopWatch.Start();
        }

        static void stopWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            // stopWatch.Stop();
            Console.WriteLine("Process stopped: {0}"
                              , e.NewEvent.Properties["ProcessName"].Value);
        }

        static void startWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            // startWatch.Stop();
            Console.WriteLine("Process started: {0}"
                              , e.NewEvent.Properties["ProcessName"].Value);
        }
    }
        
}