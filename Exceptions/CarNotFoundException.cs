using System;
namespace CarRentalSystem.Exceptions
{
    internal class CarNotFoundException : Exception
    {
        public CarNotFoundException(string message) : base(message){ }
    }
}