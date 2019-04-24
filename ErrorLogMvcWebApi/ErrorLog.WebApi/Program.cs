namespace ErrorLog.WebApi
{
    using Business.Core.Interfaces;
    using Business.MongoDb;
    using Business.RavenDb;
    using Microsoft.Owin.Hosting;
    using SimpleInjector;
    using System;

    internal class Program
    {
        public static Container container;

        private static void Main(string[] args)
        {
            string baseAddress = "http://127.0.0.1:9091/";

            // Start OWIN host
            Console.WriteLine(string.Format("OWIN will be started with {0} adress.", baseAddress));
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Console.WriteLine("OWIN has started.");
                // Create HttpCient and make a request to api/values
                ////HttpClient client = new HttpClient();

                ////var response = client.GetAsync(baseAddress + "api/Log/Info").Result;

                ////Console.WriteLine(response);
                ////Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                // Create the container as usual.
                Bootstrap();

                // Here your usual Web API configuration stuff.
                Console.ReadLine();
            }
        }

        private static void Bootstrap()
        {
            // Create the container as usual.
            container = new Container();

            switch (AppValues.DbMode)
            {
                case 1:
                    container.Register<IErrorLogBusiness, ErrorLogMongoDbBusiness>(Lifestyle.Singleton);
                    break;

                case 2:
                    container.Register<IErrorLogBusiness, ErrorLogRavenDbBusiness>(Lifestyle.Singleton);
                    break;

                default:
                    break;
            }

            // Optionally verify the container.
            container.Verify();
        }
    }
}