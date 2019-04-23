namespace ErrorLog.WebApi
{
    using ErrorLog.Business.Core.Interfaces;
    using ErrorLog.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;

    public class LogController : BaseApiController
    {
        private IErrorLogBusiness logBusiness;

        public LogController(IErrorLogBusiness logBusiness)
        {
            this.logBusiness = logBusiness;
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
                if (string.IsNullOrWhiteSpace(oid))
                {
                    result = logBusiness.GetById(oid);
                }
            }
            catch (Exception e)
            {
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