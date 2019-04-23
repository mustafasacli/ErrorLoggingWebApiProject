using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Http;

namespace ErrorLog.WebApi
{
    public abstract class BaseApiController : ApiController
    {
        protected Guid ActivityId { get { return AppValues.ActivityId; } }

        protected string RequestAddress
        {
            get
            {
                string s = string.Empty;

                try
                {
                    s = GetIPFromServerVariables();
                }
                catch (Exception e) { }

                return s;
            }
        }

        protected string RequestMachineName
        {
            get
            {
                string s = string.Empty;

                try
                {
                    HttpContext context = HttpContext.Current;
                    s = context.Request.UserHostName;

                    if (string.IsNullOrEmpty(s))
                    {
                        s = Dns.GetHostEntry(this.RequestAddress).HostName;
                    }
                }
                catch (Exception e)
                {

                }

                return s;
            }
        }

        protected string ResponseAddress
        {
            get
            {
                string s = string.Empty;

                try
                {
                    s = GetIPFromDNS();
                }
                catch (Exception e) { }

                return s;
            }
        }

        private string GetIPFromDNS()
        {
            string ipAdress = string.Empty;

            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAdress = ip.ToString();
                        break;
                    }
                }
            }
            catch
            {
            }

            return ipAdress;
        }

        private string GetIPFromServerVariables()
        {
            string ip = string.Empty;

            try
            {
                HttpContext context = HttpContext.Current;
                string ipAdress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ipAdress))
                {
                    string[] ipAdresses = ipAdress.Split(',');
                    if (ipAdresses.Length != 0)
                    {
                        ip = ipAdresses[0];
                    }
                }
                if (string.IsNullOrEmpty(ip))
                {
                    ip = context.Request.ServerVariables["REMOTE_ADDR"];
                }
            }
            catch (Exception e)
            {
            }

            return ip;
        }

        protected string GetUserName
        {
            get
            {
                /*
                System.Security.Claims.ClaimsPrincipal principal = Request.GetRequestContext().Principal as
                    System.Security.Claims.ClaimsPrincipal;

                var Name = System.Security.Claims.ClaimsPrincipal.Current.Identity.Name;
                var Name1 = User.Identity.Name;
                return Ok();
                */
                string s = null;
                //https://stackoverflow.com/questions/9768872/can-i-access-iidentity-from-web-api
                //base.ControllerContext.RequestContext.Principal.Identity.;
                try
                {
                    s = RequestContext.Principal.Identity.Name;
                    if (string.IsNullOrWhiteSpace(s))
                        s = base.ControllerContext.RequestContext.Principal.Identity.Name;
                }
                catch (Exception ex)
                {
                    try
                    {
                        //var i = this.Logger?.Error(ex, this.RequestAddress, this.ResponseAddress, Environment.MachineName);

                        //this.FileLogger?.Log(ex);
                    }
                    catch { }
                }

                return s;
            }
        }

        protected void LogDebug(string className, string methodName, string message)
        {
            try
            {
                //this.Logger?.Log(
                //                this.RequestAddress, this.ResponseAddress, System.Environment.MachineName, LogType.Debug,
                //                className, methodName, message);
            }
            catch (Exception ex)
            {
                try
                {
                    //    var i = this.Logger?.Error(ex, this.RequestAddress, this.ResponseAddress, Environment.MachineName);

                    //    this.FileLogger?.Log(ex);
                }
                catch { }
            }
        }

        protected IEnumerable<string> GetInfoArray()
        {
            yield return $"RequestAddress : {this.RequestAddress}";
            yield return $"ResponseAddress : {this.ResponseAddress}";
            yield return $"GetUserName : {this.GetUserName}";
            yield return $"MachineName : {Environment.MachineName}";
        }
    }
}
