using NHibernate;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Automapping;
using NHibernate.Cfg;
using System;
using System.Security.Permissions;
using System.Security;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace BatchMKV.Repositories
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;
        private static FluentConfiguration _fluentConfiguration;

        private static readonly string dbPassword = "dh387Gh43348skjl3HSN_sk";

        private static string dbFolder = null;
        public static string DatabaseFolder
        {
            get
            {
                if (dbFolder == null)
                    dbFolder = String.Format("{0}\\BatchMKV", System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData)); 
                return dbFolder;
            }
        }

        private static string dbFilename = null;
        public static string DatabaseFilename
        {
            get
            {
                if (dbFilename == null)
                    dbFilename = string.Format("{0}\\store.bmkv", DatabaseFolder);
                return dbFilename;
            }
        }

        public static ISessionFactory SessionFactory
        {
            get
            {
                // Configure
                if (_fluentConfiguration == null)
                {
                    string dbConnectionString = string.Format("Data Source={0};Version=3;{1}", DatabaseFilename, (!String.IsNullOrWhiteSpace(dbPassword) ? String.Format("Password={0};", dbPassword) : ""));

                    _fluentConfiguration = Fluently.Configure()
                        .Database(SQLiteConfiguration.Standard.ConnectionString(dbConnectionString))
                        .Mappings(m =>
                            m.FluentMappings.AddFromAssemblyOf<Domain.SourceState>());
                }

                try
                {
                    // Check if database exists, create it if it does not.
                    if (!System.IO.File.Exists(DatabaseFilename))
                    {
                        // Create folder.
                        System.IO.DirectoryInfo folder = System.IO.Directory.CreateDirectory(DatabaseFolder);
                        if (!folder.Exists) return null;

                        _fluentConfiguration = _fluentConfiguration.ExposeConfiguration(BuildSchema);
                    }

                    // Build session factory
                    if (_sessionFactory == null)
                        _sessionFactory = _fluentConfiguration.BuildSessionFactory();
                }
                catch
                {
                    return null;
                }

                return _sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            try { return (SessionFactory != null ? SessionFactory.OpenSession() : null); }
            catch { return null; }
        }

        private static void BuildSchema(Configuration config)
        {
            try { new SchemaExport(config).Create(false, true); }
            catch { }
        }

        private static void UpdateSchema(Configuration config)
        {
            new SchemaUpdate(config).Execute(false, true);
        }
    }
}