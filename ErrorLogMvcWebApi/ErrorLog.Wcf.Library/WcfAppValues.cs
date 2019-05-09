////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	AppValues.cs
//
// summary:	Implements the application values class
////////////////////////////////////////////////////////////////////////////////////////////////////

namespace ErrorLog.Wcf.Library
{
    using System;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An application values. </summary>
    ///
    /// <remarks>   Msacli, 24.04.2019. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public static class WcfAppValues
    {
        //System.Diagnostics.Trace.CorrelationManager.ActivityId = Guid.NewGuid();
        /// <summary>   The gd. </summary>
        private static Guid gd = Guid.Empty;

        /// <summary>   The lock object. </summary>
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
    }
}