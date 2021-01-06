using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.ServiceLayer;
using Model.ObjectModel;
using POS.View;

namespace POS.Controller
{
    public interface IProductsController
    {
        // view dependency injection
        IList<IProductsView> views { get; }

        // service layer dependency injection
        IProductsService service { get; set; }

        // data operations
        void newProduct(IProduct product);
        void deleteProduct(string productNumber);
    }

    public class ProductsController : IProductsController
    {
        // view dependency injection
        public IList<IProductsView> views { get; }

        /// <summary>
        /// Add a view to the observer list, and subscribe to its events.
        /// </summary>
        /// <param name="view"></param>
        public void addObserver(IProductsView view)
        {
            // subscribe to view events
            view.addProductRequest += V_addProductRequest;
            view.populateProductsRequest += V_populateRequest;
            view.deleteProductRequest += V_deleteProductRequest;

            // add the view to the list of observers.
            this.views.Add(view);
        }

        // service layer dependency injection
        public IProductsService service { get; set; }

        /// <summary>
        /// Default constructor. Instantiates the observer list, injects the service layer and subscribes to its events.
        /// </summary>
        public ProductsController()
        {
            this.views = new List<IProductsView>();

            // service layer dependency injection
            this.service = new ProductsService();

            // subscribe to service layer events
            this.service.GetAllProducts += Service_GetAllProducts;
        }

        /// <summary>
        /// Event handler for delete request from the view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void V_deleteProductRequest(object sender, DeleteProductRequestEventArgs args)
        {
            this.service.delete(args.getProductNumber());
        }

        /// <summary>
        /// Event handler for populate request from the view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void V_populateRequest(object sender, EventArgs e)
        {
            // ask the model for a list of all products.
            List<IProduct> allProducts = this.service.readAll().ToList();

            // update the views
            foreach (var v in views)
                v.populateProducts(allProducts);
        }

        /// <summary>
        /// Event handler for add new product request from the view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void V_addProductRequest(object sender, AddProductRequestEventArgs args)
        {
            // ask the model to add the new record
            this.service.create(args.getProduct());
        }

        /// <summary>
        /// Event handler for the service layer retrieving all product records.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Service_GetAllProducts(object sender, GetAllProductsEventArgs args)
        {
            // update the views
            foreach (var view in views)
                view.populateProducts(args.getList());
        }

        /// <summary>
        /// Create a new product record in the database.
        /// </summary>
        /// <param name="product"></param>
        public void newProduct(IProduct product)
        {
            // ask the model to do the processing
            this.service.create(product);
        }

        /// <summary>
        /// Delete a product record from the database.
        /// </summary>
        /// <param name="productNumber"></param>
        public void deleteProduct(string productNumber)
        {
            // ask the model to do the processing
            this.service.delete(productNumber);
        }
    }
}
