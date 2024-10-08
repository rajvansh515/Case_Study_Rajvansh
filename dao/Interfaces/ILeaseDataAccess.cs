using System;
using CarRentalSystem.Model;
namespace CarRentalSystem.dao.Interfaces
{
    public interface ILeaseDataAccess
    {
        Lease CreateLease(int customerId, int vehicleId, DateTime startDate, DateTime endDate, string type);
        void ReturnCar(int leaseId);
        List<Lease> GetActiveLeases();
        List<Lease> GetLeaseHistory();
        Lease GetLeaseById(int leaseId);
    }
}