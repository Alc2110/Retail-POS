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
    public partial class ViewCustomersForm : Form, ICustomersView
    {
        // get an instance of the logger for this class
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private delegate void populateCustomersDelegate(IEnumerable<ICustomer> customers);

        /// <summary>
        /// Fill the list.
        /// </summary>
        /// <param name="customers"></param>
        public void populateCustomers(IEnumerable<ICustomer> customers)
        {
            if (listView_customers.InvokeRequired)
            {
                showBusyIndicator();

                var d = new populateCustomersDelegate(populateCustomers);

                listView_customers.Invoke(d, customers);

                removeBusyIndicator();
            }
            else
            {
                showBusyIndicator();

                _populateCustomers(customers);

                removeBusyIndicator();
            }
        }

        private void _populateCustomers(IEnumerable<ICustomer> customers)
        {
            // tell the list it is being updated
            listView_customers.BeginUpdate();

            // clear the list
            listView_customers.Items.Clear();

            // add items to the list
            foreach (var c in customers)
            {
                ListViewItem newItem = new ListViewItem(new string[] { c.customerId.ToString(),
                    c.fullName, c.address, c.phoneNumber, c.email, c.city, c.state.ToString(), c.Postcode.ToString()});
                newItem.Font = new Font(listView_customers.Font, FontStyle.Regular); // remove the bold font styling that was applied when setting the headers bold
                listView_customers.Items.Add(newItem);
            }

            // tell the list it is ready
            listView_customers.EndUpdate();
        }

        #region events
        public event EventHandler populateCustomersRequest;

        protected virtual void OnPopulateRequest(EventArgs args)
        {
            EventHandler temp = populateCustomersRequest;
            if (temp != null)
                populateCustomersRequest?.Invoke(this, args);
        }

        public event EventHandler<AddCustomerRequestEventArgs> addCustomerRequest;

        public virtual void OnAddCustomerRequest(AddCustomerRequestEventArgs args)
        {
            EventHandler<AddCustomerRequestEventArgs> temp = addCustomerRequest;
            if (temp != null)
                addCustomerRequest?.Invoke(this, args);
        }

        public event EventHandler<DeleteCustomerRequestEventArgs> deleteCustomerRequest;

        protected virtual void OnDeleteRequest(DeleteCustomerRequestEventArgs args)
        {
            EventHandler<DeleteCustomerRequestEventArgs> temp = deleteCustomerRequest;
            if (temp != null)
                deleteCustomerRequest?.Invoke(this, args);
        }
        #endregion

        /// <summary>
        /// Constructor for this window.
        /// </summary>
        public ViewCustomersForm()
        {
            InitializeComponent();

            logger.Info("Initialising view customers form");

            // prepare the listView
            listView_customers.Columns.Add("ID", 50);
            listView_customers.Columns.Add("Full name", 120);
            listView_customers.Columns.Add("Street address", 150);
            listView_customers.Columns.Add("Phone number", 100);
            listView_customers.Columns.Add("Email", 150);
            listView_customers.Columns.Add("City", 130);
            listView_customers.Columns.Add("State", 100);
            listView_customers.Columns.Add("Postcode", 75);
            listView_customers.View = System.Windows.Forms.View.Details;
            listView_customers.GridLines = true;
            // make the headers bold
            for (int i = 0; i < listView_customers.Columns.Count; i++)
            {
                listView_customers.Columns[i].ListView.Font = new Font(listView_customers.Columns[i].ListView.Font, FontStyle.Bold);
            }

            // colour and position the busy indicator properly
            circularProgressBar1.Location = calculateBusyIndicatorPos();
            circularProgressBar1.ProgressColor = Configuration.ProgressBarColours.TASK_IN_PROGRESS_COLOUR;
          
            // can't delete anything until something is selected
            button_deleteSelectedCustomer.Enabled = false;
        }

        #region UI event handlers
        /// <summary>
        /// Close button clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_close_Click(object sender, EventArgs e)
        {
            logger.Info("Closing new customers form");

            this.Close();
        }

        /// <summary>
        /// Delete button clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_deleteSelectedCustomer_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Add button clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_addNewCustomer_Click(object sender, EventArgs e)
        {
            NewCustomerForm newCustomerForm = new NewCustomerForm(this);
            newCustomerForm.Show();
        }

        /// <summary>
        /// Window loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewCustomersForm_Load(object sender, EventArgs e)
        {
            // populate the list upon loading
            OnPopulateRequest(null);
        }

        /// <summary>
        /// Selected list item changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_customers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView_customers.SelectedItems.Count>1)
            {
                // only one selection allowed at a time
                foreach (ListViewItem item in listView_customers.SelectedItems)
                {
                    item.Selected = false;
                }
            }

            if (listView_customers.SelectedItems.Count==1)
            {
                //button_deleteSelectedCustomer.Enabled = true;
            }
            else
            {
                button_deleteSelectedCustomer.Enabled = false;
            }
        }

        /// <summary>
        /// Window resized.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewCustomersForm_Resize(object sender, EventArgs e)
        {
            // reposition the circular progress bar
            circularProgressBar1.Location = calculateBusyIndicatorPos();
        }
        #endregion

        private System.Drawing.Point calculateBusyIndicatorPos()
        {
            int xPos = ((groupBox1.Width)/2)-((circularProgressBar1.Width)/2);
            int yPos = ((groupBox1.Height) / 2) - ((circularProgressBar1.Height) / 2);

            return new System.Drawing.Point(xPos, yPos);
        } 

        private void removeBusyIndicator()
        {
            circularProgressBar1.Visible = false;
            listView_customers.Visible = true;
        }

        private void showBusyIndicator()
        {
            circularProgressBar1.Visible = true;
            listView_customers.Visible = false;
        }
    }
}
