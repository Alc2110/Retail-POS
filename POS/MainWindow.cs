using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS.View;
using Model.ServiceLayer;
using Model.ObjectModel;
using POS.Controller;
using System.Diagnostics;
using System.Reflection;
using System.Globalization;
using System.IO;

namespace POS
{
    public partial class MainWindow : Form
    {
        // controllers
        Controller.ProductsController productsController = new ProductsController();
        Controller.CustomerController customerController = new CustomerController();
        Controller.StaffController staffController = new StaffController();
        Controller.PurchasesController purchasesController = new PurchasesController();

        public enum State
        {
            READY,
            SALE_MEMBER,
            SALE_NON_MEMBER,
        }

        public State currentState;

        // create an instance of the logger for this class
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Default constructor for this window.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // logging
            logger.Info("Initialising main window");

            // customer state combo box
            foreach (var val in States.GetValues(typeof(States)))
            {
                string state = val.ToString();
                if (!state.Equals("Default"))
                    comboBox_customerState.Items.Add(state);
            }

            // TODO: implement this
            menuItem_scripting.Enabled = false;

            listView_sales.GridLines = true;

            reset();

            // set title
            this.Text = "Retail POS v" + Configuration.VERSION + " - " + Configuration.storeName;
        }

        private void reset()
        {
            logger.Info("Resetting the main window");

            currentState = State.READY;
            logger.Info("Current state: " + currentState.ToString());

            // user priveleges
            logger.Info("Current user level: " + Configuration.userLevel.ToString());
            switch (Configuration.userLevel)
            {
                case Configuration.Role.ADMIN:
                    databaseToolStripMenuItem.Enabled = true;
                    addNewStaffToolStripMenuItem.Enabled = true;

                    button_Discount.Enabled = true;
                    //button_newItem.Enabled = true;

                    break;

                case Configuration.Role.NORMAL:
                    databaseToolStripMenuItem.Enabled = false;
                    addNewStaffToolStripMenuItem.Enabled = false;

                    button_Discount.Enabled = false;
                    //button_newItem.Enabled = false;

                    break;

                default:
                    break;
            }

            // regardless of user priveleges, some UI functions are not yet needed
            textBox_customerAddress.Enabled = false;
            textBox_customerCity.Enabled = false;
            textBox_customerEmail.Enabled = false;
            textBox_customerName.Enabled = false;
            textBox_customerPhone.Enabled = false;
            textBox_customerPostCode.Enabled = false;
            comboBox_customerState.Enabled = false;
            textBox_customerAccNo.Enabled = false;

            textBox_customerAccNo.Text = null;
            textBox_customerAddress.Text = null;
            textBox_customerCity.Text = null;
            textBox_customerEmail.Text = null;
            textBox_customerName.Text = null;
            textBox_customerPhone.Text = null;
            textBox_customerPostCode.Text = null;
            textBox_itemProductID.Text = null;
            textBox_itemQuantity.Text = null;

            button_checkout.Enabled = false;
            button_Discount.Enabled = false;
            button_clearSale.Enabled = false;
            button_addItem.Enabled = false;
            button_removeItem.Enabled = false;
            button_priceLookup.Enabled = false;
            button_findCustomer.Enabled = false;

            if (Configuration.userLevel == Configuration.Role.ADMIN)
            {
                
            }
            else
            {
                
            }

            button_newSaleMember.Enabled = true;
            button_newSaleNonMember.Enabled = true;

            textBox_itemProductID.Enabled = true;
            textBox_itemQuantity.Enabled = false;

            double priceDisplayed = 0.00;
            string sPriceDisplayed = priceDisplayed.ToString("C2", CultureInfo.GetCultureInfo("en-AU"));
            richTextBox_itemPrice.Text = sPriceDisplayed;
            //richTextBox_itemPrice.Text = "0.00";

            menuItem_scripting.Enabled = false;

            foreach (ListViewItem listItem in listView_sales.Items)
            {
                listView_sales.Items.Remove(listItem);
            }

            switch (Configuration.userLevel)
            {
                case Configuration.Role.ADMIN:
                    toolStripStatusLabel_accType.Text = "Admin";
                    break;
                case Configuration.Role.NORMAL:
                    toolStripStatusLabel_accType.Text = "Normal";
                    break;
                default:
                    // shouldn't happen
                    throw new ApplicationException("Unknown user access level");
            }
            
            toolStripStatusLabel_state.Text = "Ready";
        }

        #region UI Event Handlers
        /// <summary>
        /// Data -> Export -> Purchases menu item clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void menuItem_exportTransactions_Click(object sender, EventArgs e)
        {
            // show the save file dialog
            // allow the options for .xlsx and .csv
            SaveFileDialog exportDialog = new SaveFileDialog();
            exportDialog.Filter = "Excel spreadsheet (*.xlsx)|*.xlsx|Comma-separated values file (*.csv)|*.csv";

            if (exportDialog.ShowDialog() == DialogResult.OK)
            {
                string fullPath = exportDialog.FileName;
                string dir = Path.GetDirectoryName(fullPath);
                string fileName = Path.GetFileNameWithoutExtension(fullPath);
                string fileExt = Path.GetExtension(fullPath);

                if (fileExt.Equals(".xlsx"))
                {
                    try
                    {
                        await Task.Run(() =>
                        {
                            ExcelWriter purchasesExportExcel = new ExcelWriter(dir, fileName, "Purchases", "Purchases");

                            // write headers and metadata to the spreadsheet
                            purchasesExportExcel.writeHeadersAndMetadata(Configuration.staffID, staffController.service.read(Configuration.staffID).fullName,
                                new string[] { "Purchase ID", "Timestamp", "Product Number", "Product Description", "Product Price", "Staff ID", "Salesperson", "Customer ID", "Customer" }, null);

                            // write data records to the spreadsheet
                            foreach (var purchaseRecord in purchasesController.service.readAll())
                            {
                                if (purchaseRecord.customer != null)
                                {
                                    purchasesExportExcel.writeRecord(new string[] { purchaseRecord.purchaseId.ToString(),
                                        purchaseRecord.timestamp, purchaseRecord.product.productNumber, purchaseRecord.product.description, purchaseRecord.product.price.ToString(),
                                        purchaseRecord.staff.staffID.ToString(), purchaseRecord.staff.fullName,
                                        purchaseRecord.customer.customerId.ToString(), purchaseRecord.customer.fullName});
                                }
                                else
                                {
                                    purchasesExportExcel.writeRecord(new string[] { purchaseRecord.purchaseId.ToString(),
                                        purchaseRecord.timestamp, purchaseRecord.product.productNumber, purchaseRecord.product.description, purchaseRecord.product.price.ToString(),
                                        purchaseRecord.staff.staffID.ToString(), purchaseRecord.staff.fullName,
                                        "", ""});
                                }
                            }

                            // save the file
                            purchasesExportExcel.close();
                        });
                    }
                    catch (System.Data.SqlClient.SqlException sqlEx)
                    {
                        // database error
                        // tell the user and the logger
                        string dbErrorMsg = "Failed to export data. Database error.";
                        System.Windows.Forms.MessageBox.Show(dbErrorMsg, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        logger.Error(dbErrorMsg);
                        logger.Error("Stack Trace: " + sqlEx.StackTrace);

                        // nothing more we can do
                        return;
                    }
                    catch (System.IO.IOException ioEx)
                    {
                        // IO error
                        string ioErrorMsg = "Failed to export data. Error saving file.";
                        System.Windows.Forms.MessageBox.Show(ioErrorMsg, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        logger.Error(ioErrorMsg);
                        logger.Error("Stack Trace: " + ioEx.StackTrace);

                        // nothing more we can do
                        return;
                    }
                }
                else if (fileExt.Equals(".csv"))
                {

                }

                // at this point, it succeeded
                // tell the user and the logger.
                string exportSuccessMsg = "Data export successful.";
                System.Windows.Forms.MessageBox.Show(exportSuccessMsg, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                logger.Info(exportSuccessMsg);
            }
        }

        /// <summary>
        /// Logout button clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
            logout();
        }
       
        /// <summary>
        /// Add item button clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_addItem_Click(object sender, EventArgs e)
        {
            // retrieve product record from database, for this ID 
            string productNumber = textBox_itemProductID.Text;
            Product retrieved = null;
            try
            {
                // run this as a Task
               await Task.Run(() =>
               {
                   retrieved = (Product)productsController.service.read(productNumber);
               });
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                // something went wrong
                // tell the user and the logger
                string retrieveProductErrorMessage = "Failed to retrieve product information. Database Error";
                System.Windows.Forms.MessageBox.Show(retrieveProductErrorMessage, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Error(ex, retrieveProductErrorMessage);
                logger.Error("Stack trace: " + ex.StackTrace);

                // nothing more we can do
                return;
            }

            logger.Info("Retrieving product from database, for product ID: " + productNumber);

            // could not find product
            if (retrieved == null)
            {
                // tell the user and the logger
                string nullProductMessage = "Could not find specified product";
                MessageBox.Show(nullProductMessage, "Retail POS", 
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                logger.Warn(nullProductMessage);

                // reset the product id text box
                textBox_itemProductID.Text = string.Empty;

                // nothing more to do
                return;
            }

            // check if item already in list
            bool itemInList = false;
            foreach (ListViewItem listItem in listView_sales.Items)
            {
                if (listItem.SubItems[0].Text.Equals(productNumber))
                {
                    // item already exists
                    itemInList = true;
                    logger.Info("Item of this type is already in list (cart)");

                    // calculate cost of new items being added
                    float productPrice = retrieved.price;
                    string sNumber = textBox_itemQuantity.Text;
                    int iNumber = Int32.Parse(sNumber);

                    // add this cost to the total cost
                    string sOldCost = listItem.SubItems[4].Text;
                    float iOldCost = float.Parse(sOldCost);
                    float iNewCost = iOldCost + (productPrice * iNumber);
                    listItem.SubItems[4].Text = iNewCost.ToString();

                    // update number of items
                    string sQuantity = listItem.SubItems[2].Text;
                    int iQuantity = Int32.Parse(sQuantity);
                    int iNewQuantity = iQuantity + iNumber;
                    listItem.SubItems[2].Text = iNewQuantity.ToString();

                    deselectAllItems();
                    //listItem.Selected = true;

                    // display total price

                    break;
                }
            }

            if (!itemInList)
            {
                // item not yet in list
                logger.Info("Item of this type not yet in list. Adding item to list");

                // add it to the list
                string[] itemArr = new string[5];
                itemArr[0] = retrieved.productNumber;
                itemArr[1] = retrieved.description;
                //itemArr[2] = "1";// quantity
                string sQuantity = textBox_itemQuantity.Text;
                int iQuantity = Int32.Parse(sQuantity);
                //itemArr[2] = textBox_itemQuantity.Text;
                itemArr[2] = sQuantity;
                //itemArr[4] = retrievedProduct.getPrice().ToString();// total
                itemArr[4] = (iQuantity * retrieved.price).ToString();
                itemArr[3] = retrieved.price.ToString();
                ListViewItem item = new ListViewItem(itemArr);
                //item.Font = new Font(item.Font, FontStyle.Regular);
                item.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
                listView_sales.Items.Add(item);

                // select this item in the list
                deselectAllItems();
                //item.Selected = true;

                // display total price

                // item not yet in list
                // item should now be 1
                textBox_itemQuantity.Text = "1";
            }
            else
            {
                // increment item count
                string sItemCount = textBox_itemQuantity.Text;
                int iItemCount = Int32.Parse(sItemCount);
                textBox_itemQuantity.Text = (iItemCount + 1).ToString();
            }

            // checkout button
            if (listView_sales.Items.Count>0)
            {
                button_checkout.Enabled = true;
            }
            else
            {
                button_checkout.Enabled = false;
            }

            // clean up UI
            textBox_itemProductID.Select();
            button_addItem.Enabled = false;
            button_removeItem.Enabled = false;
            textBox_itemProductID.Text = string.Empty;
            textBox_itemQuantity.Text = string.Empty;

            deselectAllItems();

            displayTotal();
        }

        /// <summary>
        /// Main window loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Load(object sender, EventArgs e)
        {
            // prepare the listView
            // make the headers bold
            listView_sales.Font = new Font("Microsoft Sans Serif", 12);
            for (int i = 0; i < listView_sales.Columns.Count; i++)
            {
                listView_sales.Columns[i].ListView.Font = new Font(listView_sales.Columns[i].ListView.Font, FontStyle.Bold);
            }
        }

        /// <summary>
        /// New sale button clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_newSaleNonMember_Click(object sender, EventArgs e)
        {
            logger.Info("Initiating nom-member sale");

            button_newSaleMember.Enabled = false;
            button_newSaleNonMember.Enabled = false;

            button_clearSale.Enabled = true;

            textBox_itemProductID.Enabled = true;

            currentState = State.SALE_NON_MEMBER;
            toolStripStatusLabel_state.Text = "Non-member sale";
        }

        /// <summary>
        /// Clear sale button clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_clearSale_Click(object sender, EventArgs e)
        {
            if (listView_sales.Items.Count==0)
            {
                // no items
                reset();
            }
            else
            {
                // items exist, ask user for confirmation
                DialogResult dialogResult = MessageBox.Show("A sale is taking place. Do you really want to clear it?",
                                                            "Retail POS", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (dialogResult==DialogResult.Yes)
                {
                    logger.Info("Clearing sale");

                    reset();
                }
                else if (dialogResult==DialogResult.No)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Main window closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((currentState == State.SALE_MEMBER) || (currentState == State.SALE_NON_MEMBER))
            {
                var result = MessageBox.Show("A sale is taking place. Do you really want to close the application?",
                                                "Retail POS", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

                if (result != System.Windows.Forms.DialogResult.Yes)
                    e.Cancel = true;
            }

            if (!(Configuration.currentProgramState == Model.ProgramState.LOGGED_OUT))
            {
                Application.Exit();
            }
        }

        /// <summary>
        /// Show customer list window button clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_customerList_Click(object sender, EventArgs e)
        {
            View.ViewCustomersForm customersListForm = new ViewCustomersForm();
            customerController.addObserver(customersListForm);
            customersListForm.Show();
        }

        /// <summary>
        /// View -> History menu item clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            View.ViewTransactionsForm transactionsListForm = new ViewTransactionsForm();
            transactionsListForm.Show();
        }
        
        /// <summary>
        /// Add new customer button clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_addCustomer_Click(object sender, EventArgs e)
        {
            //View.NewCustomerForm newCustomerForm = new View.NewCustomerForm(this);
            //newCustomerForm.Show();
        }

        private void addNewCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //View.NewCustomerForm newCustomerForm = new View.NewCustomerForm(this);
            //newCustomerForm.Show();
        }
        
        private void addNewStaffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            View.NewStaffForm newStaffForm = new View.NewStaffForm();
            newStaffForm.Show();
        }

        private void button_staffList_Click(object sender, EventArgs e)
        {
            View.ViewStaffForm viewStaffForm = new View.ViewStaffForm();
            viewStaffForm.Show();
        }

        /// <summary>
        /// Product list button clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_findItem_Click(object sender, EventArgs e)
        {
            View.ViewProductsForm viewProductsForm = new ViewProductsForm();
            productsController.addObserver(viewProductsForm);
            viewProductsForm.Show();
        }

        private void button_newItem_Click(object sender, EventArgs e)
        {
            /*
            View.NewProductForm newProductForm = new View.NewProductForm();
            newProductForm.Show();
            */
        }

        /// <summary>
        /// Load customer button clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_findCustomer_Click(object sender, EventArgs e)
        {
            // get customer data
            // prepare the data
            //string customerAccNumber = textBox_customerAccNo.Text;
            int customerAccNumber = (Int32.Parse(textBox_customerAccNo.Text));
            logger.Info("Attempting to find customer with account number: " + customerAccNumber);

            Customer retrievedCustomer = null;
            try
            {
                // run this operation on a separate thread
                await Task.Run(() =>
                {
                    retrievedCustomer = (Customer)customerController.service.read(customerAccNumber);
                });
            }
            catch (Exception ex)
            {
                // something went wrong 
                // tell the user and the logger
                string errorRetrievingCustomerMessage = "Failed to retrieve customer: " + ex.Message;
                System.Windows.Forms.MessageBox.Show(errorRetrievingCustomerMessage, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Error(ex, errorRetrievingCustomerMessage);
                logger.Error("Stack trace: " + ex.StackTrace);

                // nothing more we can do
                return;
            }

            // could not find customer
            if (retrievedCustomer==null)
            {
                // tell the user and the logger
                string nullCustomerMessage = "Could not find specified customer";
                MessageBox.Show(nullCustomerMessage, "Retail POS",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                logger.Warn(nullCustomerMessage);

                // nothing more we can do
                return;
            }

            // found the customer
            // load the data into the window
            logger.Info("Found customer record");
            button_findCustomer.Enabled = false;
            textBox_customerName.Text = retrievedCustomer.fullName;
            textBox_customerPhone.Text = retrievedCustomer.phoneNumber;
            textBox_customerEmail.Text = retrievedCustomer.email;
            textBox_customerAddress.Text = retrievedCustomer.address;
            textBox_customerCity.Text = retrievedCustomer.city;
            textBox_customerPostCode.Text = retrievedCustomer.Postcode.ToString();
            Model.ObjectModel.States retrievedCustomerState = retrievedCustomer.state;
            string sRetrievedCustomerState = retrievedCustomerState.ToString();
            int comboBoxIndex = comboBox_customerState.FindStringExact(sRetrievedCustomerState);
            if (comboBoxIndex!=-1)
            {
                comboBox_customerState.SelectedIndex = comboBoxIndex;
            }
            else
            {
                // shouldn't happen
                reset();
            }

            textBox_itemProductID.Enabled = true;

            textBox_customerAccNo.Enabled = false;
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logout();
        }
        #endregion

        /// <summary>
        /// Unselect all product records in the list.
        /// </summary>
        public void deselectAllItems()
        {
            foreach (ListViewItem selectedItem in listView_sales.SelectedItems)
            {
                selectedItem.Selected = false;
            }
        }

        /// <summary>
        /// Log out from the current session.
        /// </summary>
        public void logout()
        {
            if ((currentState == State.SALE_NON_MEMBER) || (currentState == State.SALE_MEMBER))
            {
                DialogResult logoutConfirmation = MessageBox.Show("A sale is taking place. Do you really want to log out?", "Retail POS", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (logoutConfirmation == DialogResult.Yes)
                {
                    Configuration.currentProgramState = Model.ProgramState.LOGGED_OUT;

                    logger.Info("Logging out");
                    LoginForm loginForm = new LoginForm();
                    loginForm.Show();
                    this.Close();
                }
                else if (logoutConfirmation == DialogResult.No)
                {
                    return;
                }
            }
            else
            {
                Configuration.currentProgramState = Model.ProgramState.LOGGED_OUT;

                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                this.Close();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_newSaleMember_Click(object sender, EventArgs e)
        {
            currentState = State.SALE_MEMBER;
            toolStripStatusLabel_state.Text = "Member Sale";
            logger.Info("Initiating member sale");

            button_clearSale.Enabled = true;

            // get customer data
            textBox_itemProductID.Enabled = false;
            textBox_customerAccNo.Enabled = true;
            button_findCustomer.Enabled = true;
        }

        private void textBox_itemQuantity_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_itemProductID_TextChanged(object sender, EventArgs e)
        {
            if (textBox_itemProductID.Text!=string.Empty)
            {
                if (currentState == State.SALE_MEMBER || currentState == State.SALE_NON_MEMBER)
                {
                    // enable "Add Item" button and item quantity textbox
                    button_addItem.Enabled = true;
                    textBox_itemQuantity.Enabled = true;

                    textBox_itemQuantity.Text = "1";
                }

                button_priceLookup.Enabled = true;
            }
            else
            {
                button_addItem.Enabled = false;
                button_priceLookup.Enabled = false;

                textBox_itemQuantity.Text = string.Empty;
            }
        }

        private void listView_sales_SelectedIndexChanged(object sender, EventArgs e)
        {
            int numberSelectedItems = listView_sales.SelectedItems.Count;

            switch (numberSelectedItems)
            {
                case 0:
                    // display total cost of items in cart
                    displayTotal();

                    button_removeItem.Enabled = false;

                    button_Discount.Enabled = false;

                    textBox_itemQuantity.ReadOnly = false;
                    textBox_itemQuantity.Text = string.Empty;

                    break;

                case 1:
                    // display price
                    richTextBox_itemPrice.Text = listView_sales.SelectedItems[0].SubItems[4].Text;

                    button_removeItem.Enabled = true;

                    // only Admins can approve discounts
                    if (Configuration.userLevel == Configuration.Role.ADMIN)
                    {
                        button_Discount.Enabled = true;
                    }
                    else
                    {
                        button_Discount.Enabled = false;
                    }

                    // cannot add items
                    button_addItem.Enabled = false;
                    textBox_itemProductID.Text = string.Empty;
                    // display quantity in quantity textbox, which is readonly
                    textBox_itemQuantity.ReadOnly = true;
                    textBox_itemQuantity.Text = listView_sales.SelectedItems[0].SubItems[2].Text;

                    double selectedItemTotal = Convert.ToDouble(listView_sales.SelectedItems[0].SubItems[4].Text);
                    richTextBox_itemPrice.Text = selectedItemTotal.ToString("C", new CultureInfo("en-AU"));

                    break;

                default:
                    // cannot select multiple items
                    deselectAllItems();

                    button_removeItem.Enabled = false;

                    button_Discount.Enabled = false;

                    break;
            }
        }

        private void button_removeItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem selectedItem in listView_sales.SelectedItems)
            {
                listView_sales.Items.Remove(selectedItem);
            }

            // checkout button
            if (listView_sales.Items.Count > 0)
            {
                button_checkout.Enabled = true;
            }
            else
            {
                button_checkout.Enabled = false;
            }

            displayTotal();
        }

        /// <summary>
        /// Checkout button clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_checkout_Click(object sender, EventArgs e)
        {
            // confirmation
            float total = 0;
            foreach (ListViewItem item in listView_sales.Items)
            {
                total += float.Parse(item.SubItems[4].Text);
            }
            DialogResult checkoutConfirmResult = MessageBox.Show("Total: " + total.ToString() + "\n\nCheckout Now? Clicking NO will clear sale.", "Retail POS", 
                MessageBoxButtons.YesNoCancel);
            switch (checkoutConfirmResult)
            {
                case DialogResult.Yes:
                    break;
                case DialogResult.No:
                    reset();
                    return;
                case DialogResult.Cancel:
                    return;
            }

            // gather the data
            List<string[]> cart = new List<string[]>();
            foreach (ListViewItem item in listView_sales.Items)
            {
                cart.Add(new string[] { item.SubItems[0].Text,
                                           item.SubItems[1].Text,
                                           item.SubItems[2].Text,
                                           item.SubItems[3].Text,
                                           item.SubItems[4].Text });
            }

            // two main tasks:
            // 1. update the database
            // 2. generate invoice

            // update the database

            Controller.CheckoutHandler checkoutHandler = new CheckoutHandler(productsController, purchasesController);

            Customer member = null;
            bool customerIsMember = false;
            switch (currentState)
            {
                case (State.SALE_MEMBER):
                    customerIsMember = true;
                    break;
                case (State.SALE_NON_MEMBER):
                    customerIsMember = false;
                    break;
                default:
                    // should never happen
                    throw new ApplicationException("Invalid state for checkout operation: " + currentState.ToString());
            }

            List<IPurchase> purchases = new List<IPurchase>();

            Staff currStaff = null;  

            try
            {
                // read and write data from the database
                await Task.Run(() =>
                {
                    currStaff = (Staff)staffController.service.read(Configuration.staffID);

                    foreach (string[] item in cart)
                    {
                        if (customerIsMember)
                        {
                            member = (Customer)customerController.service.read(int.Parse(textBox_customerAccNo.Text));
                        }

                        for (int i = 1; i <= int.Parse(item[2]); i++)
                        {
                            Purchase purchase = new Purchase
                            {
                                customer = member,
                                staff = currStaff,
                                product = (Product)productsController.service.read(item[0])
                            };

                            purchases.Add(purchase);
                        }
                    }

                    checkoutHandler.processCheckout(purchases);
                });
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                // checkout failed, database error
                // tell the user and the logger
                string checkoutFailedMsg = "Checkout Failed. Database Error.";
                System.Windows.Forms.MessageBox.Show(checkoutFailedMsg, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Error(checkoutFailedMsg);
                logger.Error("Stack Trace: " + checkoutFailedMsg);

                // nothing more we can do
                return;
            }

            // generate invoice
            try
            {
                
                var folder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                ExcelWriter invoice = new ExcelWriter(Path.Combine(folder, "invoices"), "Invoice " + DateTime.Now.ToString("dd-MM-yyyy hh꞉mm꞉ss tt"), "Invoice", "Invoice"); 

                // write headers and metadata
                Dictionary<string, string> extraMetadata = new Dictionary<string, string>();
                if (customerIsMember)
                {
                    extraMetadata.Add("Customer ID: ", member.customerId.ToString());
                    extraMetadata.Add("Customer Name: ", member.fullName);

                    invoice.writeHeadersAndMetadata(currStaff.staffID, currStaff.fullName, new string[] {"Product Number", "Product Description", "Price", "Quantity", "Paid" }, extraMetadata);
                }
                else
                {
                    invoice.writeHeadersAndMetadata(currStaff.staffID, currStaff.fullName, new string[] {"Product Number", "Product Description", "Price", "Quantity", "Paid" }, null);
                }

                // write product purchases data
                List<string[]> purchaseData = new List<string[]>();
                foreach (ListViewItem item in listView_sales.Items)
                {
                    purchaseData.Add(new string[] { item.SubItems[0].Text,
                                                       item.SubItems[1].Text,
                                                       item.SubItems[3].Text,
                                                       item.SubItems[2].Text,
                                                       item.SubItems[4].Text });
                }
                await Task.Run(() =>
                {
                    foreach (var record in purchaseData)
                        invoice.writeRecord(record);
                    
                    // save the file
                    invoice.close();
                });
                
            }
            catch (System.IO.IOException ex)
            {
                // failed to save invoice
                // tell the user and the logger
                string saveInvoiceFailedMsg = "Error saving invoice.";
                System.Windows.Forms.MessageBox.Show(saveInvoiceFailedMsg, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Error(saveInvoiceFailedMsg);
                logger.Error("Stack Trace: " + saveInvoiceFailedMsg);

                // nothing more we can do
                reset();
                return;
            }

            // at this point, it succeeded
            System.Windows.Forms.MessageBox.Show("Checkout succeeded.", "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            logger.Info("Checkout succeeded.");
            reset();
        }

        /// <summary>
        /// Display total price in the price indicator.
        /// </summary>
        private void displayTotal()
        {
            if (listView_sales.Items.Count > 0)
            {
                float fTotalCost = 0;
                foreach (ListViewItem item in listView_sales.Items)
                {
                    // get total price of current item
                    string sCurrentItemCost = item.SubItems[4].Text;
                    float fCurrentItemCost = float.Parse(sCurrentItemCost);
                    fTotalCost += fCurrentItemCost;
                }

                string priceDisplayed = "Total: " + fTotalCost.ToString("C2", new CultureInfo("en-AU"));
                richTextBox_itemPrice.Text = priceDisplayed;

                //button_removeItem.Enabled = true;
            }
            else
            {
                double priceDisplayed = 0.00;
                richTextBox_itemPrice.Text = priceDisplayed.ToString("C2", new CultureInfo("en-AU"));

                button_removeItem.Enabled = false;
            }
        }

        /// <summary>
        /// Discount button clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Discount_Click(object sender, EventArgs e)
        {
            // create discount form and show it
            string selectedItemID = listView_sales.SelectedItems[0].SubItems[0].Text;
            float selectedItemPrice = float.Parse(listView_sales.SelectedItems[0].SubItems[4].Text);
            DiscountForm discountForm = new DiscountForm(selectedItemID, selectedItemPrice);
            discountForm.Show();

            // subscribe to events from the discount form
            discountForm.OnApplyDiscount += new EventHandler<DiscountForm.DiscountEventArgs>(applyDiscount);
        }

        /// <summary>
        /// Discount event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void applyDiscount(object sender, DiscountForm.DiscountEventArgs e)
        {
            // extract data from event arguments
            string itemNumber = e.getItemIDNumber();
            float totalItemPrice = e.getPrice();

            // find the correct item in the list
            foreach (ListViewItem item in listView_sales.Items)
            {
                if (item.SubItems[0].Text.Equals(itemNumber))
                {
                    // apply discount
                    item.SubItems[4].Text = totalItemPrice.ToString();

                    // reselect item
                    ListViewItem currSelectedItem = listView_sales.SelectedItems[0];
                    currSelectedItem.Selected = false;
                    currSelectedItem.Selected = true;

                    break;
                }
            }
        }

        /// <summary>
        /// Look up item button clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_priceLookup_Click(object sender, EventArgs e)
        {
            string productNumber = textBox_itemProductID.Text;
            Product retrieved = null;
            try
            {
                await Task.Run(() =>
                {
                    retrieved = (Product)productsController.service.read(productNumber);
                });
            }
            catch (Exception ex)
            {
                // database error
                // tell the user and the logger
                string dbErrorMessage = "Error retrieving data from database: " + ex.Message;
                MessageBox.Show(dbErrorMessage, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Error(ex, dbErrorMessage + ": " + ex.Message);
                logger.Error("Stack trace: " + ex.StackTrace);

                // nothing more we can do
                return;
            }
            
            if (retrieved == null)
            {
                // could not retrieve product
                // tell the user and the logger
                string nullProductMessage = "Error: Could not find specified product";
                MessageBox.Show(nullProductMessage, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                logger.Info(nullProductMessage);

                // nothing more we can do
                return;
            }
            else
            {
                // at this point, it succeeded
                // tell the user and the logger
                MessageBox.Show("Product ID: " + productNumber + "\nDescription: " + retrieved.description +
                                "\nPrice: " + retrieved.price.ToString(), "Item Lookup", MessageBoxButtons.OK);
                logger.Info("Successfully retrieved product information");

            }

            textBox_itemProductID.Text = string.Empty;
        }
        
        // "import" menu item click events
        //Controller.SpreadsheetImportFactory spreadsheetImportFactory = new Controller.SpreadsheetImportFactory();
        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
  
        }

        private void customersToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void staffToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void productsToolStripMenuItem2_Click(object sender, EventArgs e)
        {
  
        }

        private void textBox_itemProductID_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData==Keys.Tab)
            {
                // make sure the text box is not empty, and in a sale state
                if (((textBox_itemProductID.Text!=null) && !(textBox_itemProductID.Text.Equals(string.Empty))) && ((this.currentState==State.SALE_MEMBER) || (this.currentState==State.SALE_NON_MEMBER)))
                {
                    // trigger the add item button click event
                    button_addItem.PerformClick();
                    textBox_itemProductID.Select();
                }
            }
        }

        private void textBox_itemQuantity_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Tab)
            {
                // make sure the text box is not empty, and in a sale state
                if (((textBox_itemProductID.Text != null) && !(textBox_itemProductID.Text.Equals(string.Empty))) && ((this.currentState == State.SALE_MEMBER) || (this.currentState == State.SALE_NON_MEMBER)))
                {
                    // trigger the add item button click event
                    button_addItem.PerformClick();
                    textBox_itemProductID.Select();
                }
            }
        }

        /// <summary>
        /// Help -> About menu item clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.Show();
        }

        /// <summary>
        /// Data -> Export -> Staff menu item clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void menuItem_exportStaff_Click(object sender, EventArgs e)
        {
            // show the save file dialog
            // allow options for .xlsx and .csv
            SaveFileDialog exportDialog = new SaveFileDialog();
            exportDialog.Filter = "Excel spreadsheet (*.xlsx)|*.xlsx|Comma-separated values file (*.csv)|*.csv";

            if (exportDialog.ShowDialog() == DialogResult.OK)
            {
               
                string fullPath = exportDialog.FileName;
                string dir = Path.GetDirectoryName(fullPath);
                string fileName = Path.GetFileNameWithoutExtension(fullPath);
                string fileExt = Path.GetExtension(fullPath);

                if (fileExt.Equals(".xlsx"))
                {
                    try
                    {
                        await Task.Run(() =>
                        {
                            ExcelWriter staffExportExcel = new ExcelWriter(dir, fileName, "Staff", "Staff");

                            // write headers and metadata to the spreadsheet
                            staffExportExcel.writeHeadersAndMetadata(Configuration.staffID, staffController.service.read(Configuration.staffID).fullName,
                                new string[] { "Staff ID", "Full Name", "Password Hash", "Privelege Level" }, null);

                            // write data records to the spreadsheet
                            foreach (var staffRecord in staffController.service.readAll())
                                staffExportExcel.writeRecord(new string[] { staffRecord.staffID.ToString(),
                                    staffRecord.fullName, staffRecord.passwordHash, staffRecord.privelege.ToString() });

                            // save the file
                            staffExportExcel.close();
                        });
                    }
                    catch (System.Data.SqlClient.SqlException sqlEx)
                    {
                        // database error
                        // tell the user and the logger
                        string dbErrorMsg = "Failed to export data. Database error.";
                        System.Windows.Forms.MessageBox.Show(dbErrorMsg, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        logger.Error(dbErrorMsg);
                        logger.Error("Stack Trace: " + sqlEx.StackTrace);

                        // nothing more we can do
                        return;
                    }
                    catch (System.IO.IOException ioEx)
                    {
                        // IO error
                        string ioErrorMsg = "Failed to export data. Error saving file.";
                        System.Windows.Forms.MessageBox.Show(ioErrorMsg, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        logger.Error(ioErrorMsg);
                        logger.Error("Stack Trace: " + ioEx.StackTrace);

                        // nothing more we can do
                        return;
                    }
                }
                else if (fileExt.Equals(".csv"))
                {

                }

                // at this point, it succeeded
                // tell the user and the logger.
                string exportSuccessMsg = "Data export successful.";
                System.Windows.Forms.MessageBox.Show(exportSuccessMsg, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                logger.Info(exportSuccessMsg);
            }
        }

        /// <summary>
        /// Data -> View -> Transaction History menu item clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem_viewTransactionHistory_Click(object sender, EventArgs e)
        {
            View.ViewTransactionsForm transactionsListForm = new ViewTransactionsForm();
            transactionsListForm.Show();
        }

        /// <summary>
        /// Data -> Export -> Customer menu item clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void menuItem_exportCustomers_Click(object sender, EventArgs e)
        {
            // show the save file dialog
            // allow options for .xlsx and .csv
            SaveFileDialog exportDialog = new SaveFileDialog();
            exportDialog.Filter = "Excel spreadsheet (*.xlsx)|*.xlsx|Comma-separated values file (*.csv)|*.csv";

            if (exportDialog.ShowDialog() == DialogResult.OK)
            {
                string fullPath = exportDialog.FileName;
                string dir = Path.GetDirectoryName(fullPath);
                string fileName = Path.GetFileNameWithoutExtension(fullPath);
                string fileExt = Path.GetExtension(fullPath);

                if (fileExt.Equals(".xlsx"))
                {
                    try
                    {
                        await Task.Run(() =>
                        {
                            ExcelWriter customerExportExcel = new ExcelWriter(dir, fileName, "Customers", "Customers");

                            // write headers and metadata to the spreadsheet
                            customerExportExcel.writeHeadersAndMetadata(Configuration.staffID, staffController.service.read(Configuration.staffID).fullName, 
                                new string[] { "Customer ID", "Full Name", "Address", "Phone Number", "Email", "City", "State", "Postcode" }, null);

                            // write data records to the spreadsheet
                            foreach (var customerRecord in customerController.service.readAll())
                                customerExportExcel.writeRecord(new string[] { customerRecord.customerId.ToString(),
                                    customerRecord.fullName, customerRecord.address, customerRecord.phoneNumber, customerRecord.email, customerRecord.city, customerRecord.state.ToString(),
                                    customerRecord.Postcode.ToString() });

                            // save the file
                            customerExportExcel.close();
                        });
                    }
                    catch (System.Data.SqlClient.SqlException sqlEx)
                    {
                        // database error
                        // tell the user and the logger
                        string dbErrorMsg = "Failed to export data. Database error.";
                        System.Windows.Forms.MessageBox.Show(dbErrorMsg, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        logger.Error(dbErrorMsg);
                        logger.Error("Stack Trace: " + sqlEx.StackTrace);

                        // nothing more we can do
                        return;
                    }
                    catch (System.IO.IOException ioEx)
                    {
                        // io error
                        string ioErrorMsg = "Failed to export data. Error saving file.";
                        System.Windows.Forms.MessageBox.Show(ioErrorMsg, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        logger.Error(ioErrorMsg);
                        logger.Error("Stack Trace: " + ioEx.StackTrace);

                        // nothing more we can do
                        return;
                    }
                }
                else if (fileExt.Equals(".csv"))
                {

                }

                // at this point, it succeeded
                // tell the user and the logger.
                string exportSuccessMsg = "Data export successful.";
                System.Windows.Forms.MessageBox.Show(exportSuccessMsg, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                logger.Info(exportSuccessMsg);
            }
        }

        /// <summary>
        /// Data -> Export -> Products menu item clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void menuItem_exportProducts_Click(object sender, EventArgs e)
        {
            // show the save file dialog
            // allow the options for .xlsx and .csv
            SaveFileDialog exportDialog = new SaveFileDialog();
            exportDialog.Filter = "Excel spreadsheet (*.xlsx)|*.xlsx|Comma-separated values file (*.csv)|*.csv";

            if (exportDialog.ShowDialog() == DialogResult.OK)
            {
                string fullPath = exportDialog.FileName;
                string dir = Path.GetDirectoryName(fullPath);
                string fileName = Path.GetFileNameWithoutExtension(fullPath);
                string fileExt = Path.GetExtension(fullPath);

                if (fileExt.Equals(".xlsx"))
                {
                    try
                    {
                        await Task.Run(() =>
                        {
                            ExcelWriter productsExportExcel = new ExcelWriter(dir, fileName, "Product", "Product");

                            // write headers and metadata to the spreadsheet
                            productsExportExcel.writeHeadersAndMetadata(Configuration.staffID, staffController.service.read(Configuration.staffID).fullName,
                                    new string[] { "Product ID", "Product Number", "Description", "Price", "Quantity" }, null);

                            // write data records to the spreadsheet
                            foreach (var productRecord in productsController.service.readAll())
                                productsExportExcel.writeRecord(new string[] { productRecord.productId.ToString(),
                            productRecord.productNumber, productRecord.description, productRecord.price.ToString(), productRecord.quantity.ToString()});

                            // save the file
                            productsExportExcel.close();
                        });
                    }
                    catch (System.Data.SqlClient.SqlException sqlEx)
                    {
                        // database error
                        // tell the user and the logger
                        string dbErrorMsg = "Failed to export data. Database error.";
                        System.Windows.Forms.MessageBox.Show(dbErrorMsg, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        logger.Error(dbErrorMsg);
                        logger.Error("Stack Trace: " + sqlEx.StackTrace);

                        // nothing more we can do
                        return;
                    }
                    catch (System.IO.IOException ioEx)
                    {
                        // IO error
                        string ioErrorMsg = "Failed to export data. Error saving file.";
                        System.Windows.Forms.MessageBox.Show(ioErrorMsg, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        logger.Error(ioErrorMsg);
                        logger.Error("Stack Trace: " + ioEx.StackTrace);

                        // nothing more we can do
                        return;
                    }
                }
                else if (fileExt.Equals(".csv"))
                {

                }

                // at this point, it succeeded
                // tell the user and the logger.
                string exportSuccessMsg = "Data export successful.";
                System.Windows.Forms.MessageBox.Show(exportSuccessMsg, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                logger.Info(exportSuccessMsg);
            }
        }
    }
}
