namespace ErrorLog.Models
{
    using System;
    using System.Runtime.Serialization;

    public class ErrorLogModel
    {
        [DataMember]
        public string LogId
        { get; set; }

        [DataMember]
        public string RequestAddres
        { get; set; }

        [DataMember]
        public string ResponseAddress
        { get; set; }

        [DataMember]
        public string ResponseMachineName
        { get; set; }

        [DataMember]
        public string UserId
        { get; set; }

        [DataMember]
        public string ClassName
        { get; set; }

        [DataMember]
        public string MethodName
        { get; set; }

        [DataMember]
        public string Message
        { get; set; }

        [DataMember]
        public string StackTrace
        { get; set; }

        [DataMember]
        public string ExceptionData
        { get; set; }

        [DataMember]
        public DateTime? LogTime
        { get; set; }

        [DataMember]
        public long? LogTimeUnixTimestamp
        { get; set; }

        [DataMember]
        public DateTime CreatedOn
        { get; set; }

        [DataMember]
        public long CreatedOnTimestamp
        { get; set; }
    }
}