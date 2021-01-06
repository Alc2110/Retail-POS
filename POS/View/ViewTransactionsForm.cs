using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model.ServiceLayer;
using Model.ObjectModel;

namespace POS.View
{
    public partial class ViewTransactionsForm : Form
    {
        // get an instance of the logger for this class
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        // delegate for handling events fired by the model
        //private delegate void populateTransactionsViewDelegate(object sender, GetAllTransactionsEventArgs args);

        public ViewTransactionsForm()
        {
            InitializeComponent();

            logger.Info("Initialising view transactions form");

            // prepare the listView
            listView_transactions.Columns.Add("Transaction ID", 100);
            listView_transactions.Columns.Add("Timestamp", 200);
            listView_transactions.Columns.Add("Customer ID", 100);
            listView_transactions.Columns.Add("Customer full name", 200);
            listView_transactions.Columns.Add("Staff ID", 100);
            listView_transactions.Columns.Add("Staff full name", 200);
            listView_transactions.Columns.Add("Product ID", 100);
            listView_transactions.Columns.Add("Product description", 200);
            listView_transactions.Columns.Add("Product price", 100);
            listView_transactions.View = System.Windows.Forms.View.Details;
            listView_transactions.GridLines = true;
            // make the headers bold
            for (int i = 0; i < listView_transactions.Columns.Count; i++)
            {
                listView_transactions.Columns[i].ListView.Font = new Font(listView_transactions.Columns[i].ListView.Font, FontStyle.Bold);
            }

            // colour and position the busy indicator properly
            circularProgressBar1.Location = calculateBusyIndicatorPos();
            circularProgressBar1.ProgressColor = Configuration.ProgressBarColours.TASK_IN_PROGRESS_COLOUR;
            showBusyIndicator();
        }

        #region UI event handlers
        private void button_close_Click(object sender, EventArgs e)
        {
            logger.Info("Closing view transactions form");

            this.Close();
        }

        private void button_viewInvoices_Click(object sender, EventArgs e)
        {
            // TODO: invoices
        }

        private void ViewTransactionsForm_Resize(object sender, EventArgs e)
        {
            circularProgressBar1.Location = calculateBusyIndicatorPos();
        }
        #endregion
        /*
        private void transactionEventHandler(object sender, GetAllTransactionsEventArgs args)
        {
            if (listView_transactions.InvokeRequired)
            {
                listView_transactions.Invoke(new populateTransactionsViewDelegate(populateView), new object[] { sender, args });
            }
            else
            {
                showBusyIndicator();

                populateView(sender, args);
            }
        }
        
        private void populateView(object sender, GetAllTransactionsEventArgs args)
        {
            showBusyIndicator();

            // tell the listView it is being updated
            listView_transactions.BeginUpdate();

            // clear the list
            foreach (ListViewItem item in listView_transactions.Items)
            {
                listView_transactions.Items.Remove(item);
            }

            // populate it
            foreach (var transaction in args.getList())
            {
                string[] itemArr = new string[9];

                itemArr[0] = transaction.TransactionID.ToString();
                itemArr[1] = transaction.Timestamp;

                if (transaction.customer != null)
                {
                    itemArr[2] = transaction.customer.CustomerID.ToString();
                    itemArr[3] = transaction.customer.FullName;
                }

                itemArr[4] = transaction.staff.StaffID.ToString();
                itemArr[5] = transaction.staff.FullName;
                itemArr[6] = transaction.product.ProductIDNumber;
                itemArr[7] = transaction.product.Description;
                itemArr[8] = transaction.product.price.ToString();
                
                ListViewItem transactionItem = new ListViewItem(itemArr);
                transactionItem.Font = new Font(transactionItem.Font, FontStyle.Regular);
                listView_transactions.Items.Add(transactionItem);
            }

            // tell the listView it is ready
            listView_transactions.EndUpdate();

            removeBusyIndicator();
        }
        */

        private async void ViewTransactionsForm_Load(object sender, EventArgs e)
        {
            // subscribe to Model events
            //Models.transactionService.GetAllTransactions += new EventHandler<GetAllTransactionsEventArgs>(transactionEventHandler);

            // populate the list upon loading
            try
            {
                showBusyIndicator();

                // run this operation in a separate thread
                await Task.Run(() =>
                {
                    //Models.transactionService.getAllTransactions();
                });
            }
            catch (System.IO.InvalidDataException invDatEx)
            {
                // found invalid data in the database
                // tell the user and the logger
                string errorMessage = "Error: found invalid data in database: " + invDatEx.Message;
                logger.Error(invDatEx, errorMessage);
                logger.Error("Stack trace: " + invDatEx.Message);

                MessageBox.Show(errorMessage, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Error);

                removeBusyIndicator();
            }
            catch (Exception ex)
            {
                // some other error
                // tell the user and the logger
                string errorMessage = "Error: could not retrieve data from database: " + ex.Message;
                logger.Error(ex, errorMessage);
                logger.Error("Stack trace: " + ex.StackTrace);

                MessageBox.Show(errorMessage, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Error);

                removeBusyIndicator();
            }
        }

        private System.Drawing.Point calculateBusyIndicatorPos()
        {
            int xPos = ((groupBox1.Width) / 2) - ((circularProgressBar1.Width) / 2);
            int yPos = ((groupBox1.Height) / 2) - ((circularProgressBar1.Height) / 2);

            return new System.Drawing.Point(xPos, yPos);
        }

        private void removeBusyIndicator()
        {
            circularProgressBar1.Visible = false;
            listView_transactions.Visible = true;
        }

        private void showBusyIndicator()
        {
            circularProgressBar1.Visible = true;
            listView_transactions.Visible = false;
        }
    }
}
