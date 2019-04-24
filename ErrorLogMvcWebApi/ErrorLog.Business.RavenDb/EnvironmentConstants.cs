namespace ErrorLog.Business.RavenDb
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An environment constants. </summary>
    ///
    /// <remarks>   Msacli, 24.04.2019. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public static class EnvironmentConstants
    {
        /// <summary>
        /// URL of the database server.
        /// </summary>
        public const string DbServerUrl = "http://127.0.0.1:8081/";

        /// <summary>
        /// Name of the database.
        /// </summary>
        public const string DbName = "ErrorLogSampleRavenDb";
    }
}