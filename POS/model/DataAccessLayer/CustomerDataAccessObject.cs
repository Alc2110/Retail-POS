using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using Model.ObjectModel;
using POS;

namespace Model.DataAccessLayer
{
    public interface ICustomerDataAccessObject
    {
        void addCustomer(ICustomer customer);
        ICustomer getCustomer(int id);
        IEnumerable<ICustomer> getAllCustomers();
        void deleteCustomer(int id);
        void updateCustomer(ICustomer customer);
    }

    public class CustomerDataAccessObject : ICustomerDataAccessObject
    {
        // connection string
        public string connString { get; set; }

        // default constructor
        // loads the connection string
        public CustomerDataAccessObject()
        {
            this.connString = Configuration.connectionString;
        }

        // constructor with parameter
        public CustomerDataAccessObject(string connString)
        {
            this.connString = connString;
        }

        /// <summary>
        /// Retrieve a customer record from the database.
        /// </summary>
        /// <param name="id">Customer id.</param>
        /// <returns>Customer object</returns>
        public ICustomer getCustomer(int id)
        {
            ICustomer customer = new Customer();

            string queryGetCustomer = "SELECT * From Customers " +
                                      "WHERE CustomerID = @id;";

            using (SqlConnection conn = new SqlConnection(this.connString))
            {
                // define the command object
                SqlCommand cmd = new SqlCommand(queryGetCustomer, conn);

                // parameterise
                SqlParameter idParam = new SqlParameter();
                idParam.ParameterName = "@id";
                idParam.Value = id;
                cmd.Parameters.Add(idParam);

                // try a connection
                conn.Open();

                // execute the query
                SqlDataReader reader = cmd.ExecuteReader();

                // check if results exist
                if (reader.HasRows)
                {
                    // results exist
                    while (reader.Read())
                    {
                        customer.customerId = reader.GetInt32(0);
                        customer.fullName = reader.GetString(1);
                        customer.address = reader.GetString(2);
                        customer.phoneNumber = reader.GetString(3);
                        customer.email = reader.GetString(4);
                        customer.city = reader.GetString(5);
                        States state;
                        if (Enum.TryParse(reader.GetString(6), out state))
                        {
                            customer.state = state;
                        }
                        else
                        {
                            // should never happen
                            throw new System.Exception("Invalid customer data");
                        }

                        customer.Postcode = reader.GetInt32(7);
                    }
                }
                else
                {
                    customer = null;
                }

                return customer;
            }
        }

        // this works
        /// <summary>
        /// Retrieves a list of all customer records in the database.
        /// </summary>
        /// <returns>Task List of customer objects.</returns>
        public IEnumerable<ICustomer> getAllCustomers()
        {
            string queryGetAllCustomers = "SELECT * From Customers;";

            using (SqlConnection conn = new SqlConnection(this.connString))
            {
                SqlCommand cmd = new SqlCommand(queryGetAllCustomers, conn);

                // try a connection
                conn.Open();

                // execute the query
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ICustomer customer = new Customer();
                        customer.customerId = reader.GetInt32(0);
                        customer.fullName = reader.GetString(1);
                        customer.address = reader.GetString(2);
                        customer.phoneNumber = reader.GetString(3);
                        customer.email = reader.GetString(4);
                        customer.city = reader.GetString(5);
                        States state;
                        if (Enum.TryParse(reader.GetString(6), out state))
                        {
                            customer.state = state;
                        }
                        else
                        {
                            // should never happen
                            throw new Exception("Invalid customer data");
                        }
                        customer.Postcode = reader.GetInt32(7);

                        yield return customer;
                    }
                }
            }
        }

        /// <summary>
        /// Delete a customer record from the database.
        /// </summary>
        /// <param name="customer">Customer object</param>
        public void deleteCustomer(int id)
        {
            // CustomerID in the database is the PK
            // TODO: parameterise this!
            string queryDeleteCustomer = "DELETE FROM Customers WHERE CustomerID = " + id + ";";

            using (SqlConnection conn = new SqlConnection(this.connString))
            {
                SqlCommand cmd = new SqlCommand(queryDeleteCustomer, conn);

                // try a connection
                conn.Open();

                // execute the query
                cmd.ExecuteNonQuery();
            }

            return;
        }

        /// <summary>
        /// Add a customer record to the database.
        /// </summary>
        /// <param name="customer">Customer object.</param>
        public void addCustomer(ICustomer customer)
        {
            // CustomerID in the database is PK and AI
            string queryAddCustomer = "INSERT INTO Customers (FullName, StreetAddress, PhoneNumber, Email, City, State_, Postcode) " +
                                      "VALUES (@name, @address, @number, @email, @city, @state, @postcode);";

            using (SqlConnection conn = new SqlConnection(this.connString))
            {
                SqlCommand cmd = new SqlCommand(queryAddCustomer, conn);

                // parameterise
                SqlParameter nameParam = new SqlParameter();
                nameParam.ParameterName = "@name";
                nameParam.Value = customer.fullName;
                cmd.Parameters.Add(nameParam);

                SqlParameter addressParam = new SqlParameter();
                addressParam.ParameterName = "@address";
                addressParam.Value = customer.address;
                cmd.Parameters.Add(addressParam);

                SqlParameter numberParam = new SqlParameter();
                numberParam.ParameterName = "@number";
                numberParam.Value = customer.phoneNumber;
                cmd.Parameters.Add(numberParam);

                SqlParameter emailParam = new SqlParameter();
                emailParam.ParameterName = "@email";
                emailParam.Value = customer.email;
                cmd.Parameters.Add(emailParam);

                SqlParameter cityParam = new SqlParameter();
                cityParam.ParameterName = "@city";
                cityParam.Value = customer.city;
                cmd.Parameters.Add(cityParam);

                SqlParameter stateParam = new SqlParameter();
                stateParam.ParameterName = "@state";
                stateParam.Value = customer.state.ToString();
                cmd.Parameters.Add(stateParam);

                SqlParameter postcodeParam = new SqlParameter();
                postcodeParam.ParameterName = "@postcode";
                postcodeParam.Value = customer.Postcode;
                cmd.Parameters.Add(postcodeParam);

                // try a connection
                conn.Open();

                // execute the query
                cmd.ExecuteNonQuery();
            }
        }

        // this works
        /// <summary>
        /// Update a customer record in the database.
        /// </summary>
        /// <param name="customer"></param>
        public void updateCustomer(ICustomer customer)
        {
            // CustomerID in the database is PK and AI
            string queryUpdateCustomer = "UPDATE Customers " +
                                         "SET FullName = @name, StreetAddress = @address, PhoneNumber = @number, Email = @email, City = @city, State_ = @state, Postcode = @Postcode " +
                                         "WHERE CustomerID = @id;";

            using (SqlConnection conn = new SqlConnection(this.connString))
            {
                SqlCommand cmd = new SqlCommand(queryUpdateCustomer, conn);

                // parameterise
                SqlParameter idParam = new SqlParameter();
                idParam.ParameterName = "@id";
                idParam.Value = customer.customerId;
                cmd.Parameters.Add(idParam);

                SqlParameter nameParam = new SqlParameter();
                nameParam.ParameterName = "@name";
                nameParam.Value = customer.fullName;
                cmd.Parameters.Add(nameParam);

                SqlParameter addressParam = new SqlParameter();
                addressParam.ParameterName = "@address";
                addressParam.Value = customer.address;
                cmd.Parameters.Add(addressParam);

                SqlParameter numberParam = new SqlParameter();
                numberParam.ParameterName = "@number";
                numberParam.Value = customer.phoneNumber;
                cmd.Parameters.Add(numberParam);

                SqlParameter emailParam = new SqlParameter();
                emailParam.ParameterName = "@email";
                emailParam.Value = customer.email;
                cmd.Parameters.Add(emailParam);

                SqlParameter cityParam = new SqlParameter();
                cityParam.ParameterName = "@city";
                cityParam.Value = customer.city;
                cmd.Parameters.Add(cityParam);

                SqlParameter stateParam = new SqlParameter();
                stateParam.ParameterName = "@state";
                stateParam.Value = customer.state.ToString();
                cmd.Parameters.Add(stateParam);

                SqlParameter postcodeParam = new SqlParameter();
                postcodeParam.ParameterName = "@postcode";
                postcodeParam.Value = customer.Postcode.ToString();
                cmd.Parameters.Add(postcodeParam);

                // try a connection
                conn.Open();

                // execute the query
                cmd.ExecuteNonQuery();
            }
        }
    }
}
