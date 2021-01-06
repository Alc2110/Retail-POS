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
using Model.ServiceLayer;

namespace POS.View
{
    public partial class ViewStaffForm : Form
    {
        // get an instance of the logger for this class
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        // delegate for handling events fired by the model
        private delegate void populateStaffViewDelegate(object sender, GetAllStaffEventArgs args);

        public ViewStaffForm()
        {
            InitializeComponent();

            logger.Info("Initialising view staff form");

            // prepare the listView
            listView_staff.Columns.Add("ID", 50);
            listView_staff.Columns.Add("Full name", 200);
            listView_staff.Columns.Add("Password", 300);
            listView_staff.Columns.Add("Privelege", 75);
            listView_staff.View = System.Windows.Forms.View.Details;
            listView_staff.GridLines = true;
            // make the headers bold
            for (int i = 0; i < listView_staff.Columns.Count; i++)
            {
                listView_staff.Columns[i].ListView.Font = new Font(listView_staff.Columns[i].ListView.Font, FontStyle.Bold);
            }

            // colour and position the busy indicator properly
            circularProgressBar1.Location = calculateBusyIndicatorPos();
            circularProgressBar1.ProgressColor = Configuration.ProgressBarColours.TASK_IN_PROGRESS_COLOUR;
            showBusyIndicator();

            // can't delete anything until something is selected
            button_deleteSelectedStaff.Enabled = false;

    
        }

        #region UI event handlers
        private void button_addNewStaff_Click(object sender, EventArgs e)
        {
            NewStaffForm newStaffForm = new NewStaffForm();
            newStaffForm.Show();
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            logger.Info("Closing view staff form");

            this.Close();
        }

        private async void button_deleteSelectedStaff_Click(object sender, EventArgs e)
        {
            
                try
                {
                    showBusyIndicator();

                    // prepare the data
                    int idNum;
                    Int32.TryParse(listView_staff.SelectedItems[0].SubItems[0].Text, out idNum);
                    
                    // log it
                    logger.Info("Deleting staff record: ");
                    logger.Info("ID: " + idNum);

                    await Task.Run(() =>
                    {
                       
                    });
                }
                catch (Exception ex)
                {
                    // it failed
                    // tell the user and the logger
                    string errorMessage = "Error deleting data from database: " + ex.Message;
                    MessageBox.Show(errorMessage, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    logger.Error(ex, errorMessage);
                    logger.Error("Stack Trace: " + ex.StackTrace);

                    removeBusyIndicator();

                    // nothing more we can do
                    return;
                }

                removeBusyIndicator();

                // at this point, it succeeded
                // tell the user and the logger
                string successMessage = "Successfully deleted staff record";
                MessageBox.Show(successMessage, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                logger.Info(successMessage);
            
        }

        private void listView_staff_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView_staff.SelectedItems.Count>1)
            {
                // only allowed to select one item at a time
                foreach (ListViewItem selectedItem in listView_staff.SelectedItems)
                {
                    selectedItem.Selected = false;
                }
            }

            if (listView_staff.SelectedItems.Count==1)
            {
                button_deleteSelectedStaff.Enabled = true;
            }
            else
            {
                button_deleteSelectedStaff.Enabled = false;
            }
        }

        private void ViewStaffForm_Resize(object sender, EventArgs e)
        {
            circularProgressBar1.Location = calculateBusyIndicatorPos();
        }
        #endregion

        private void staffEventHandler(object sender, GetAllStaffEventArgs args)
        {
            if (listView_staff.InvokeRequired)
            {
                listView_staff.Invoke(new populateStaffViewDelegate(populateView), new object[] { sender, args });
            }
            else
            {
                populateView(sender, args);
            }
        }

        private void populateView(object sender, GetAllStaffEventArgs args)
        {
            showBusyIndicator();

            // tell the listView it is being updated
            listView_staff.BeginUpdate();

            // clear the listView
            listView_staff.Items.Clear();

            // populate it
            foreach (var staff in args.getList())
            {
                string[] itemArr = new string[4];
                itemArr[0] = staff.staffID.ToString();
                itemArr[1] = staff.fullName;
                itemArr[2] = staff.passwordHash;
                itemArr[3] = staff.privelege.ToString();

                ListViewItem staffListViewItem = new ListViewItem(itemArr);
                staffListViewItem.Font = new Font(staffListViewItem.Font, FontStyle.Regular);
                listView_staff.Items.Add(staffListViewItem);
            }

            // tell the listView it is ready
            listView_staff.EndUpdate();

            removeBusyIndicator();
        }

        private async void ViewStaffForm_Load(object sender, EventArgs e)
        {
            // subscribe to model events
            //Models.staffService.GetAllStaff += new EventHandler<GetAllStaffEventArgs>(staffEventHandler);

            // populate the list view upon loading
            try
            {
                showBusyIndicator();

                // run this task in a separate thread
                await Task.Run(() =>
                {
                    
                });
            }
            catch (Exception ex)
            {
                // it failed
                // tell the user and the logger
                string errorMessage = "Error retrieving data from the database: " + ex.Message;
                logger.Error(ex, errorMessage);
                logger.Error("Stack Trace: " + ex.StackTrace);
                MessageBox.Show(errorMessage, "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Error);

                removeBusyIndicator();

                return;
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
            listView_staff.Visible = true;
        }

        private void showBusyIndicator()
        {
            circularProgressBar1.Visible = true;
            listView_staff.Visible = false;
        }
    }
}
