namespace ErrorLog.WebApi
{
    using ErrorLog.Business.Core.Interfaces;
    using ErrorLog.Models;
    using IoC.Library;
    using SimpleFileLogging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;

    public class LogController : BaseApiController
    {
        private IErrorLogBusiness logBusiness;

        public LogController()//IErrorLogBusiness logBusiness)
        {
            this.logBusiness = ErrorLogIoC.Instance.Container.GetInstance<IErrorLogBusiness>();//logBusiness;
        }

        [ResponseType(typeof(string))]
        [AcceptVerbs("POST")]
        public IHttpActionResult Post([FromBody]ErrorLogModel log)
        {
            var result = string.Empty;

            try
            {
                result = logBusiness.Save(log);
            }
            catch (Exception e)
            {
                SimpleFileLogger.Instance.LogError(e);
                result = "err";
            }

            return Ok(result);
        }

        [ResponseType(typeof(ErrorLogModel))]
        [AcceptVerbs("GET")]
        public IHttpActionResult Get(string oid)
        {
            var result = new ErrorLogModel { };

            try
            {
                result = logBusiness.GetById(oid);
            }
            catch (Exception e)
            {
                SimpleFileLogger.Instance.LogError(e);
            }

            result = result ?? new ErrorLogModel { };

            return Ok(result);
        }

        [ResponseType(typeof(IEnumerable<ErrorLogModel>))]
        [AcceptVerbs("GET")]
        public IHttpActionResult Search(long? startTimestamp, long? endTimestamp)
        {
            var result = new ErrorLogModel[] { }.AsEnumerable();

            try
            {
                result = logBusiness.GetLogs(startTimestamp, endTimestamp);
            }
            catch (Exception e)
            {
                SimpleFileLogger.Instance.LogError(e);
            }

            result = result ?? new ErrorLogModel[] { }.AsEnumerable();

            return Ok(result);
        }

        [AcceptVerbs("GET")]
        [Route("Info")]
        public IHttpActionResult GetInfo()
        {
            return Ok(GetInfoArray());
        }
    }
}