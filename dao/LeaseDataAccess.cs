using System;
using CarRentalSystem.dao.Interfaces;
using CarRentalSystem.Model;
using System.Data.SqlClient;
using System.Text;
using CarRentalSystem.Exceptions;

namespace CarRentalSystem.dao
{
    public class LeaseDataAccess : ILeaseDataAccess
    {
        private readonly string _connectionString;

        public LeaseDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Lease CreateLease(int customerId, int carId, DateTime startDate, DateTime endDate, string type)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Step 1: Check if the vehicle is available
                using (var checkVehicleCommand = new SqlCommand("SELECT status FROM Vehicle WHERE vehicleID = @vehicleId", connection))
                {
                    checkVehicleCommand.Parameters.AddWithValue("@vehicleId", carId);
                    string status = checkVehicleCommand.ExecuteScalar()?.ToString();

                    if (status != "available")
                    {
                        throw new InvalidOperationException("Vehicle is not available for lease.");
                    }
                }

                // Step 2: Insert the lease into the Lease table
                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO Lease (vehicleID, customerID, startDate, endDate, type) ");
                sql.Append("VALUES (@vehicleId, @customerId, @startDate, @endDate, @type); ");
                sql.Append("SELECT SCOPE_IDENTITY();");

                int leaseId;
                using (var command = new SqlCommand(sql.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@vehicleId", carId);
                    command.Parameters.AddWithValue("@customerId", customerId);
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);
                    command.Parameters.AddWithValue("@type", type);

                    leaseId = Convert.ToInt32(command.ExecuteScalar());
                }

                // Step 3: Update vehicle status to "notAvailable"
                using (var updateVehicleCommand = new SqlCommand("UPDATE Vehicle SET status = 'notAvailable' WHERE vehicleID = @vehicleId", connection))
                {
                    updateVehicleCommand.Parameters.AddWithValue("@vehicleId", carId);
                    updateVehicleCommand.ExecuteNonQuery();
                }

                // Return the newly created lease
                return new Lease(leaseId, carId, customerId, startDate, endDate, type);
            }
        }


        public void ReturnCar(int leaseId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(
                    "UPDATE Lease SET endDate = GETDATE() WHERE leaseID = @leaseId; " +
                    "UPDATE Vehicle SET status = 'available' WHERE vehicleID = (SELECT vehicleID FROM Lease WHERE leaseID = @leaseId);", connection))
                {
                    command.Parameters.AddWithValue("@leaseId", leaseId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Lease> GetActiveLeases()
        {
            var leases = new List<Lease>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Lease WHERE endDate > GETDATE()", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            leases.Add(CreateLeaseFromReader(reader));
                        }
                    }
                }
            }
            return leases;
        }

        public List<Lease> GetLeaseHistory()
        {
            var leases = new List<Lease>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Lease", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            leases.Add(CreateLeaseFromReader(reader));
                        }
                    }
                }
            }
            return leases;
        }

        public Lease CreateLeaseFromReader(SqlDataReader reader)
        {
            return new Lease(
                reader.GetInt32(reader.GetOrdinal("leaseID")),
                reader.GetInt32(reader.GetOrdinal("vehicleID")),
                reader.GetInt32(reader.GetOrdinal("customerID")),
                reader.GetDateTime(reader.GetOrdinal("startDate")),
                reader.GetDateTime(reader.GetOrdinal("endDate")),
                reader.GetString(reader.GetOrdinal("type"))
            );
        }
        public Lease GetLeaseById(int leaseId)
        {
            Lease lease = null;

            string query = "SELECT leaseID, vehicleID, customerID, startDate, endDate, type FROM Lease WHERE leaseID = @leaseId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@leaseId", leaseId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lease = CreateLeaseFromReader(reader);
                        }
                    }
                }
            }
            // If no lease was found, throw the LeaseNotFoundException
            if (lease == null)
            {
                throw new LeaseNotFoundException($"Lease with ID {leaseId} was not found.");
            }
            return lease;
        }

    }
}