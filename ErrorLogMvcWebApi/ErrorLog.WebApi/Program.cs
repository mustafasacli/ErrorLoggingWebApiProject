namespace ErrorLog.WebApi
{
    using Microsoft.Owin.Hosting;
    using System;

    internal class Program
    {
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

                // Here your usual Web API configuration stuff.
                Console.ReadLine();
            }
        }
    }
}