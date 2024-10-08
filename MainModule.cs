
using CarRentalSystem.Model;
using CarRentalSystem.Repository;
using System.Transactions;

namespace CarRentalSystem
{
    public class MainModule
    {
        private ICarLeaseRepository repository;
        public MainModule(ICarLeaseRepository repository)
        {
            this.repository = repository;
        }

        public void Run()
        {
            bool exitProgram = false;

            while (!exitProgram)
            {
                DisplayMenu();
                string choice = Console.ReadLine();

                try
                {
                    ProcessUserChoice(choice, ref exitProgram);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"error : {ex.Message}");
                }

                Console.WriteLine("\ncontinue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
        private void ProcessUserChoice(string choice, ref bool exitProgram)
        {
            switch (choice)
            {
                case "1":
                    AddNewCar();
                    break;
                case "2":
                    ListAvailableCars();
                    break;
                case "3":
                    AddNewCustomer();
                    break;
                case "4":
                    ListCustomers();
                    break;
                case "5":
                    CreateNewLease();
                    break;
                case "6":
                    ReturnCar();
                    break;
                case "7":
                    RecordPayment();
                    break;
                case "8":
                    ViewLeaseHistory();
                    break;
                case "9":
                    ListRentedCars();
                    break;
                case "10":
                    RemoveCar();
                    break;
                case "11":
                    RemoveCustomer();
                    break;
                case "12":
                    ViewActiveLeases();
                    break;
                case "13":
                    exitProgram = true;
                    Console.WriteLine("Car Rental System. Thanks!!");
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        public void DisplayMenu()
        {
            Console.WriteLine("Car Rental System");
            Console.WriteLine("1. Add car");
            Console.WriteLine("2. Retreive the list of all available cars");
            Console.WriteLine("3. Add customer");
            Console.WriteLine("4. Retreive the list of all customers");
            Console.WriteLine("5. Create lease for renting a car");
            Console.WriteLine("6. Cancel a lease after use");
            Console.WriteLine("7. Record a payment for lease");
            Console.WriteLine("8. Retreive and view lease history");
            Console.WriteLine("9. Retreive the list of all rented cars");
            Console.WriteLine("10. Remove car from the system");
            Console.WriteLine("11. Remove customer from the system");
            Console.WriteLine("12. Retreive and view all active leases");
            Console.WriteLine("13. Exit");
            Console.WriteLine(new string('-', 5));
            Console.Write("Choice: ");
        }

        public void AddNewCar()
        {
            Console.WriteLine("Add Car");
            Console.Write("Make: ");
            string make = Console.ReadLine();
            Console.Write("Model: ");
            string model = Console.ReadLine();
            Console.Write("Year: ");
            int year = int.Parse(Console.ReadLine());
            Console.Write("Daly Rate: ");
            decimal dailyRate = decimal.Parse(Console.ReadLine());
            Console.Write("Passenger Capacity: ");
            int passengerCapacity = int.Parse(Console.ReadLine());
            Console.Write("Engine Capacity: ");
            string engineCapacity = Console.ReadLine();
            Vehicle newCar = new Vehicle(0, make, model, year, dailyRate, "available", passengerCapacity, engineCapacity);
            repository.AddCar(newCar);
            Console.WriteLine("\ncar added:");
            Console.WriteLine($"Make: {make}, Model: {model}, Year: {year}");
            Console.WriteLine($"Daily Rate: {dailyRate:C}, Capacity: {passengerCapacity}, Engine: {engineCapacity}");
        }

        public void ListAvailableCars()
        {
            Console.WriteLine("All Available Cars");
            List<Vehicle> availableCars = repository.ListAvailableCars();
            if (availableCars.Any())
            {
                Console.WriteLine($"{"ID",-5}{"Make",-15}{"Model",-15}{"Year",-12}{"Daily Rate",-15}{"Engine Capacity",-10}");

                foreach (var car in availableCars)
                {
                    Console.WriteLine($"{car.VehicleID,-5}{car.Make,-15}{car.Model,-15}{car.Year,-6}{car.DailyRate,15:C2}{car.PassengerCapacity,10}{car.EngineCapacity,-15}");
                }
            }
            else
            {
                Console.WriteLine("No car available");
            }
        }

        public void AddNewCustomer()
        {
            Console.WriteLine("👤 Add Customer");
            Console.Write("First name: ");
            string firstName = Console.ReadLine();
            Console.Write("Last name: ");
            string lastName = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("Phone number: ");
            string phoneNumber = Console.ReadLine();
            Customer newCustomer = new Customer(0, firstName, lastName, email, phoneNumber);
            repository.AddCustomer(newCustomer);
            Console.WriteLine("\nNew customer added:");
            Console.WriteLine($"Name: {firstName} {lastName}");
            Console.WriteLine($"Email: {email}, Phone: {phoneNumber}");
        }

        public void ListCustomers()
        {
            Console.WriteLine("👥 Customer List");
            List<Customer> customers = repository.ListCustomers();
            if (customers.Any())
            {
                Console.WriteLine($"{"ID",-5}{"Name",-30}{"Email",-25}{"Phone",-20}");
                foreach (var customer in customers)
                {
                    Console.WriteLine($"{customer.CustomerID,-5}{customer.FirstName + " " + customer.LastName,-30}{customer.Email,-25}{customer.PhoneNumber,-20}");
                }
            }
            else
            {
                Console.WriteLine("No customers found");
            }
        }

        public void CreateNewLease()
        {
            Console.WriteLine("Create New Lease");
            try
            {
                // Gather lease information
                Console.Write("Enter customer ID: ");
                int customerId = int.Parse(Console.ReadLine());
                Console.Write("Enter car ID: ");
                int carId = int.Parse(Console.ReadLine());
                Console.Write("Enter start date (yyyy-mm-dd): ");
                DateTime startDate = DateTime.Parse(Console.ReadLine());
                Console.Write("Enter end date (yyyy-mm-dd): ");
                DateTime endDate = DateTime.Parse(Console.ReadLine());

                // Calculate lease amount
                Vehicle car = repository.FindCarById(carId);
                if (car == null)
                {
                    throw new ArgumentException("Invalid car ID.");
                }
                int numberOfDays = (int)(endDate - startDate).TotalDays + 1;
                decimal totalAmount = car.DailyRate * numberOfDays;

                Console.WriteLine($"\nTotal lease amount: {totalAmount:F2}");

                // Get payment method
                Console.WriteLine("Select payment method:");
                Console.WriteLine("1.Bhim UPI");
                Console.WriteLine("2. Card");
                Console.WriteLine("3. Cash on Delivery");
                Console.Write("Enter choice: ");
                int paymentChoice = int.Parse(Console.ReadLine());
                string paymentMethod = paymentChoice switch
                {
                    1 => "Bhim UPI",
                    2 => "Card",
                    3 => "Cash on delivery",
                    _ => throw new ArgumentException("Invalid.")
                };

                // Display lease details and confirm payment
                Console.WriteLine("\nLease Details:");
                Console.WriteLine($"Customer ID: {customerId}");
                Console.WriteLine($"Car ID: {carId}");
                Console.WriteLine($"Start Date: {startDate:d}");
                Console.WriteLine($"End Date: {endDate:d}");
                Console.WriteLine($"Total Amount: {totalAmount:F2}");
                Console.WriteLine($"Payment Method: {paymentMethod}");

                Console.Write("Confirm payment with lease creation (Y/N): ");
                if (Console.ReadLine().Trim().ToUpper() != "Y")
                {
                    Console.WriteLine("Lease creation cancelled.");
                    return;
                }

                // Create lease and payment records
                using (var transaction = new TransactionScope())
                {
                    Lease newLease = repository.CreateLease(customerId, carId, startDate, endDate);
                    repository.RecordPayment(newLease.LeaseID, totalAmount);

                    transaction.Complete();

                    // Display confirmation
                    Console.WriteLine("\nLease and Payment created successfully:");
                    Console.WriteLine($"Lease ID: {newLease.LeaseID}");
                    Console.WriteLine($"Customer ID: {customerId}");
                    Console.WriteLine($"Car ID: {carId}");
                    Console.WriteLine($"Start Date: {startDate:d}");
                    Console.WriteLine($"End Date: {endDate:d}");
                    Console.WriteLine($"Lease Type: {newLease.Type}");
                    Console.WriteLine($"Total Amount: {totalAmount:F2}");
                    Console.WriteLine($"Payment Method: {paymentMethod}");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input format. Please enter valid numbers and dates.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\nError creating lease: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nAn unexpected error occurred: {ex.Message}");
            }
        }


        public void ReturnCar()
        {
            Console.WriteLine("Return Car");
            Console.Write("Enter lease ID: ");
            int leaseId = int.Parse(Console.ReadLine());
            repository.ReturnCar(leaseId);
            Console.WriteLine("\nCar returned successfully.");
            Console.WriteLine($"Lease ID: {leaseId} has been closed.");
        }

        public void RecordPayment()
        {
            Console.WriteLine("Record Payment");

            try
            {
                Console.Write("Enter lease ID: ");
                if (!int.TryParse(Console.ReadLine(), out int leaseId))
                {
                    throw new FormatException("Invalid lease ID.");
                }

                Lease lease = repository.ListLeaseHistory().Find(l => l.LeaseID == leaseId);
                if (lease == null)
                {
                    throw new ArgumentException($"Lease with ID {leaseId} not found.");
                }

                //// Calculate total amount and remaining balance
                //int numberOfDays = (int)(lease.EndDate - lease.StartDate).TotalDays + 1;
                //decimal totalAmount = lease.Vehicle.DailyRate * numberOfDays;
                //decimal remainingBalance = totalAmount - lease.Payments.Sum(p => p.Amount);

                // Display lease details
                Console.WriteLine("\nLease Details:");
                Console.WriteLine($"Lease ID: {lease.LeaseID}");
                Console.WriteLine($"Start Date: {lease.StartDate:d}");
                Console.WriteLine($"End Date: {lease.EndDate:d}");
                //Console.WriteLine($"Total Amount: {totalAmount:C}");
                //Console.WriteLine($"Remaining Balance: {remainingBalance:C}");

                // Get payment amount
                Console.Write("\nEnter payment amount: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
                {
                    throw new FormatException("Invalid payment amount.");
                }

                //if (amount > remainingBalance)
                {
                    Console.WriteLine("Warning: Payment amount exceeds remaining balance.");
                    Console.Write("Do you want to continue? (Y/N): ");
                    if (Console.ReadLine().Trim().ToUpper() != "Y")
                    {
                        Console.WriteLine("Payment cancelled.");
                        return;
                    }
                    //}

                    // Display payment options
                    Console.WriteLine("\nChoose payment method:");
                    Console.WriteLine("1. Bhim UPI");
                    Console.WriteLine("2. Card");
                    Console.WriteLine("3. Cash on Delivery");
                    Console.Write("Enter choice: ");
                    if (!int.TryParse(Console.ReadLine(), out int paymentChoice) || paymentChoice < 1 || paymentChoice > 3)
                    {
                        throw new FormatException("Invalid.");
                    }

                    string paymentMethod = paymentChoice switch
                    {
                        1 => "Bhim UPI",
                        2 => "Card",
                        3 => "Cash on Delivery",
                        _ => throw new ArgumentException("Invalid")
                    };

                    // Record the payment
                    repository.RecordPayment(leaseId, amount);

                    // Update remaining balance
                    //remainingBalance -= amount;

                    // Display confirmation
                    Console.WriteLine("\nPayment recorded successfully:");
                    Console.WriteLine($"Lease ID: {leaseId}");
                    Console.WriteLine($"Amount Paid: {amount:C}");
                    Console.WriteLine($"Payment Method: {paymentMethod}");
                    //Console.WriteLine($"New Remaining Balance: {remainingBalance:C}");

                    //if (remainingBalance <= 0)
                    //{
                    //    Console.WriteLine("Lease is now fully paid.");
                    //}
                }
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nAn unexpected error occurred: {ex.Message}");
            }
        }



        public void ViewLeaseHistory()
        {
            Console.WriteLine("Lease History");

            List<Lease> leaseHistory = repository.ListLeaseHistory();
            if (leaseHistory.Any())
            {
                Console.WriteLine($"{"LeaseID",-8}{"CustomerID",-12}{"CarID",-8}{"Start Date",-15}{"End Date",-15}{"Total Amount",-15}");

                foreach (var lease in leaseHistory)
                {
                    // Fetch the vehicle details using vehicleID
                    Vehicle vehicle = repository.FindCarById(lease.vehicleID);
                    if (vehicle == null)
                    {
                        Console.WriteLine($"Vehicle not found for Lease ID {lease.LeaseID}");
                        continue;  // Skip this lease if vehicle details are missing
                    }

                    // Calculate total amount based on the lease duration and daily rate
                    int numberOfDays = (int)(lease.EndDate - lease.StartDate).TotalDays + 1;  // +1 to include both start and end days
                    decimal totalAmount = vehicle.DailyRate * numberOfDays;

                    // Display the lease details with the calculated total amount
                    Console.WriteLine($"{lease.LeaseID,-8}{lease.CustomerID,-12}{lease.vehicleID,-8}{lease.StartDate.ToString("yyyy-MM-dd"),-15}{lease.EndDate.ToString("yyyy-MM-dd"),-15}{totalAmount,15:C2}");
                }

            }
            else
            {
                Console.WriteLine("No lease history found.");
            }
        }

        public void ListRentedCars()
        {
            Console.WriteLine("Rented Cars");
            List<Vehicle> rentedCars = repository.ListRentedCars();
            if (rentedCars.Any())
            {
                Console.WriteLine($"{"ID",-5}{"Make",-15}{"Model",-15}{"Year",-10}{"Daily Rate",-15}{" Engine Capacity",-10}");

                foreach (var car in rentedCars)
                {
                    Console.WriteLine($"{car.VehicleID,-5}{car.Make,-15}{car.Model,-15}{car.Year,-10}{car.DailyRate:C}{car.PassengerCapacity,-10}{car.EngineCapacity,-10}");
                }
            }
            else
            {
                Console.WriteLine("No cars are currently rented.");
            }
        }

        public void RemoveCar()
        {
            Console.WriteLine("Remove Car");
            Console.Write("car ID: ");

            try
            {
                int carId = int.Parse(Console.ReadLine());
                repository.RemoveCar(carId); // Attempt to remove the car

                Console.WriteLine("\nCar removed.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"\nerror: {ex.Message}");
            }
            catch (Exception ex) // General exception handling
            {
                Console.WriteLine($"\nerror: {ex.Message}");
            }
        }


        public void RemoveCustomer()
        {
            Console.WriteLine("Remove Customer");
            Console.Write("customer ID: ");

            try
            {
                int customerId = int.Parse(Console.ReadLine());
                repository.RemoveCustomer(customerId); // Attempt to remove the customer

                Console.WriteLine("\nCustomer removed successfully.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"\nerror: {ex.Message}");
            }
            catch (Exception ex) // General exception handling
            {
                Console.WriteLine($"\nerror: {ex.Message}");
            }
        }


        public void ViewActiveLeases()
        {
            Console.WriteLine("Active Leases");
            List<Lease> activeLeases = repository.ListActiveLeases();
            if (activeLeases.Any())
            {
                Console.WriteLine($"{"LeaseID",-8}{"CustomerID",-12}{"CarID",-8}{"Start Date",-15}{"End Date",-15}");
                
                foreach (var lease in activeLeases)
                {
                    Console.WriteLine($"{lease.LeaseID,-8}{lease.CustomerID,-12}{lease.vehicleID,-8}{lease.StartDate.ToString("yyyy-MM-dd"),-15}{lease.EndDate.ToString("yyyy-MM-dd"),-15}");
                }
                Console.WriteLine("\n" + new string('-', 5));
            }
            else
            {
                Console.WriteLine("No active leases found.");
            }
        }


    }
}