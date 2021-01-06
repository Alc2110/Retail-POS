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
    public interface IPurchasesController
    {
        // view dependency injection
        IList<IPurchasesView> views { get; }

        // service layer dependency injection
        IPurchasesService service { get; set; }

        // data operations
        void create(IPurchase purchase);
        IEnumerable<IPurchase> readAll();
    }

    public class PurchasesController : IPurchasesController
    {
        // view dependency injection
        public IList<IPurchasesView> views { get; }

        /// <summary>
        /// Add a view to the list of observers.
        /// </summary>
        /// <param name="view"></param>
        public void addObserver(IPurchasesView view)
        {
            view.addPurchaseRequest += V_addPurchaseRequest;
            view.populatePurchasesRequest += V_populatePurchasesRequest;
        }

        // service layer dependency injection
        public IPurchasesService service { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PurchasesController()
        {
            this.service = new PurchasesService();

            this.views = new List<IPurchasesView>();
        }

        /// <summary>
        /// Add a new purchase record to the database.
        /// </summary>
        /// <param name="purchase"></param>
        public void create(IPurchase purchase)
        {
            this.service.create(purchase);
        }

        /// <summary>
        /// Read all purchase records from the database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IPurchase> readAll()
        {
            return this.service.readAll();
        }

        /// <summary>
        /// Event handler for populate requests from views.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void V_populatePurchasesRequest(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Event handler for add new purchase requests from views.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void V_addPurchaseRequest(object sender, AddPurchaseRequestEventArgs args)
        {
            service.create(args.getPurchase());
        }
    }
}
