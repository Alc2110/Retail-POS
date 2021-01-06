using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.ObjectModel;
using Model.DataAccessLayer;

namespace Model.ServiceLayer
{
    public interface ICustomerService
    {
        // data access layer dependency injection
        ICustomerDataAccessObject dataAccessObject { get; set; }

        // data operations
        void create(ICustomer customer);
        ICustomer read(int customerId);
        void delete(int customerId);
        IEnumerable<ICustomer> readAll();

        // event for retrieving all customer records
        event EventHandler<GetAllCustomersEventArgs> GetAllCustomers;
    }

    public class CustomerService : ICustomerService
    {
        // data access layer dependency injection
        public ICustomerDataAccessObject dataAccessObject { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CustomerService()
        {
            this.dataAccessObject = new CustomerDataAccessObject();
        }

        /// <summary>
        /// Constructor with data access layer parameter injection.
        /// </summary>
        /// <param name="dataAccessObject"></param>
        public CustomerService(ICustomerDataAccessObject dataAccessObject)
        {
            this.dataAccessObject = dataAccessObject;
        }

        /// <summary>
        /// Create a customer record in the database.
        /// </summary>
        /// <param name="customer"></param>
        public void create(ICustomer customer)
        {
            // ask the data access layer to create this record
            this.dataAccessObject.addCustomer(customer);

            // fire the event to retrieve all records
            OnGetAllCustomers(new GetAllCustomersEventArgs(this.dataAccessObject.getAllCustomers()));
        }

        /// <summary>
        /// Retrieve a customer record from the database.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public ICustomer read(int customerId)
        {
            // ask the data access layer to retrieve the record
            return this.dataAccessObject.getCustomer(customerId);
        }

        /// <summary>
        /// Delete a customer record from the database.
        /// </summary>
        /// <param name="customerId"></param>
        public void delete(int customerId)
        {
            // ask the Model to delete this record
            this.dataAccessObject.deleteCustomer(customerId);

            // fire the event to retrieve all records
            OnGetAllCustomers(new GetAllCustomersEventArgs(this.dataAccessObject.getAllCustomers()));
        }

        /// <summary>
        /// Retrieve all customer records from the database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ICustomer> readAll()
        {
            return this.dataAccessObject.getAllCustomers();
        }

        public event EventHandler<GetAllCustomersEventArgs> GetAllCustomers;

        protected virtual void OnGetAllCustomers(GetAllCustomersEventArgs args)
        {
            EventHandler<GetAllCustomersEventArgs> temp = GetAllCustomers;

            if (temp != null)
                GetAllCustomers?.Invoke(this, args);
        }
    }

    /// <summary>
    /// Custom event arguments class for retrieving all customer records.
    /// </summary>
    public class GetAllCustomersEventArgs : EventArgs
    {
        private IEnumerable<ICustomer> _customers;

        public GetAllCustomersEventArgs(IEnumerable<ICustomer> customers)
        {
            this._customers = customers;
        }

        public IEnumerable<ICustomer> getList()
        {
            return this._customers;
        }
    }
}
