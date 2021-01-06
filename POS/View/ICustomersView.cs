using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.ObjectModel;

namespace POS.View
{
    public interface ICustomersView
    {
        void populateCustomers(IEnumerable<ICustomer> customers);

        // events
        event EventHandler populateCustomersRequest;
        event EventHandler<AddCustomerRequestEventArgs> addCustomerRequest;
        event EventHandler<DeleteCustomerRequestEventArgs> deleteCustomerRequest;
    }

    public class AddCustomerRequestEventArgs : EventArgs
    {
        private ICustomer _customer;

        public AddCustomerRequestEventArgs(ICustomer customer)
        {
            this._customer = customer;
        }

        public ICustomer getCustomer() { return this._customer; }
    }

    public class DeleteCustomerRequestEventArgs : EventArgs
    {
        private string _productNumber;

        public DeleteCustomerRequestEventArgs(string productNumber)
        {
            this._productNumber = productNumber;
        }

        public string getProductNumber() { return this._productNumber; }
    }
}
