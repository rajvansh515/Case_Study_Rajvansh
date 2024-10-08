using System;
namespace CarRentalSystem.Exceptions
{
    internal class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException(string message) : base(message){ }
    }
}