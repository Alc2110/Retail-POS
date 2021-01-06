using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Model;

namespace POS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // set the program state
            Configuration.currentProgramState = ProgramState.LOGGED_OUT;

            // configure the logger
            Configuration.Logger.ConfigureLogger();

            // read database connection information from configuration file
            Configuration.connectionString = "Server=" + ConfigurationManager.AppSettings["serverAddress"] + ";Database=Retail_POS;User Id=" + ConfigurationManager.AppSettings["serverUser"] +
                                              ";Password=" + ConfigurationManager.AppSettings["serverPassword"];

            // read store name from configuration file
            Configuration.storeName = ConfigurationManager.AppSettings["storeName"];

            try
            {
                // show the login form
                LoginForm loginForm = new LoginForm();
                Application.Run(loginForm);

                Application.ExitThread();
                Environment.Exit(Environment.ExitCode);
            }
            catch (Exception ex)
            {
                // something bad happened
                // this is a last resort to catch the error
                // TODO: custom error dialog, log it
                System.Windows.Forms.MessageBox.Show("A fatal error occurred: " + ex.Message + "\nStack Trace: " + ex.StackTrace);
            }
        }
    }
}
