using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS.View
{
    public partial class DiscountForm : Form
    {
        public string itemID;
        public float itemTotalPrice;

        // get an instance of the logger for this class
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public DiscountForm(string itemID, float totalPrice)
        {
            InitializeComponent();

            logger.Info("Initialising discount form");

            this.itemID = itemID;
            this.itemTotalPrice = totalPrice;

            button_apply.Enabled = false;
            button_OK.Enabled = false;

            radioButton_percentage.Checked = true;

            trackBar_percentDiscount.Enabled = true;
            trackBar_percentDiscount.Maximum = 100;

            textBox_amountDiscount.Enabled = false;
            textBox_specifyPrice.Enabled = false;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            logger.Info("Closing discount form");

            this.Close();
        }

        private void button_apply_Click(object sender, EventArgs e)
        {
            // calculate new price
            float newPrice = itemTotalPrice;
            if (trackBar_percentDiscount.Enabled)
            {
                int percentValue = trackBar_percentDiscount.Value;
                newPrice = itemTotalPrice - (((float)percentValue)/100)*itemTotalPrice;
            }
            else if (textBox_specifyPrice.Enabled)
            {
                try
                {
                    newPrice = float.Parse(textBox_specifyPrice.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error parsing price", "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
            }
            else if (textBox_amountDiscount.Enabled)
            {
                try
                {
                    float discountAmount = float.Parse(textBox_amountDiscount.Text);
                    newPrice = itemTotalPrice - discountAmount;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error parsing price", "Retail POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                    return;
                }
            }

            // fire the event
            logger.Info("Applying discount: ");
            logger.Info("Product ID: " + itemID + ", New price: " + newPrice);
            OnApplyDiscount(null, new DiscountEventArgs(itemID, newPrice));

            button_OK.Enabled = true;
        }

        private void radioButton_specifyPrice_CheckedChanged(object sender, EventArgs e)
        {
            button_apply.Enabled = false;

            if (radioButton_specifyPrice.Checked)
            {
                textBox_specifyPrice.Enabled = true;
            }
            else
            {
                textBox_specifyPrice.Enabled = false;
                textBox_specifyPrice.Text = null;
            }
        }

        private void radioButton_dollarAmount_CheckedChanged(object sender, EventArgs e)
        {
            button_apply.Enabled = false;

            if (radioButton_dollarAmount.Checked)
            {
                textBox_amountDiscount.Enabled = true;
            }
            else
            {
                textBox_amountDiscount.Enabled = false;
                textBox_amountDiscount.Text = null;
            }
        }

        private void radioButton_percentage_CheckedChanged(object sender, EventArgs e)
        {
            button_apply.Enabled = false;

            if (radioButton_percentage.Checked)
            {
                trackBar_percentDiscount.Enabled = true;
            }
            else
            {
                trackBar_percentDiscount.Enabled = false;
                trackBar_percentDiscount.Value = 0;
            }
        }

        // event for applying discount
        public event EventHandler<DiscountEventArgs> OnApplyDiscount = delegate { };

        /// <summary>
        /// Event arguments class
        /// </summary>
        public class DiscountEventArgs : EventArgs
        {
            private string itemIDnumber;
            private float calculatedPrice;

            // ctor
            public DiscountEventArgs(string itemIDnumber, float calculatedPrice)
            {
                this.itemIDnumber = itemIDnumber;
                this.calculatedPrice = calculatedPrice;
            }

            public string getItemIDNumber()
            {
                return this.itemIDnumber;
            }

            public float getPrice()
            {
                return this.calculatedPrice;
            }
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox_amountDiscount_TextChanged(object sender, EventArgs e)
        {
            if ((textBox_amountDiscount.Text!=null) && (textBox_amountDiscount.Text!=string.Empty))
            {
                button_apply.Enabled = true;
            }
            else
            {
                button_apply.Enabled = false;
            }
        }

        private void textBox_specifyPrice_TextChanged(object sender, EventArgs e)
        {
            if ((textBox_specifyPrice.Text!=null) && (textBox_specifyPrice.Text!=string.Empty))
            {
                button_apply.Enabled = true;
            }
            else
            {
                button_apply.Enabled = false;
            }
        }

        private void trackBar_percentDiscount_ValueChanged(object sender, EventArgs e)
        {
            label_percentage.Text = trackBar_percentDiscount.Value + "%";

            button_apply.Enabled = true;
        }
    } 
}
