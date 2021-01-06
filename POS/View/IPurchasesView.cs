using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.ObjectModel;

namespace POS.View
{
    public interface IPurchasesView
    {
        void populatePurchases(IEnumerable<IPurchase> purchases);

        // events
        event EventHandler populatePurchasesRequest;
        event EventHandler<AddPurchaseRequestEventArgs> addPurchaseRequest;
    }

    public class AddPurchaseRequestEventArgs : EventArgs
    {
        private IPurchase _purchase;

        public AddPurchaseRequestEventArgs(IPurchase purchase)
        {
            this._purchase = purchase;
        }

        public IPurchase getPurchase() { return this._purchase; }
    }
}
