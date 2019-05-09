////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	ErrorLogIoC.cs
//
// summary:	Implements the error log IoC class
////////////////////////////////////////////////////////////////////////////////////////////////////

namespace ErrorLog.IoC.Library
{
    using Business.Core.Interfaces;
    using Business.LiteDb;
    using Business.MongoDb;
    using Business.RavenDb;
    using Business.Sql;
    using Business.SqlCE;
    using Business.SQLite;
    using Business.VistaDb;
    using SimpleInjector;
    using System;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An error log IoC. </summary>
    ///
    /// <remarks>   Mustafa SAÇLI, 9.05.2019. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class ErrorLogIoC
    {
        /// <summary>   The lock object. </summary>
        private static object lockObj = new object();
        /// <summary>   The container. </summary>
        private Container container = null;

        /// <summary>   The instance lazy. </summary>
        private static Lazy<ErrorLogIoC> instanceLazy = new Lazy<ErrorLogIoC>(() =>
        {
            return new ErrorLogIoC();
        });

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructor that prevents a default instance of this class from being created.
        /// </summary>
        ///
        /// <remarks>   Mustafa SAÇLI, 9.05.2019. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private ErrorLogIoC()
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the ınstance. </summary>
        ///
        /// <value> The instance. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static ErrorLogIoC Instance
        {
            get { return instanceLazy.Value; }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the document store. </summary>
        ///
        /// <value> The document store. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public Container Container
        {
            get
            {
                if (container == null)
                {
                    lock (lockObj)
                    {
                        if (container == null)
                        {
                            Bootstrap();
                        }
                    }
                }

                return container;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Bootstraps this object. </summary>
        ///
        /// <remarks>   Mustafa SAÇLI, 9.05.2019. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Bootstrap()
        {
            container = new Container();

            switch (IocAppValues.DbMode)
            {
                case 1:
                    container.Register<IErrorLogBusiness, ErrorLogMongoDbBusiness>(Lifestyle.Singleton);
                    break;

                case 2:
                    container.Register<IErrorLogBusiness, ErrorLogRavenDbBusiness>(Lifestyle.Singleton);
                    break;

                case 3:
                    container.Register<IErrorLogBusiness, ErrorLogSqlBusiness>(Lifestyle.Singleton);
                    break;

                case 4:
                    container.Register<IErrorLogBusiness, ErrorLogSqlCeBusiness>(Lifestyle.Singleton);
                    break;

                case 5:
                    container.Register<IErrorLogBusiness, ErrorLogSQLiteBusiness>(Lifestyle.Singleton);
                    break;

                case 6:
                    container.Register<IErrorLogBusiness, ErrorLogVistaDbBusiness>(Lifestyle.Singleton);
                    break;

                case 7:
                    container.Register<IErrorLogBusiness, ErrorLogLiteDbBusiness>(Lifestyle.Singleton);
                    break;

                default:
                    break;
            }

            container.Verify();
        }
    }
}