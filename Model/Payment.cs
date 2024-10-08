using System;
namespace CarRentalSystem.Model
{
    public class Payment
    {
        public int paymentID;
        public int leaseID;
        public DateTime paymentDate;
        public decimal amount;

        public Payment(int paymentID, int leaseID, DateTime paymentDate, decimal amount)
        {
            this.paymentID = paymentID;
            this.leaseID = leaseID;
            this.paymentDate = paymentDate;
            this.amount = amount;
        }

        // Getter and Setter for paymentID
        public int PaymentID
        {
            get { return paymentID; }
            set { paymentID = value; }
        }

        // Getter and Setter for leaseID
        public int LeaseID
        {
            get { return leaseID; }
            set { leaseID = value; }
        }

        // Getter and Setter for paymentDate
        public DateTime PaymentDate
        {
            get { return paymentDate; }
            set { paymentDate = value; }
        }

        // Getter and Setter for amount
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }
    }
}

