using ErrorLog.Wcf.Library;
using System;
using System.ServiceModel;

namespace ErrorLog.Wcf.Service.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Uri uri = new Uri("http://127.0.0.1:1010/ErrorLogService");
            ServiceHost serviceHost = new ServiceHost(typeof(ErrorLogService));

            /// serviceHost.AddServiceEndpoint(typeof(IErrorLogService), new BasicHttpBinding(), uri.AbsoluteUri);
            WriteLine("Service is being worked.");
            serviceHost.Open();
            WriteLine("Service is working.");
            WriteLine("Please enter for closing service.");
            Console.ReadKey();
            WriteLine("Service is being closed.");
            serviceHost.Close();
            WriteLine("Service closed.");
            WriteLine("Please enter for closing.");
            Console.ReadKey();
        }

        private static void Write(string s)
        { Console.Write(s); }

        private static void WriteLine(string s)
        { Console.WriteLine(s); }
    }
}