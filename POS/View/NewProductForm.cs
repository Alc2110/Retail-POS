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
    public partial class NewProductForm : Form
    {
        // get logger instance for this class
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        Product toAdd;

        ViewProductsForm parentWindow;

        /// <summary>
        /// Default constructor for this window.
        /// </summary>
        public NewProductForm(ViewProductsForm parentWindow)
        {
            InitializeComponent();

            logger.Info("Initialising new product form");

            this.parentWindow = parentWindow;

            // cannot add anything yet
            button_add.Enabled = false;

            // event handlers for data entry controls
            textBox_ID.TextChanged += checkEntries;
            textBox_description.TextChanged += checkEntries;
            textBox_price.TextChanged += checkEntries;
            numericUpDown_quantity.ValueChanged += checkEntries;
 
            // update status bar to "ready" status
            labelProgressBar1.setColourAndText(Configuration.ProgressBarColours.IDLE_COLOUR, "Ready");
            labelProgressBar1.Value = 100;
        }

        #region UI event handlers
        /// <summary>
        /// Close button clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_close_Click(object sender, EventArgs e)
        {
            logger.Info("Closing new product form");

            this.Close();
        }
        #endregion

        /// <summary>
        /// Create product record to be added.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEntries(object sender, EventArgs e)
        {
            if ((textBox_ID.Text!=string.Empty) && (textBox_description.Text!=string.Empty) && (textBox_price.Text!=string.Empty))
            {
                // all entries are not empty

                // update status bar
                labelProgressBar1.setColourAndText(Configuration.ProgressBarColours.IDLE_COLOUR, "Ready");
                labelProgressBar1.Value = 100;

                // gather the data
                toAdd = new Product
                {
                    productNumber = textBox_ID.Text,
                    description = textBox_description.Text,
                    quantity = (int)numericUpDown_quantity.Value,
                    price = float.Parse(textBox_price.Text)
                };

                // enable the add button
                button_add.Enabled = true;
            }
            else
            {
                // something is empty
                button_add.Enabled = false;
            }

            // update status bar to ready
            labelProgressBar1.setColourAndText(Configuration.ProgressBarColours.IDLE_COLOUR, "Ready");
            labelProgressBar1.Value = 100;
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
                labelProgressBar1.setColourAndText(Configuration.ProgressBarColours.TASK_IN_PROGRESS_COLOUR, "Adding product");
                labelProgressBar1.Value = 100;

                await Task.Run(() =>
                {
                    parentWindow.OnAddProductRequest(new AddProductRequestEventArgs(toAdd));
                });
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                // it failed
                // database error
                // tell the user and the logger
                string dbErrorMsg = "Adding new product failed. Database Error.";
                System.Windows.Forms.MessageBox.Show(dbErrorMsg, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                labelProgressBar1.setColourAndText(Configuration.ProgressBarColours.TASK_FAILED_COLOUR, dbErrorMsg);
                labelProgressBar1.Value = 100;
                logger.Error(dbErrorMsg);
                logger.Error("Stack Trace: " + ex.StackTrace);

                // nothing more we can do
                return;
            }

            // at this point, it succeeded
            string successMsg = "Successfully added new product.";
            labelProgressBar1.setColourAndText(Configuration.ProgressBarColours.TASK_SUCCEEDED_COLOUR, successMsg);
            labelProgressBar1.Value = 100;
            logger.Info(successMsg);

            // clear
            textBox_description.Text = string.Empty;
            textBox_ID.Text = string.Empty;
            textBox_price.Text = string.Empty;
        }
    }
}
