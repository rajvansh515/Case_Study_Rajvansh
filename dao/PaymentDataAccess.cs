using System;
using CarRentalSystem.dao.Interfaces;
using CarRentalSystem.Model;
using System.Data.SqlClient;

namespace CarRentalSystem.dao
{
    public class PaymentDataAccess : IPaymentDataAccess
    {
        private readonly string _connectionString;

        public PaymentDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void RecordPayment(int leaseId, decimal amount)
        {
            if (leaseId <= 0)
                throw new ArgumentException("Invalid lease ID", nameof(leaseId));
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero", nameof(amount));

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(
                    @"INSERT INTO Payment (LeaseID, PaymentDate, Amount) VALUES (@leaseId, @paymentDate, @amount)", connection))
                {
                    command.Parameters.AddWithValue("@leaseId", leaseId);
                    command.Parameters.AddWithValue("@paymentDate", DateTime.UtcNow);
                    command.Parameters.AddWithValue("@amount", amount);
                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                            throw new InvalidOperationException("Payment record was not inserted");
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"SQL Error: {ex.Message}");
                        throw new InvalidOperationException("An error occurred while recording the payment", ex);
                    }
                }
            }
        }

        public List<Payment> GetPaymentHistory(int customerId)
        {
            var payments = new List<Payment>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(
                    "SELECT p.* FROM Payment p " +
                    "JOIN Lease l ON p.leaseID = l.leaseID " +
                    "WHERE l.customerID = @customerId", connection))
                {
                    command.Parameters.AddWithValue("@customerId", customerId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            payments.Add(CreatePaymentFromReader(reader));
                        }
                    }
                }
            }
            return payments;
        }

        public decimal GetTotalRevenue()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT SUM(amount) FROM Payment", connection))
                {
                    var result = command.ExecuteScalar();
                    return result == DBNull.Value ? 0 : Convert.ToDecimal(result);
                }
            }
        }

        private Payment CreatePaymentFromReader(SqlDataReader reader)
        {
            return new Payment(
                reader.GetInt32(reader.GetOrdinal("paymentID")),
                reader.GetInt32(reader.GetOrdinal("leaseID")),
                reader.GetDateTime(reader.GetOrdinal("paymentDate")),
                reader.GetInt32(reader.GetOrdinal("amount")));
        }
    }
}
