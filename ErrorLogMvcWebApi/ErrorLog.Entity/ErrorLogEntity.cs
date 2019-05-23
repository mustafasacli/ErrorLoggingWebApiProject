namespace ErrorLog.Entity
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ErrorLogTable")]
    public class ErrorLogEntity
    {
        [Key]
        public string Id
        { get; set; }

        public string RequestAddres
        { get; set; }

        public string ResponseAddress
        { get; set; }

        public string ResponseMachineName
        { get; set; }

        public string UserId
        { get; set; }

        public string ClassName
        { get; set; }

        public string MethodName
        { get; set; }

        public string Message
        { get; set; }

        public string StackTrace
        { get; set; }

        public string ExceptionData
        { get; set; }

        public DateTime? LogTime
        { get; set; }

        public long? LogTimeUnixTimestamp
        { get; set; }

        public DateTime CreatedOn
        { get; set; }

        public long CreatedOnUnixTimestamp
        { get; set; }
    }
}