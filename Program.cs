using CarRentalSystem.dao;
using CarRentalSystem.dao.Interfaces;
using CarRentalSystem.Repository;
using CarRentalSystem.Util;

namespace CarRentalSystem;
class Program
{
    static void Main(string[] args)
    {
        string connectionString = DbConnUtil.GetConnString();

        IVehicleDataAccess vehicleDataAccess = new VehicleDataAccess(connectionString);
        ICustomerDataAccess customerDataAccess = new CustomerDataAccess(connectionString);
        ILeaseDataAccess leaseDataAccess = new LeaseDataAccess(connectionString);
        IPaymentDataAccess paymentDataAccess = new PaymentDataAccess(connectionString);

      
        ICarLeaseRepository repository = new ICarLeaseRepositoryImpl(
            vehicleDataAccess,
            customerDataAccess,
            leaseDataAccess,
            paymentDataAccess
        );

        
        MainModule mainModule = new MainModule(repository);
        mainModule.Run();
    }
}