using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model.ObjectModel;

namespace POS.View
{
    public partial class NewCustomerForm : Form
    {
        // get logger instance for this class
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        Customer toAdd;

        ViewCustomersForm parentWindow;

        /// <summary>
        /// Default constructor for this window.
        /// </summary>
        /// <param name="parentWindow"></param>
        public NewCustomerForm(ViewCustomersForm parentWindow)
        {
            InitializeComponent();

            logger.Info("Initialising new customer form");

            this.parentWindow = parentWindow;

            // prepare the comboBox
            foreach (var val in States.GetValues(typeof(States)))
            {
                string state = val.ToString();
                if (!state.Equals("Default"))
                    comboBox_state.Items.Add(state);
            }

            // cannot add anything yet
            button_add.Enabled = false;

            // subscribe to textbox interaction events
            textBox_city.TextChanged += checkEntries;
            textBox_email.TextChanged += checkEntries;
            textBox_fullName.TextChanged += checkEntries;
            textBox_phoneNumber.TextChanged += checkEntries;
            textBox_postcode.TextChanged += checkEntries;
            textBox_streetAddress.TextChanged += checkEntries;

            // status bar
            labelProgressBar1.Value = 100;
            labelProgressBar1.setColourAndText(Configuration.ProgressBarColours.IDLE_COLOUR, "Ready");
        }

        #region UI event handlers
        /// <summary>
        /// Close button clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_cancel_Click(object sender, EventArgs e)
        {
            logger.Info("Closing new customer form");

            this.Close();
        }

        /// <summary>
        /// Add button clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_add_Click(object sender, EventArgs e)
        {
            try
            {
                toAdd = new Customer
                {
                    fullName = textBox_fullName.Text,
                    address = textBox_streetAddress.Text,
                    city = textBox_city.Text,
                    email = textBox_email.Text,
                    phoneNumber = textBox_phoneNumber.Text,
                    state = parseEnum<States>(comboBox_state.Text),
                    Postcode = int.Parse(textBox_postcode.Text)
                };

                labelProgressBar1.setColourAndText(Configuration.ProgressBarColours.TASK_IN_PROGRESS_COLOUR, "Adding customer");
                labelProgressBar1.Value = 100;

                await Task.Run(() =>
                {
                    parentWindow.OnAddCustomerRequest(new AddCustomerRequestEventArgs(toAdd));
                });
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                // it failed
                // database error
                // tell the user and the logger
                string dbErrorMsg = "Adding new customer failed. Database Error.";
                System.Windows.Forms.MessageBox.Show(dbErrorMsg, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                labelProgressBar1.setColourAndText(Configuration.ProgressBarColours.TASK_FAILED_COLOUR, dbErrorMsg);
                labelProgressBar1.Value = 100;
                logger.Error(dbErrorMsg);
                logger.Error("Stack Trace: " + ex.StackTrace);

                // nothing more we can do
                return;
            }

            // at this point, it succeeded
            string successMsg = "Successfully added new customer.";
            labelProgressBar1.setColourAndText(Configuration.ProgressBarColours.TASK_SUCCEEDED_COLOUR, successMsg);
            labelProgressBar1.Value = 100;
            logger.Info(successMsg);

            // clear
            textBox_city.Text = string.Empty;
            textBox_email.Text = string.Empty;
            textBox_fullName.Text = string.Empty;
            textBox_phoneNumber.Text = string.Empty;
            textBox_postcode.Text = string.Empty;
            textBox_streetAddress.Text = string.Empty;
        }
        #endregion

        public T parseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        private void checkEntries(object sender, EventArgs e)
        {
            if ((textBox_fullName.Text!=string.Empty) && 
                (textBox_streetAddress.Text!=string.Empty) && 
                (textBox_phoneNumber.Text!=string.Empty) && 
                (textBox_email.Text!=string.Empty) && 
                (textBox_email.Text!=string.Empty) && (
                textBox_city.Text!=string.Empty) && 
                (textBox_postcode.Text!=string.Empty))
            {
                button_add.Enabled = true;
            }
            else
            {
                button_add.Enabled = false;
            }

            labelProgressBar1.setColourAndText(Configuration.ProgressBarColours.IDLE_COLOUR, "Ready");
            labelProgressBar1.Value = 100;
        }
    }
}
