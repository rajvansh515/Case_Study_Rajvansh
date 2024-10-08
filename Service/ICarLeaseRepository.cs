using System;
using CarRentalSystem.Model;

namespace CarRentalSystem.Repository
{
    public interface ICarLeaseRepository
    {
        // Car Management
        void AddCar(Vehicle car);
        void RemoveCar(int carId);
        List<Vehicle> ListAvailableCars();
        List<Vehicle> ListRentedCars();
        Vehicle FindCarById(int carId);

        // Customer Management
        void AddCustomer(Customer customer);
        void RemoveCustomer(int customerId);
        List<Customer> ListCustomers();
        Customer FindCustomerById(int customerId);
        
        // Lease Management
        Lease CreateLease(int customerId, int carId, DateTime startDate, DateTime endDate);
        void ReturnCar(int leaseId);
        List<Lease> ListActiveLeases();
        List<Lease> ListLeaseHistory();

        // Payment Handling
        void RecordPayment(int leaseId, decimal amount);
    }
}