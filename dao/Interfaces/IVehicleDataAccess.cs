using System;
using CarRentalSystem.Model;
namespace CarRentalSystem.dao.Interfaces
{
    public interface IVehicleDataAccess
    {
        void AddVehicle(Vehicle vehicle);
        void RemoveVehicle(int vehicleId);
        List<Vehicle> GetAvailableVehicles();
        List<Vehicle> GetRentedVehicles();
        Vehicle GetVehicleById(int vehicleId);
    }
}