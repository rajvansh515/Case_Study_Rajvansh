using System;
namespace CarRentalSystem.Model
{
    public class Lease
    {
        public int leaseID;
        public int vehicleID;
        public int customerID;
        public DateTime startDate;
        public DateTime endDate;
        public string type;

        public Lease(int leaseID, int vehicleID, int customerID, DateTime startDate, DateTime endDate, string type)
        {
            this.leaseID = leaseID;
            this.vehicleID = vehicleID;
            this.customerID = customerID;
            this.startDate = startDate;
            this.endDate = endDate;
            this.type = type;
        }

        // Getter and Setter for leaseID
        public int LeaseID
        {
            get { return leaseID; }
            set { leaseID = value; }
        }

        // Getter and Setter for vehicleID
        public int VehicleID
        {
            get { return vehicleID; }
            set { vehicleID = value; }
        }

        // Getter and Setter for customerID
        public int CustomerID
        {
            get { return customerID; }
            set { customerID = value; }
        }

        // Getter and Setter for startDate
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        // Getter and Setter for endDate
        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        // Getter and Setter for type
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}

