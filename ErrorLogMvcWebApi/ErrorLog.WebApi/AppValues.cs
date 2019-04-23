using System;

namespace ErrorLog.WebApi
{
    public static class AppValues
    {
        //System.Diagnostics.Trace.CorrelationManager.ActivityId = Guid.NewGuid();
        private static Guid gd = Guid.Empty;

        private static object lockObj = new object();

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
