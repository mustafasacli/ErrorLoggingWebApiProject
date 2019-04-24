using System;

namespace ErrorLog.WebApi
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An application values. </summary>
    ///
    /// <remarks>   Msacli, 24.04.2019. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public static class AppValues
    {
        //System.Diagnostics.Trace.CorrelationManager.ActivityId = Guid.NewGuid();
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
                if (gd == null || gd == Guid.Empty)
                {
                    lock (lockObj)
                    {
                        if (gd == null || gd == Guid.Empty)
                        {
                            gd = Guid.NewGuid();
                        }
                    }
                }

                return gd;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets a value indicating whether the is raven database. </summary>
        ///
        /// <value> True if ıs raven database, false if not. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static bool IsRavenDb
        {
            /// isRavenDb
            get
            {
                var isRavenDbStr = System.Configuration.ConfigurationManager.AppSettings["isRavenDb"] ?? string.Empty;
                isRavenDbStr = isRavenDbStr.Trim();
                isRavenDbStr = isRavenDbStr.Replace(' ', '\0');
                var result = isRavenDbStr == "1";
                return result;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the database mode. </summary>
        ///
        /// <value> The database mode. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static int DbMode
        {
            get
            {
                var dbMode = System.Configuration.ConfigurationManager.AppSettings["dbMode"] ?? string.Empty;
                dbMode = dbMode.Trim();
                dbMode = dbMode.Replace(' ', '\0');
                int say;
                int.TryParse(dbMode, out say);
                return say;
            }
        }
    }
}