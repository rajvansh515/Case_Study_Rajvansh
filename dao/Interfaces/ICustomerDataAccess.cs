using System;
using CarRentalSystem.Model;
namespace CarRentalSystem.dao.Interfaces
{
    public interface ICustomerDataAccess
    {
        void AddCustomer(Customer customer);
        void RemoveCustomer(int customerId);
        List<Customer> GetAllCustomers();
        Customer GetCustomerById(int customerId);
    }
}