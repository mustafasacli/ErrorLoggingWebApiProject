namespace ErrorLog.Wcf.Library
{
    //class WcfIoC
    //{
    //}

    using Business.Core.Interfaces;
    using Business.MongoDb;
    using Business.RavenDb;
    using Business.Sql;
    using Business.SqlCE;
    using Business.SQLite;
    using SimpleInjector;
    using System;

    public class WcfIoC
    {
        private static object lockObj = new object();
        private Container container = null;

        private static Lazy<WcfIoC> instanceLazy = new Lazy<WcfIoC>(() =>
        {
            return new WcfIoC();
        });

        private WcfIoC()
        {
        }

        public static WcfIoC Instance
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

        private void Bootstrap()
        {
            // Create the container as usual.
            container = new Container();

            switch (AppValues.DbMode)
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

                default:
                    break;
            }

            // Optionally verify the container.
            container.Verify();
        }
    }
}