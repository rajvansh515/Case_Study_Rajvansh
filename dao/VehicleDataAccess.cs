using System;
using CarRentalSystem.dao.Interfaces;
using CarRentalSystem.Exceptions;
using CarRentalSystem.Model;
using System.Data.SqlClient;
namespace CarRentalSystem.dao
{
        public class VehicleDataAccess : IVehicleDataAccess
        {
            private readonly string _connectionString;

            public VehicleDataAccess(string connectionString)
            {
                _connectionString = connectionString;
            }

            public void AddVehicle(Vehicle vehicle)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(
                        "INSERT INTO Vehicle (make, model, year, dailyRate, status, passengerCapacity, engineCapacity) " +
                        "VALUES (@make, @model, @year, @dailyRate, @status, @passengerCapacity, @engineCapacity)", connection))
                    {
                        command.Parameters.AddWithValue("@make", vehicle.Make);
                        command.Parameters.AddWithValue("@model", vehicle.Model);
                        command.Parameters.AddWithValue("@year", vehicle.Year);
                        command.Parameters.AddWithValue("@dailyRate", vehicle.DailyRate);
                        command.Parameters.AddWithValue("@status", vehicle.Status);
                        command.Parameters.AddWithValue("@passengerCapacity", vehicle.PassengerCapacity);
                        command.Parameters.AddWithValue("@engineCapacity", vehicle.EngineCapacity);

                        command.ExecuteNonQuery();
                    }
                }
            }

        public void RemoveVehicle(int vehicleId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Check if the vehicle has any leases
                using (var checkLeaseCommand = new SqlCommand("SELECT COUNT(*) FROM Lease WHERE vehicleID = @vehicleId", connection))
                {
                    checkLeaseCommand.Parameters.AddWithValue("@vehicleId", vehicleId);
                    int leaseCount = (int)checkLeaseCommand.ExecuteScalar();

                    if (leaseCount > 0)
                    {
                        throw new InvalidOperationException("Cannot delete vehicle because it has existing leases.");
                    }
                }

                // If no leases exist, proceed with deletion
                using (var command = new SqlCommand("DELETE FROM Vehicle WHERE vehicleID = @vehicleId", connection))
                {
                    command.Parameters.AddWithValue("@vehicleId", vehicleId);
                    command.ExecuteNonQuery();
                }
            }
        }


        public List<Vehicle> GetAvailableVehicles()
            {
                var vehicles = new List<Vehicle>();
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT * FROM Vehicle WHERE status = 'available'", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                vehicles.Add(CreateVehicleFromReader(reader));
                            }
                        }
                    }
                }
                return vehicles;
            }

            public List<Vehicle> GetRentedVehicles()
            {
                var vehicles = new List<Vehicle>();
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT * FROM Vehicle WHERE status = 'notAvailable'", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                vehicles.Add(CreateVehicleFromReader(reader));
                            }
                        }
                    }
                }
                return vehicles;
            }

            public Vehicle GetVehicleById(int vehicleId)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT * FROM Vehicle WHERE vehicleID = @vehicleId", connection))
                    {
                        command.Parameters.AddWithValue("@vehicleId", vehicleId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return CreateVehicleFromReader(reader);
                            }
                        }
                    }
                }
                throw new CarNotFoundException($"Vehicle with ID {vehicleId} not found.");
            }

            public Vehicle CreateVehicleFromReader(SqlDataReader reader)
            {
                return new Vehicle(
                    reader.GetInt32(reader.GetOrdinal("vehicleID")),
                    reader.GetString(reader.GetOrdinal("make")),
                    reader.GetString(reader.GetOrdinal("model")),
                    reader.GetInt32(reader.GetOrdinal("year")),
                    reader.GetDecimal(reader.GetOrdinal("dailyRate")),
                    reader.GetString(reader.GetOrdinal("status")),
                    reader.GetInt32(reader.GetOrdinal("passengerCapacity")),
                    reader.GetString(reader.GetOrdinal("engineCapacity"))
                );
            }
        }
    }