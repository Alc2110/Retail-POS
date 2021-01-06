using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;

namespace POS
{
    public partial class LoginForm : Form
    {
        string retrievedHash = null;

        // create instance of logger for this class
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        int staffID;

        public LoginForm()
        {
            logger.Info("Initialising login form");

            InitializeComponent();

            // subscribe to the form closing event
            this.FormClosing += LoginForm_FormClosing;
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            button_OK.Enabled = false;
            tryLogin();
        }

        private async void tryLogin()
        {
            // attempt connection
            using (SqlConnection conn = new SqlConnection(Configuration.connectionString))
            {
                try
                {
                    // try it
                    // tell the logger
                    logger.Info("Attempting connection with database");
                    logger.Info("Connection string: " + Configuration.connectionString);
                    await conn.OpenAsync();
                }
                catch (Exception ex)
                {
                    // it failed
                    // tell the user and the logger
                    conn.Close();
                    string errorMessage = "Error connecting to database: " + ex.Message;
                    MessageBox.Show(errorMessage, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    logger.Error(ex, errorMessage);

                    button_OK.Enabled = true;

                    return;
                }

                switch (await authenticated(conn))
                {
                    case AuthResult.SUCCESS:
                        // authentication successful
                        // tell the user and the logger
                        string authSuccessMessage = "Login successful";
                        MessageBox.Show(authSuccessMessage, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        logger.Info(authSuccessMessage);

                        // config
                        Configuration.staffID = staffID;

                        // update the program state
                        Configuration.currentProgramState = Model.ProgramState.LOGGED_IN;

                        // show main form and close this one
                        MainWindow mainForm = new MainWindow();
                        this.Hide();
                        conn.Close();
                        mainForm.ShowDialog();

                        logger.Info("Exiting application with Exit Code: " + Environment.ExitCode);

                        break;

                    case AuthResult.NO_USERNAME:
                        // no such username exists
                        // tell the user and the logger
                        string noUsernameMessage = "Username does not exist";
                        MessageBox.Show(noUsernameMessage, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        logger.Info(noUsernameMessage);
                        conn.Close();

                        button_OK.Enabled = true;

                        break;

                    case AuthResult.DENIED:
                        // authentication failed
                        // feedback for user
                        string authFailMessage = "Incorrect password";
                        MessageBox.Show(authFailMessage, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();

                        button_OK.Enabled = true;

                        break;
                }
            }
        }

        private async Task<AuthResult> authenticated(SqlConnection conn)
        {
            // salting and hashing
            Security.Hasher loginHasher = new Security.Hasher(textBox_username.Text, textBox_password.Text);
            string hash = loginHasher.computeHash();

            // check if username exists
            string userNameQuery = "DECLARE @varFullName varchar(max);" +
            " SET @varFullName = '" + textBox_username.Text + "';" +
            " SELECT * FROM Staff" +
            " WHERE FullName = @varFullName;";
            SqlCommand userNameCommand = new SqlCommand(userNameQuery, conn);
            SqlDataReader userNameDataReader = await userNameCommand.ExecuteReaderAsync();
            if (!userNameDataReader.HasRows)
            {
                // empty, no such username exists
                return AuthResult.NO_USERNAME;
            }

            // at this point, the username provided exists in the database
            // retrieve salted hash from database and compare
            // while there, might as well grab the user privelege level
            while (userNameDataReader.Read())
            {
                retrievedHash = userNameDataReader.GetString(2);
                staffID = userNameDataReader.GetInt32(0);

                string retrievedPrivelege = userNameDataReader.GetString(3);
                switch (retrievedPrivelege)
                {
                    case "Admin":
                        Configuration.userLevel = Configuration.Role.ADMIN;
                        logger.Info("Admin user");

                        break;

                    case "Normal":
                        Configuration.userLevel = Configuration.Role.NORMAL;
                        logger.Info("Normal user");

                        break;

                    default:
                        // this shouldn't happen
                        // TODO: handle it appropriately
                        break;
                }
            }

            if (retrievedHash.Equals(hash))
            {
                // success
                // grab the staff ID
                Configuration.staffID = staffID;
                
                return AuthResult.SUCCESS;
            }

            // access denied
            return AuthResult.DENIED;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            logger.Info("Exiting application with Exit Code: " + Environment.ExitCode);
            Application.ExitThread();
            Environment.Exit(Environment.ExitCode);
        }

        // login form is closed
        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.ExitThread();
            Environment.Exit(Environment.ExitCode);
        }
    }

    enum AuthResult
    {
        SUCCESS,
        NO_USERNAME,
        DENIED
    }
}
