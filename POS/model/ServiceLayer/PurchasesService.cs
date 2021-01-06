using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.ObjectModel;
using Model.DataAccessLayer;

namespace Model.ServiceLayer
{
    public interface IPurchasesService
    {
        // data access layer dependency injection
        IPurchaseDataAccessObject dataAccessObject { get; set; }

        // data operations
        void create(IPurchase purchase);
        IEnumerable<IPurchase> readAll();

        // event for retrieving all purchase records
        event EventHandler<GetAllPurchasesEventArgs> GetAllPurchases;
    }

    public class PurchasesService : IPurchasesService
    {
        // data access layer dependency injection
        public IPurchaseDataAccessObject dataAccessObject { get; set; }

        /// <summary>
        /// Constructor with data access layer parameter.
        /// </summary>
        /// <param name="dataAccessObject"></param>
        public PurchasesService(IPurchaseDataAccessObject dataAccessObject)
        {
            this.dataAccessObject = dataAccessObject;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PurchasesService()
        {
            this.dataAccessObject = new PurchaseDataAccessObject();
        }

        /// <summary>
        /// Create a new purchase record in the database.
        /// </summary>
        /// <param name="purchase"></param>
        public void create(IPurchase purchase)
        {
            // ask the data access layer to do the processing
            this.dataAccessObject.addTransaction(purchase);

            // fire the event to retrieve all purchase records
            OnGetAllPurchases(new GetAllPurchasesEventArgs(this.dataAccessObject.getAllTransactions()));
        }

        /// <summary>
        /// Retrieve all purchase records from the database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IPurchase> readAll()
        {
            return this.dataAccessObject.getAllTransactions();
        }

        public event EventHandler<GetAllPurchasesEventArgs> GetAllPurchases;

        protected virtual void OnGetAllPurchases(GetAllPurchasesEventArgs args)
        {
            EventHandler<GetAllPurchasesEventArgs> temp = GetAllPurchases;

            if (temp != null)
                GetAllPurchases?.Invoke(this, args);
        }
    }

    /// <summary>
    /// Custom event arguments class for retreiving all purchase records.
    /// </summary>
    public class GetAllPurchasesEventArgs : EventArgs
    {
        private IEnumerable<IPurchase> _purchases;

        public GetAllPurchasesEventArgs(IEnumerable<IPurchase> purchases)
        {
            this._purchases = purchases;
        }

        public IEnumerable<IPurchase> getList()
        {
            return _purchases;
        }
    }
}
