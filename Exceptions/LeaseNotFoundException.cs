using System;
namespace CarRentalSystem.Exceptions
{
    internal class LeaseNotFoundException : Exception
    {
        public LeaseNotFoundException(string message) : base(message){ }
    }
}