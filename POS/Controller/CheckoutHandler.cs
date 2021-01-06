using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.ObjectModel;

namespace POS.Controller
{
    /// <summary>
    /// Processes a checkout. Takes a collection of Purchases and updates the database.
    /// </summary>
    public class CheckoutHandler
    {
        IProductsController productsController;

        IPurchasesController purchasesController;

        /// <summary>
        /// Constructor, injects appropriate controllers.
        /// </summary>
        /// <param name="productsController"></param>
        /// <param name="purchasesController"></param>
        public CheckoutHandler(IProductsController productsController, IPurchasesController purchasesController)
        {
            this.productsController = productsController;
            this.purchasesController = purchasesController;
        }

        /// <summary>
        /// Update the database and fire appropriate events.
        /// </summary>
        /// <param name="items"></param>
        public void processCheckout(IEnumerable<IPurchase> items)
        {
            // add new purchases
            foreach (var item in items)
                purchasesController.create(item);

            // fire the event to update any product views
            productsController.service.fire();
        }
    }
}
