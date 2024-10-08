using System;
namespace CarRentalSystem.Model
{
    public class Customer
    {
        public int customerID;
        public string firstName;
        public string lastName;
        public string email;
        public string phoneNumber;

        public Customer(int customerID, string firstName, string lastName, string email, string phoneNumber)
        {
            this.customerID = customerID;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.phoneNumber = phoneNumber;
        }

        // Getter and Setter for customerID
        public int CustomerID
        {
            get { return customerID; }
            set { customerID = value; }
        }

        // Getter and Setter for firstName
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        // Getter and Setter for lastName
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        // Getter and Setter for email
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        // Getter and Setter for phoneNumber
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }
    }
}