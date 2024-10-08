using System;
using CarRentalSystem.dao.Interfaces;
using CarRentalSystem.Model;
namespace CarRentalSystem.Repository
{
    public class ICarLeaseRepositoryImpl : ICarLeaseRepository
    {
        private readonly IVehicleDataAccess _vehicleDataAccess;
        private readonly ICustomerDataAccess _customerDataAccess;
        private readonly ILeaseDataAccess _leaseDataAccess;
        private readonly IPaymentDataAccess _paymentDataAccess;
        public ICarLeaseRepositoryImpl(
            IVehicleDataAccess vehicleDataAccess,
            ICustomerDataAccess customerDataAccess,
            ILeaseDataAccess leaseDataAccess,
            IPaymentDataAccess paymentDataAccess)
        {
            _vehicleDataAccess = vehicleDataAccess;
            _customerDataAccess = customerDataAccess;
            _leaseDataAccess = leaseDataAccess;
            _paymentDataAccess = paymentDataAccess;
        }
        // Car Management 
        public void AddCar(Vehicle car)
        {
            _vehicleDataAccess.AddVehicle(car);
        }
        public void RemoveCar(int carId)
        {
            _vehicleDataAccess.RemoveVehicle(carId);
        }
        public List<Vehicle> ListAvailableCars()
        {
            return _vehicleDataAccess.GetAvailableVehicles();
        }
        public List<Vehicle> ListRentedCars()
        {
            return _vehicleDataAccess.GetRentedVehicles();
        }
        public Vehicle FindCarById(int carId)
        {
            return _vehicleDataAccess.GetVehicleById(carId);
        }
        // Customer Management
        public void AddCustomer(Customer customer)
        {
            _customerDataAccess.AddCustomer(customer);
        }
        public void RemoveCustomer(int customerId)
        {
            _customerDataAccess.RemoveCustomer(customerId);
        }
        public List<Customer> ListCustomers()
        {
            return _customerDataAccess.GetAllCustomers();
        }
        public Customer FindCustomerById(int customerId)
        {
            return _customerDataAccess.GetCustomerById(customerId);
        }
        //Leasemanagement
        public Lease CreateLease(int customerId, int carId, DateTime startDate, DateTime endDate)
        {
            // Determine lease type based on duration
            string leaseType = (endDate - startDate).TotalDays <= 30 ? "DailyLease" : "MonthlyLease";
            // Validate lease type against allowed values
            if (leaseType != "DailyLease" && leaseType != "MonthlyLease")
            {
                throw new ArgumentException("Invalid lease type. Must be either 'Daily' or 'Monthly'.");
            }
            return _leaseDataAccess.CreateLease(customerId, carId, startDate, endDate, leaseType);
        }

        public void ReturnCar(int leaseId)
        {
            _leaseDataAccess.ReturnCar(leaseId);
        }
        public List<Lease> ListActiveLeases()
        {
            return _leaseDataAccess.GetActiveLeases();
        }
        public List<Lease> ListLeaseHistory()
        {
            return _leaseDataAccess.GetLeaseHistory();
        }
        // Payment Handling
        public void RecordPayment(int leaseId, decimal amount)
        {
            _paymentDataAccess.RecordPayment(leaseId, amount);
        }
    }
}