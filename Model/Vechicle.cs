using System;
namespace CarRentalSystem.Model
{
    public class Vehicle
    {
        public int vehicleID;
        public string make;
        public string model;
        public int year;
        public decimal dailyRate;
        public string status;
        public int passengerCapacity;
        public string engineCapacity;
        

        public Vehicle(int vehicleID, string make, string model, int year, decimal dailyRate,string status, int passengerCapacity, string engineCapacity)
        {
            this.vehicleID = vehicleID;
            this.make = make;
            this.model = model;
            this.year = year;
            this.dailyRate = dailyRate;
            this.status = status;
            this.passengerCapacity = passengerCapacity;
            this.engineCapacity = engineCapacity;
        }
        // Getter and Setter for vehicleID
        public int VehicleID
        {
            get { return vehicleID; }
            set { vehicleID = value; }
        }

        // Getter and Setter for make
        public string Make
        {
            get { return make; }
            set { make = value; }
        }

        // Getter and Setter for model
        public string Model
        {
            get { return model; }
            set { model = value; }
        }

        // Getter and Setter for year
        public int Year
        {
            get { return year; }
            set { year = value; }
        }

        // Getter and Setter for dailyRate
        public decimal DailyRate
        {
            get { return dailyRate; }
            set { dailyRate = value; }
        }

        // Getter and Setter for status
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        // Getter and Setter for passengerCapacity
        public int PassengerCapacity
        {
            get { return passengerCapacity; }
            set { passengerCapacity = value; }
        }

        // Getter and Setter for engineCapacity
        public string EngineCapacity
        {
            get { return engineCapacity; }
            set { engineCapacity = value; }
        }

    }
}