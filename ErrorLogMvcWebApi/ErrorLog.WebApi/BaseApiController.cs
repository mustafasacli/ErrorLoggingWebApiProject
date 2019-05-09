namespace ErrorLog.WebApi
{
    using Microsoft.Owin;
    using SimpleFileLogging;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Web;
    using System.Web.Http;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A controller for handling base apis. </summary>
    ///
    /// <remarks>   Msacli, 24.04.2019. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public abstract class BaseApiController : ApiController
    {
        private static Guid gd = Guid.Empty;

        private static object lockObj = new object();

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the activity ıd. </summary>
        ///
        /// <value> The activity Identifier. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static Guid ActivityId
        {
            get
            {
                return WebApiAppValues.ActivityId;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the request address. </summary>
        ///
        /// <value> The request address. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        protected string RequestAddress
        {
            get
            {
                string s = string.Empty;

                try
                {
                    s = GetIPFromServerVariables();
                }
                catch (Exception e)
                {
                    SimpleFileLogger.Instance.LogError(e);
                }

                return s;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the name of the request machine. </summary>
        ///
        /// <value> The name of the request machine. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
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
                    SimpleFileLogger.Instance.LogError(e);
                }

                return s;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the response address. </summary>
        ///
        /// <value> The response address. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        protected string ResponseAddress
        {
            get
            {
                string s = string.Empty;

                try
                {
                    s = GetIPFromDNS();
                }
                catch (Exception e)
                {
                    SimpleFileLogger.Instance.LogError(e);
                }

                return s;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets ip from DNS. </summary>
        ///
        /// <remarks>   Msacli, 24.04.2019. </remarks>
        ///
        /// <returns>   The ip from DNS. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
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
            catch (Exception e)
            {
                SimpleFileLogger.Instance.LogError(e);
            }

            return ipAdress;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets ip from server variables. </summary>
        ///
        /// <remarks>   Msacli, 24.04.2019. </remarks>
        ///
        /// <returns>   The ip from server variables. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private string GetIPFromServerVariables()
        {
            string ip = string.Empty;

            try
            {
                HttpContext context = HttpContext.Current;
                string ipAdress = context?.Request?.ServerVariables["HTTP_X_FORWARDED_FOR"];
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
                    ip = context?.Request?.ServerVariables["REMOTE_ADDR"];
                }

                if (string.IsNullOrEmpty(ip))
                {
                    /// ip = ((HttpContextBase)Request.Properties["MS_HttpContext"]).Request.UserHostAddress;
                    ip = GetClientIp();
                }
            }
            catch (Exception e)
            {
                SimpleFileLogger.Instance.LogError(e);
            }

            return ip ?? string.Empty;
        }

        private string GetClientIp()
        {
            if (Request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContext)Request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (Request.Properties.ContainsKey("MS_OwinContext"))
            {
                /// return ((HttpContext)Request.Properties["MS_OwinContext"]).Request.UserHostAddress;
                return (Request.Properties["MS_OwinContext"] as OwinContext)?.Request?.RemoteIpAddress;
            }
            else
            {
                return null;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the name of the user. </summary>
        ///
        /// <value> The name of the user. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
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
                    s = RequestContext?.Principal?.Identity?.Name;

                    if (string.IsNullOrWhiteSpace(s))
                        s = base.ControllerContext?.RequestContext?.Principal?.Identity?.Name;
                }
                catch (Exception ex)
                {
                    SimpleFileLogger.Instance.LogError(ex);
                    try
                    {
                        //var i = this.Logger?.Error(ex, this.RequestAddress, this.ResponseAddress, Environment.MachineName);

                        //this.FileLogger?.Log(ex);
                    }
                    catch { }
                }

                return s ?? string.Empty;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Logs a debug. </summary>
        ///
        /// <remarks>   Msacli, 24.04.2019. </remarks>
        ///
        /// <param name="className">    Name of the class. </param>
        /// <param name="methodName">   Name of the method. </param>
        /// <param name="message">      The message. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
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
                SimpleFileLogger.Instance.LogError(ex);
                try
                {
                    // var i = this.Logger?.Error(ex, this.RequestAddress, this.ResponseAddress, Environment.MachineName);

                    // this.FileLogger?.Log(ex);
                }
                catch { }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the info arrays in this collection. </summary>
        ///
        /// <remarks>   Msacli, 24.04.2019. </remarks>
        ///
        /// <returns>
        /// An enumerator that allows foreach to be used to process the info arrays in this collection.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        protected IEnumerable<string> GetInfoArray()
        {
            yield return $"RequestAddress : {this.RequestAddress}";
            yield return $"ResponseAddress : {this.ResponseAddress}";
            yield return $"GetUserName : {this.GetUserName}";
            yield return $"MachineName : {Environment.MachineName}";
        }
    }
}