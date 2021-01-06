using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Sql;
using Model.ObjectModel;
using POS;

namespace Model.DataAccessLayer
{
    public interface IProductDataAccessObject
    {
        void create(IProduct product);
        IProduct read(string productNumber);
        void update(IProduct product);
        void delete(string productNumber);
        IEnumerable<IProduct> readAll();
    }

    public class ProductDataAccessObject : IProductDataAccessObject
    {
        private string _connString { get; set; }

        /// <summary>
        /// Default constructor. Gets connection string from configuration.
        /// </summary>
        public ProductDataAccessObject()
        {
            this._connString = Configuration.connectionString;
        }

        /// <summary>
        /// Constructor with connection string parameter.
        /// </summary>
        /// <param name="connString"></param>
        public ProductDataAccessObject(string connString)
        {
            this._connString = connString;
        }

        /// <summary>
        /// Create a product record in the database.
        /// </summary>
        /// <param name="product"></param>
        public void create(IProduct product)
        {
            string queryAddProduct = "INSERT INTO Products (ProductIDNumber,Description_,Quantity,Price) " +
                                     "VALUES (@idNumber, @description, @quantity, @price);";

            using (SqlConnection conn = new SqlConnection(this._connString))
            {
                SqlCommand cmd = new SqlCommand(queryAddProduct, conn);

                // parameterise
                SqlParameter idNumberParam = new SqlParameter();
                idNumberParam.ParameterName = "@idNumber";
                idNumberParam.Value = product.productNumber;
                cmd.Parameters.Add(idNumberParam);

                SqlParameter descParam = new SqlParameter();
                descParam.ParameterName = "@description";
                descParam.Value = product.description;
                cmd.Parameters.Add(descParam);

                SqlParameter quantityParam = new SqlParameter();
                quantityParam.ParameterName = "@quantity";
                quantityParam.Value = product.quantity;
                cmd.Parameters.Add(quantityParam);

                SqlParameter priceParam = new SqlParameter();
                priceParam.ParameterName = "@price";
                priceParam.Value = product.price;
                cmd.Parameters.Add(priceParam);

                // attempt a connection
                conn.Open();

                // execute the query
                cmd.ExecuteNonQuery();
            }

            return;
        }

        /// <summary>
        /// Retrieve a product record, using its item number.
        /// </summary>
        /// <param name="productNumber"></param>
        /// <returns></returns>
        public IProduct read(string productNumber)
        {
            IProduct product = new Product();

            string queryGetProduct = "SELECT * FROM Products " +
                                     "WHERE ProductIDNumber = @id;";

            using (SqlConnection conn = new SqlConnection(this._connString))
            {
                SqlCommand cmd = new SqlCommand(queryGetProduct, conn);

                // parameterise
                SqlParameter idParam = new SqlParameter();
                idParam.ParameterName = "@id";
                idParam.Value = productNumber;
                cmd.Parameters.Add(idParam);

                SqlDataReader reader;

                // attempt a connection
                conn.Open();

                // execute the query
                reader = cmd.ExecuteReader();

                // check if results exist
                if (reader.HasRows)
                {
                    // results exist
                    while (reader.Read())
                    {
                        product.productId = reader.GetInt32(0);
                        product.productNumber = reader.GetString(1);
                        product.description = reader.GetString(2);
                        product.quantity = reader.GetInt32(3);
                        // an SQL float is a .NET double
                        double dprice = reader.GetDouble(4);
                        float fprice = Convert.ToSingle(dprice);
                        product.price = fprice;
                    }
                }
                else
                {
                    product = null;
                }

                return product;
            }
        }

        /// <summary>
        /// Update a product record in the database.
        /// </summary>
        /// <param name="product"></param>
        public void update(IProduct product)
        {
            string queryUpdateProduct = "UPDATE Products " +
                                        "SET ProductIDNumber = @idNumber, Description_ = @desc, Quantity = @quantity, Price = @price " +
                                        "WHERE ProductID = " + product.productId + ";";

            using (SqlConnection conn = new SqlConnection(this._connString))
            {
                SqlCommand cmd = new SqlCommand(queryUpdateProduct, conn);

                // paramterise
                SqlParameter idNumParam = new SqlParameter();
                idNumParam.ParameterName = "@idNumber";
                idNumParam.Value = product.productNumber;
                cmd.Parameters.Add(idNumParam);

                SqlParameter descParam = new SqlParameter();
                descParam.ParameterName = "@desc";
                descParam.Value = product.description;
                cmd.Parameters.Add(descParam);

                SqlParameter quantParam = new SqlParameter();
                quantParam.ParameterName = "@quantity";
                quantParam.Value = product.quantity;
                cmd.Parameters.Add(quantParam);

                SqlParameter priceParam = new SqlParameter();
                priceParam.ParameterName = "@price";
                priceParam.Value = product.price;
                cmd.Parameters.Add(priceParam);

                // attempt a connection
                conn.Open();

                // execute the query
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Delete product record, using its item number.
        /// </summary>
        /// <param name="productNumber"></param>
        public void delete(string productNumber)
        {
            string queryDeleteProduct = "DELETE FROM Products " +
                                        "WHERE ProductIDNumber" +
                                        "" +
                                        " = @idNumber;";

            using (SqlConnection conn = new SqlConnection(this._connString))
            {
                SqlCommand cmd = new SqlCommand(queryDeleteProduct, conn);

                // parameterise
                SqlParameter idParam = new SqlParameter();
                idParam.ParameterName = "@idNumber";
                idParam.Value = productNumber;
                cmd.Parameters.Add(idParam);

                // attempt a connection
                conn.Open();

                // execute the query
                cmd.ExecuteNonQuery();
            }

            return;
        }

        /// <summary>
        /// Get all product records.
        /// </summary>
        /// <returns>IEnumerable of all product records.</returns>
        public IEnumerable<IProduct> readAll()
        {
            string queryGetAllProducts = "SELECT * FROM Products;";

            using (SqlConnection conn = new SqlConnection(this._connString))
            {
                SqlCommand cmd = new SqlCommand(queryGetAllProducts, conn);

                // try a connection
                conn.Open();

                // execute the query
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        IProduct product = new Product();
                        product.productId = reader.GetInt32(0);
                        product.productNumber = reader.GetString(1);
                        product.description = reader.GetString(2);
                        product.quantity = reader.GetInt32(3);
                        // a SQL float is a .NET double
                        double dprice = reader.GetDouble(4);
                        float fprice = Convert.ToSingle(dprice);
                        product.price = fprice;

                        yield return product;
                    }
                }
                else
                {
                    yield break;
                }
            }
        }
    }
}
