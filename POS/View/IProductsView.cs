using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.ObjectModel;

namespace POS.View
{
    public interface IProductsView
    {
        void populateProducts(IEnumerable<IProduct> products);

        // events
        event EventHandler populateProductsRequest;
        event EventHandler<AddProductRequestEventArgs> addProductRequest;
        event EventHandler<DeleteProductRequestEventArgs> deleteProductRequest;
    }

    /// <summary>
    /// Custom event arguments class for a request to add a new product.
    /// </summary>
    public class AddProductRequestEventArgs : EventArgs
    {
        private IProduct _product;

        /// <summary>
        /// Constructor with parameter.
        /// </summary>
        /// <param name="product"></param>
        public AddProductRequestEventArgs(IProduct product)
        {
            this._product = product;
        }

        public IProduct getProduct() { return this._product; }
    }

    /// <summary>
    /// Custom event arguments class for a request to delete a product, using an item number.
    /// </summary>
    public class DeleteProductRequestEventArgs : EventArgs
    {
        private string _productNumber;

        /// <summary>
        /// Constructor with parameter.
        /// </summary>
        /// <param name="productNumber"></param>
        public DeleteProductRequestEventArgs(string productNumber)
        {
            this._productNumber = productNumber;
        }

        public string getProductNumber() { return this._productNumber; }
    }
}
