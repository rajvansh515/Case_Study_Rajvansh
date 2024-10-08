using System;
using CarRentalSystem.Model;
namespace CarRentalSystem.dao.Interfaces
{
    public interface IPaymentDataAccess
    {
        void RecordPayment(int leaseId, decimal amount);
        List<Payment> GetPaymentHistory(int customerId);
        decimal GetTotalRevenue();
    }
}