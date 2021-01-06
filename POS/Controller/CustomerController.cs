using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.ObjectModel;
using Model.ServiceLayer;
using POS.View;

namespace POS.Controller
{
    public interface ICustomerController
    {
        // view dependency injection
        IList<ICustomersView> views { get; }

        // service layer dependency injection
        ICustomerService service { get; set; }

        // data operations
        void newCustomer(ICustomer customer);
        void deleteCustomer(int customerId);
    }

    public class CustomerController : ICustomerController
    {
        // view dependency injection
        public IList<ICustomersView> views { get; }

        /// <summary>
        /// Add a view to the list of observers, and subscribe to its events.
        /// </summary>
        /// <param name="view"></param>
        public void addObserver(ICustomersView view)
        {
            // subscribe to the view's events
            view.addCustomerRequest += V_addCustomerRequest;
            view.populateCustomersRequest += V_populateCustomersRequest;
            view.deleteCustomerRequest += V_deleteCustomerRequest;

            // add the view to the list of observers
            views.Add(view);
        }

        // service layer dependency injection
        public ICustomerService service { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CustomerController()
        {
            this.views = new List<ICustomersView>();

            this.service = new CustomerService();
        }

        /// <summary>
        /// Event handler for a delete customer request from views.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void V_deleteCustomerRequest(object sender, DeleteCustomerRequestEventArgs args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Event handler for a populate request from views.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void V_populateCustomersRequest(object sender, EventArgs args)
        {
            List<ICustomer> allCustomers = this.service.readAll().ToList();

            foreach (var v in views)
                v.populateCustomers(allCustomers);
        }

        /// <summary>
        /// Event handler for a add new customer request from views.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void V_addCustomerRequest(object sender, AddCustomerRequestEventArgs args)
        {
            this.service.create(args.getCustomer());
        }

        /// <summary>
        /// Add a new customer record in the database.
        /// </summary>
        /// <param name="customer"></param>
        public void newCustomer(ICustomer customer)
        {
            // ask the model to do the processing
            this.service.create(customer);
        }

        /// <summary>
        /// Delete a customer record from the database.
        /// </summary>
        /// <param name="customerId"></param>
        public void deleteCustomer(int customerId)
        {
            // ask the model to do the processing
            this.service.delete(customerId);
        }
    }
}
