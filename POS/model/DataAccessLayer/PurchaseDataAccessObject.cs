using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using Model.ObjectModel;
using POS;

namespace Model.DataAccessLayer
{
    public interface IPurchaseDataAccessObject
    {
        IEnumerable<IPurchase> getAllTransactions();
        void addTransaction(IPurchase transaction);
    }

    public class PurchaseDataAccessObject : IPurchaseDataAccessObject
    {
        // connection string
        public string connString { get; set; }

        // default constructor
        // loads the connection string
        public PurchaseDataAccessObject()
        {
            this.connString = Configuration.connectionString;
        }

        // constructor with parameter
        public PurchaseDataAccessObject(string connString)
        {
            this.connString = connString;
        }

        /// <summary>
        /// Retrieve all transactions in the database.
        /// </summary>
        /// <returns>Task List of Transactions</returns>
        public IEnumerable<IPurchase> getAllTransactions()
        {
            string getAllTransactionsQuery = "SELECT Transactions.TransactionID, Transactions.Timestamp_," +
                                             "Customers.CustomerID, Customers.FullName, Customers.PhoneNumber, Customers.Email, Customers.StreetAddress, Customers.State_, Customers.Postcode," +
                                             "Staff.StaffID, Staff.FullName, Staff.PasswordHash, Staff.Privelege, " +
                                             "Products.ProductID, Products.ProductIDNumber, Products.Description_, Products.Price " +
                                             "FROM(((Transactions " +
                                              " FULL JOIN Customers ON Transactions.CustomerID = Customers.CustomerID)" +
                                              " INNER JOIN Products ON Transactions.ProductID = Products.ProductID)" +
                                              " INNER JOIN Staff ON Transactions.StaffID = Staff.StaffID);";

            using (SqlConnection conn = new SqlConnection(this.connString))
            {
                // prepare the command
                SqlCommand cmd = new SqlCommand(getAllTransactionsQuery, conn);

                // try a connection
                conn.Open();

                // execute the query
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Purchase transaction = new Purchase();
                    transaction.purchaseId = reader.GetInt32(0);
                    transaction.timestamp = reader.GetDateTime(1).ToString();

                    Product transactionProduct = new Product();
                    transactionProduct.productId = reader.GetInt32(13);
                    transactionProduct.productNumber = reader.GetString(14);
                    transactionProduct.description = reader.GetString(15);

                    // an SQL float is a .NET double
                    double productPrice = reader.GetDouble(16);
                    //transactionProduct.setPrice(Convert.ToSingle(productPrice));
                    transactionProduct.price = Convert.ToSingle(productPrice);

                    Staff transactionStaff = new Staff();
                    transactionStaff.staffID = reader.GetInt32(9);
                    transactionStaff.fullName = reader.GetString(10);
                    transactionStaff.passwordHash = reader.GetString(11);
                    switch (reader.GetString(12))
                    {
                        case "Admin":
                            transactionStaff.privelege = Privelege.Admin;

                            break;

                        case "Normal":
                            transactionStaff.privelege = Privelege.Normal;

                            break;

                        default:
                            // this shouldn't happen, data validation techniques should prevent it
                            throw new System.IO.InvalidDataException("Found entry with invalid staff privelege level in database.");
                    }

                    Customer transactionCustomer = new Customer();
                    // check if Customer exists for this transaction
                    if (!reader.IsDBNull(2))
                    {
                        transactionCustomer.customerId = reader.GetInt32(2);
                        transactionCustomer.fullName = reader.GetString(3);
                        transactionCustomer.phoneNumber = reader.GetString(4);
                        transactionCustomer.email = reader.GetString(5);
                        transactionCustomer.address = reader.GetString(6);
                        States state;
                        if (Enum.TryParse(reader.GetString(7), out state))
                        {
                            transactionCustomer.state = state;
                        }
                        else
                        {
                            // this shouldn't happen, data validation techniques should prevent it
                            throw new System.IO.InvalidDataException("Found invalid entry for field 'State' in database.");
                        }
                    }
                    else
                    {
                        // no customer
                        transactionCustomer = null;
                    }

                    transaction.staff = transactionStaff;
                    transaction.product = transactionProduct;
                    transaction.customer = transactionCustomer;

                    yield return transaction;
                }
            }
        }

        /// <summary>
        /// Add a transaction to the database.
        /// </summary>
        /// <param name="transaction">Transaction interface.</param>
        public void addTransaction(IPurchase transaction)
        {
            // TODO: figure out a way of putting the timestamp from the transaction object into the query
            // prepare the query
            string query = "INSERT INTO Transactions (Timestamp_, CustomerID, StaffID, ProductID)" +
                           "VALUES (SYSDATETIME(), @customerID, @staffID, @productID);" +
                           "\n" +
                           "DECLARE @Quantity int;" +
                           "SET @Quantity = ((SELECT Products.Quantity FROM Products WHERE Products.ProductID = @productID) - 1);" +
                           "UPDATE Products SET Products.Quantity = @Quantity WHERE Products.ProductID = @productID;";

            using (SqlConnection conn = new SqlConnection(this.connString))
            {
                // try a connection
                conn.Open();

                // prepare the ADO.NET transaction
                SqlTransaction sqlTrans;

                // prepare the command 
                SqlCommand cmd = new SqlCommand(query, conn);

                // parameterise
                SqlParameter customerIDParam = new SqlParameter();
                if (transaction.customer != null)
                {
                    customerIDParam.Value = transaction.customer.customerId;
                }
                else
                {
                    customerIDParam.Value = DBNull.Value;
                }
                customerIDParam.ParameterName = "@customerID";
                cmd.Parameters.Add(customerIDParam);

                SqlParameter staffIDParam = new SqlParameter();
                staffIDParam.Value = transaction.staff.staffID;
                staffIDParam.ParameterName = "@staffID";
                cmd.Parameters.Add(staffIDParam);

                SqlParameter productIDParam = new SqlParameter();
                productIDParam.Value = transaction.product.productId;
                productIDParam.ParameterName = "@productID";
                cmd.Parameters.Add(productIDParam);

                // begin the transaction
                sqlTrans = conn.BeginTransaction();

                cmd.Transaction = sqlTrans;

                try
                {
                    // execute the query
                    cmd.ExecuteNonQuery();

                    // commit the transaction
                    sqlTrans.Commit();
                }
                catch (SqlException ex)
                {
                    // transaction failed

                    // roll it back
                    sqlTrans.Rollback();

                    // also throw it up to the UI, so it can inform the user
                    throw;
                }
            }
        }
    }
}
