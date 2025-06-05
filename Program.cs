using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Deployment.Application;
using System.Security.Principal;
using EventLogger;

namespace BatchMKV
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Environment.ExitCode = 0;

            // Check if event sources should be created
            if (args != null && args.Length > 0)
            {
                foreach(string arg in args)
                {
                    if (arg.ToLower() == "/createeventsources")
                    {
                        // Check if we are running as administrator.
                        var wi = WindowsIdentity.GetCurrent();
                        var wp = new WindowsPrincipal(wi);

                        if (!wp.IsInRole(WindowsBuiltInRole.Administrator))
                        {
                            Environment.ExitCode = -1;
                            Application.Exit();
                            return;
                        }

                        IEventLogger eventLogger = new WindowsEventLogger();
                        eventLogger.LogName = "BATCHMKV";
                        eventLogger.SourcePrefix = "BatchMKV";
                        string[] sources = { "Main", "MakeMKV", "MKVToolNix" };
                        foreach(string source in sources)
                        {
                            if (!eventLogger.CreateEventSource(source))
                            {
                                Environment.ExitCode = -2;
                                Application.Exit();
                                return;
                            }
                        }

                        // Logs were created successfully.
                        Environment.ExitCode = 1;
                        Application.Exit();
                        return;
                    }
                }
            }

            // Check that database is OK
            bool databaseOK = false;
            try
            {
                using (NHibernate.ISession session = Repositories.NHibernateHelper.OpenSession())
                    databaseOK = (session != null);
            }
            catch
            { databaseOK = false; }

            if (!databaseOK)
            {
                MessageBox.Show(String.Format("Could not create or open database to store source information and settings at:\r\n\r\n{0}\r\n\r\nThe application will now exit.", Repositories.NHibernateHelper.DatabaseFilename), "Unable to create or open database", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new fMain());
        }

    }
}
