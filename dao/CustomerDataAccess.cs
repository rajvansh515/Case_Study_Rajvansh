using System;
using CarRentalSystem.dao.Interfaces;
using CarRentalSystem.Exceptions;
using CarRentalSystem.Model;
using System.Data.SqlClient;
namespace CarRentalSystem.dao
{
    public class CustomerDataAccess : ICustomerDataAccess
    {
        private readonly string _connectionString;

        public CustomerDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddCustomer(Customer customer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(
                    "INSERT INTO Customer (firstName, lastName, email, phoneNumber) " +
                    "VALUES (@firstName, @lastName, @email, @phoneNumber)", connection))
                {
                    command.Parameters.AddWithValue("@firstName", customer.FirstName);
                    command.Parameters.AddWithValue("@lastName", customer.LastName);
                    command.Parameters.AddWithValue("@email", customer.Email);
                    command.Parameters.AddWithValue("@phoneNumber", customer.PhoneNumber);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void RemoveCustomer(int customerId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("DELETE FROM Customer WHERE customerID = @customerId", connection))
                    {
                        command.Parameters.AddWithValue("@customerId", customerId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547) // Foreign key violation (e.g., active leases)
                {
                    throw new InvalidOperationException("Cannot delete the customer because they have active leases.");
                }
                throw; // Re-throw if it's a different SQL exception
            }
        }

        public List<Customer> GetAllCustomers()
        {
            var customers = new List<Customer>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Customer", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(CreateCustomerFromReader(reader));
                        }
                    }
                }
            }
            return customers;
        }

        public Customer GetCustomerById(int customerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Customer WHERE customerID = @customerId", connection))
                {
                    command.Parameters.AddWithValue("@customerId", customerId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return CreateCustomerFromReader(reader);
                        }
                    }
                }
            }
            throw new CustomerNotFoundException($"Customer with ID {customerId} not found.");
        }

        public Customer CreateCustomerFromReader(SqlDataReader reader)
        {
            return new Customer(
                reader.GetInt32(reader.GetOrdinal("customerID")),
                reader.GetString(reader.GetOrdinal("firstName")),
                reader.GetString(reader.GetOrdinal("lastName")),
                reader.GetString(reader.GetOrdinal("email")),
                reader.GetString(reader.GetOrdinal("phoneNumber"))
            );
        }
    }
}