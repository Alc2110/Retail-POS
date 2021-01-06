using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS.Security;

namespace POS.View
{
    public partial class NewStaffForm : Form
    {
        // get logger instance for this class
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public NewStaffForm()
        {
            InitializeComponent();

            // log it
            logger.Info("Initialising new staff form");

            // prepare the comboBox
            comboBox_privelege.Items.Add("Admin");
            comboBox_privelege.Items.Add("Normal");

            // cannot add anything yet
            button_addStaff.Enabled = false;

            // event handlers for data entry controls
            textBox_fullName.TextChanged += checkEntries;
            textBox_password.TextChanged += checkEntries;
            textBox_repeatPassword.TextChanged += checkEntries;

            // status bar colour
            labelProgressBar1.setColourAndText(Configuration.ProgressBarColours.IDLE_COLOUR, "Ready");
            labelProgressBar1.Value = 100;
        }

        #region UI event handlers
        private void button_close_Click(object sender, EventArgs e)
        {
            logger.Info("Closing new staff form");

            this.Close();
        }

        private async void button_addStaff_Click(object sender, EventArgs e)
        {
           
                try
                {
                    // use wait cursor
                    this.UseWaitCursor = true;

                    // update the status bar
                    labelProgressBar1.setColourAndText(Configuration.ProgressBarColours.TASK_IN_PROGRESS_COLOUR, "Adding new staff record");
                    labelProgressBar1.Value = 100;

                    if (!(textBox_password.Text.Equals(textBox_repeatPassword.Text)))
                    {
                        // cannot add, passwords don't match
                        MessageBox.Show("Passwords do not match. Please try again", "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        logger.Info("Passwords do not match");

                        textBox_password.Text = null;
                        textBox_repeatPassword.Text = null;

                        return;
                    }
                    else
                    {
                        // can add
                        // hash the password, prepare the data
                        string fullName = textBox_fullName.Text;
                        string password = textBox_password.Text;
                        string privelege = comboBox_privelege.Text;
                        Hasher hasher = new Hasher(fullName, password);
                        string passwordHash = hasher.computeHash();
                        Model.ObjectModel.Staff newStaff = new Model.ObjectModel.Staff();
                        newStaff.fullName = fullName;
                        newStaff.passwordHash = passwordHash;
                        switch (privelege)
                        {
                            case "Admin":
                                newStaff.privelege = Model.ObjectModel.Privelege.Admin;
                                break;
                            case "Normal":
                                newStaff.privelege = Model.ObjectModel.Privelege.Normal;
                                break;
                            default:
                                // shouldn't happen
                                return;
                        }

                        // log it
                        logger.Info("Adding staff record: ");
                        logger.Info("Full name: " + fullName);
                        logger.Info("Password: " + password);
                        logger.Info("Hashed and salted password: " + passwordHash);
                        logger.Info("Privelege: " + privelege);

                        await Task.Run(() =>
                        {
                            
                        });
                    }
                }
                catch (Exception ex)
                {
                    // failed to add new staff
                    // tell the user and the logger
                    string errorMessage = "Error adding new staff member: " + ex.Message;
                    logger.Error(ex, errorMessage);
                    logger.Error("Stack trace: " + ex.StackTrace);
                    labelProgressBar1.setColourAndText(Configuration.ProgressBarColours.TASK_FAILED_COLOUR, "Error adding new staff");
                    labelProgressBar1.Value = 100;
                    this.UseWaitCursor = false;
                    MessageBox.Show(errorMessage, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // nothing more we can do
                    return;
                }

                // at this point, it succeeded
                // feedback for user
                string successMessage = "Successfully added new staff member record";
                logger.Info(successMessage);
                labelProgressBar1.setColourAndText(Configuration.ProgressBarColours.TASK_SUCCEEDED_COLOUR, successMessage);
                labelProgressBar1.Value = 100;
                this.UseWaitCursor = false;
                MessageBox.Show(successMessage, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // clean up UI
                textBox_fullName.Text = string.Empty;
                textBox_password.Text = string.Empty;
                textBox_repeatPassword.Text = string.Empty;
            
        }

        private void checkEntries(object sender, EventArgs e)
        {
            if ((textBox_fullName.Text!=string.Empty) && (textBox_password.Text!=string.Empty) && (textBox_repeatPassword.Text!=string.Empty))
            {
                button_addStaff.Enabled = true;
            }
            else
            {
                button_addStaff.Enabled = false;
            }

            labelProgressBar1.setColourAndText(Configuration.ProgressBarColours.IDLE_COLOUR, "Ready");
            labelProgressBar1.Value = 100;
        }
        #endregion
    }
}
