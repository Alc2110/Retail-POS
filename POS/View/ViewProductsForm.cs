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
    public partial class ViewProductsForm : Form, IProductsView
    {
        // get an instance of the logger for this class
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private delegate void populateProductsDelegate(IEnumerable<IProduct> products);

        /// <summary>
        /// Fill the list.
        /// </summary>
        /// <param name="products"></param>
        public void populateProducts(IEnumerable<IProduct> products)
        {
            if (listView_products.InvokeRequired)
            {
                showBusyIndicator();

                var d = new populateProductsDelegate(populateProducts);

                listView_products.Invoke(d, products);

                removeBusyIndicator();
            }
            else
            {
                showBusyIndicator();

                _populateProducts(products);

                removeBusyIndicator();
            }
        }

        private void _populateProducts(IEnumerable<IProduct> products)
        {
            //showBusyIndicator();

            listView_products.BeginUpdate();

            // clear the list
            listView_products.Items.Clear();

            // add items to the list
            foreach (var p in products)
            {
                ListViewItem newItem = new ListViewItem(new string[] { p.productNumber, p.description, p.quantity.ToString(), p.price.ToString() });
                newItem.Font = new Font(listView_products.Font, FontStyle.Regular); // remove the bold font styling that was applied when setting the headers bold
                listView_products.Items.Add(newItem);
            }

            listView_products.EndUpdate();

            //removeBusyIndicator();
        }

        #region events
        public event EventHandler populateProductsRequest;

        protected virtual void OnPopulateRequest(EventArgs args)
        {
            EventHandler temp = populateProductsRequest;
            if (temp != null)
                populateProductsRequest?.Invoke(this, args);
        }
        
        public event EventHandler<AddProductRequestEventArgs> addProductRequest;

        public virtual void OnAddProductRequest(AddProductRequestEventArgs args)
        {
            EventHandler<AddProductRequestEventArgs> temp = addProductRequest;
            if (temp != null)
                addProductRequest?.Invoke(this, args);
        }
        
        public event EventHandler<DeleteProductRequestEventArgs> deleteProductRequest;

        protected virtual void OnDeleteRequest(DeleteProductRequestEventArgs args)
        {
            EventHandler<DeleteProductRequestEventArgs> temp = deleteProductRequest;
            if (temp != null)
                deleteProductRequest?.Invoke(this, args);
        }
        #endregion

        /// <summary>
        /// Default constructor for this window.
        /// </summary>
        public ViewProductsForm()
        {
            InitializeComponent();

            logger.Info("Initialising view products form");

            // prepare the listView
            listView_products.Columns.Add("Product Number", 150);
            listView_products.Columns.Add("Description", 300);
            listView_products.Columns.Add("Quantity", 100);
            listView_products.Columns.Add("Price", 100);
            listView_products.View = System.Windows.Forms.View.Details;
            listView_products.GridLines = true;
            // make the headers bold
            listView_products.Columns[0].ListView.Font = new Font(listView_products.Columns[0].ListView.Font, FontStyle.Bold);

            // initially show the busy indicator
            showBusyIndicator();
            // colour and position the busy indicator properly
            circularProgressBar1.Location = calculateBusyIndicatorPos();
            circularProgressBar1.ProgressColor = Configuration.ProgressBarColours.TASK_IN_PROGRESS_COLOUR;

            // can't delete anything until something is selected
            button_deleteSelectedProduct.Enabled = false;
        }

        #region UI event handlers
        /// <summary>
        /// Close button clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_close_Click(object sender, EventArgs e)
        {
            logger.Info("Closing new products form");

            this.Close();
        }

        /// <summary>
        /// Show add new product window clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_addNewProduct_Click(object sender, EventArgs e)
        {
            // create the add new product form and subscribe to "Add" button click events
            NewProductForm newProductForm = new NewProductForm(this);
            newProductForm.Show();
        }

        /// <summary>
        /// Delete product button clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_deleteSelectedProduct_Click(object sender, EventArgs e)
        {
            // TODO: fix cannot delete products that are associated with a Transaction record. See: https://dba.stackexchange.com/questions/153351/delete-statement-conflicted-with-the-reference-constraint
            throw new NotImplementedException();   
        }

        /// <summary>
        /// Selected product changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_products_SelectedIndexChanged(object sender, EventArgs e)
        {
            // only one item allowed to be selected at a time
            if (listView_products.SelectedItems.Count > 1)
            {
                foreach (ListViewItem item in listView_products.SelectedItems)
                {
                    item.Selected = false;
                }
            }

            if (listView_products.SelectedItems.Count == 1)
            {
                //button_deleteSelectedProduct.Enabled = true;
            }
            else
            {
                button_deleteSelectedProduct.Enabled = false;
            }
        }

        /// <summary>
        /// Window resized.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewProductsForm_Resize(object sender, EventArgs e)
        {
            // reposition the circular progress bar
            circularProgressBar1.Location = calculateBusyIndicatorPos();
        }

        /// <summary>
        /// Window loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewProductsForm_Load(object sender, EventArgs e)
        {
            // populate the list upon loading
            OnPopulateRequest(null);
        }
        #endregion

        /// <summary>
        /// Show the list view and hide the busy indicator.
        /// </summary>
        private void removeBusyIndicator()
        {
            circularProgressBar1.Visible = false;
            listView_products.Visible = true;
        }

        /// <summary>
        /// Show the busy indicator and hide the list view.
        /// </summary>
        private void showBusyIndicator()
        {
            circularProgressBar1.Visible = true;
            listView_products.Visible = false;
        }

        /// <summary>
        /// Calculate the appropriate position of the busy indicator.
        /// </summary>
        /// <returns></returns>
        private System.Drawing.Point calculateBusyIndicatorPos()
        {
            int xPos = ((groupBox1.Width) / 2) - ((circularProgressBar1.Width) / 2);
            int yPos = ((groupBox1.Height) / 2) - ((circularProgressBar1.Height) / 2);

            return new System.Drawing.Point(xPos, yPos);
        } 
    }
}
