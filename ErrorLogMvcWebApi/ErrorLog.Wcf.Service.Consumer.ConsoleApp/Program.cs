using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceProxy.Generic;
using System.ServiceModel;
using ErrorLog.Models;

namespace ErrorLog.Wcf.Service.Consumer.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var binding = new WSHttpBinding("WSHttpBinding_IErrorLogService");
            GenericServiceProxy proxy = new GenericServiceProxy("http://127.0.0.1:8081/ErrorLogService", binding);
            var errorModel =
                proxy.Dispatch<IErrorLogService, ErrorLogModel>(q => q.Get("3b5603b6-6e91-4d41-84d2-5de5e650a51c"));

            if (errorModel == null)
            {
                Console.WriteLine("Error Model is null");
            }
            else
            {
                Console.WriteLine(errorModel.ClassName);
                Console.WriteLine(errorModel.CreatedOn);
                Console.WriteLine(errorModel.CreatedOnUnixTimestamp);
                Console.WriteLine(errorModel.ExceptionData);
                Console.WriteLine(errorModel.Id);
                Console.WriteLine(errorModel.LogTime);
                Console.WriteLine(errorModel.LogTimeUnixTimestamp);
                Console.WriteLine(errorModel.Message);
                Console.WriteLine(errorModel.MethodName);
                Console.WriteLine(errorModel.RequestAddres);
                Console.WriteLine(errorModel.ResponseAddress);
                Console.WriteLine(errorModel.ResponseMachineName);
                Console.WriteLine(errorModel.StackTrace);
                Console.WriteLine(errorModel.UserId);
            }

            Console.ReadKey();
        }
    }
}
