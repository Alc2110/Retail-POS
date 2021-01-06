using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.ObjectModel;
using Model.DataAccessLayer;

namespace Model.ServiceLayer
{
    public interface IProductsService
    {
        // data access layer dependency injection
        IProductDataAccessObject dataAccessObject { get; set; }

        // data operations
        void create(IProduct product);
        IProduct read(string productNumber);
        void delete(string productNumber);
        IEnumerable<IProduct> readAll();

        void fire();

        // event for retrieving all product records
        event EventHandler<GetAllProductsEventArgs> GetAllProducts;
    }

    public class ProductsService : IProductsService
    {
        // data access layer dependency injection
        public IProductDataAccessObject dataAccessObject { get; set; }

        /// <summary>
        /// Constructor with data access layer dependency injection.
        /// </summary>
        /// <param name="dataAccessObject"></param>
        public ProductsService(IProductDataAccessObject dataAccessObject)
        {
            this.dataAccessObject = dataAccessObject;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ProductsService()
        {
            this.dataAccessObject = new ProductDataAccessObject();
        }

        /// <summary>
        /// Create a new product record in the database, then refresh the views via the event for the controller.
        /// </summary>
        /// <param name="product"></param>
        public void create(IProduct product)
        {
            // ask the service layer to add the new record to the database
            this.dataAccessObject.create(product);

            // fire the event to retrieve all records
            OnGetAllProducts(new GetAllProductsEventArgs(this.dataAccessObject.readAll()));
        }

        /// <summary>
        /// Retrieve all product records from the database.
        /// </summary>
        /// <param name="productNumber"></param>
        /// <returns></returns>
        public IProduct read(string productNumber)
        {
            return this.dataAccessObject.read(productNumber);
        }

        /// <summary>
        /// Fire the event to retrieve all product records from the database.
        /// </summary>
        public void fire()
        {
            // fire the event to retrieve all records
            OnGetAllProducts(new GetAllProductsEventArgs(this.dataAccessObject.readAll()));
        }

        /// <summary>
        /// Delete a product record from the database.
        /// </summary>
        /// <param name="productNumber"></param>
        public void delete(string productNumber)
        {
            // ask the data access layer to delete this record
            this.dataAccessObject.delete(productNumber);

            // fire the event to retrieve all records
            OnGetAllProducts(new GetAllProductsEventArgs(this.dataAccessObject.readAll()));
        }

        /// <summary>
        /// Retrieve all product records from the database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IProduct> readAll()
        {
            return this.dataAccessObject.readAll();
        }

        public event EventHandler<GetAllProductsEventArgs> GetAllProducts;

        protected virtual void OnGetAllProducts(GetAllProductsEventArgs args)
        {
            EventHandler<GetAllProductsEventArgs> temp = GetAllProducts;

            if (temp != null)
                GetAllProducts?.Invoke(this, args);
        }
    }

    // custom event args class for the get all products event
    public class GetAllProductsEventArgs : EventArgs
    {
        private IEnumerable<IProduct> _products;

        /// <summary>
        /// Constructor with parameter.
        /// </summary>
        /// <param name="products">The products.</param>
        public GetAllProductsEventArgs(IEnumerable<IProduct> products)
        {
            this._products = products;
        }

        public IEnumerable<IProduct> getList()
        {
            return _products;
        }
    }
}
