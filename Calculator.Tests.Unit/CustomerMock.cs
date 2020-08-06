using System.Collections.Generic;

namespace Calculator.Tests.Unit
{
    internal class CustomerMock : ICustomer
    {
        private readonly int _age;

        public CustomerMock(int age)
        {
            _age = age;
        }

        public string FirstName { get; set; }
        public IPhoneNumber PhoneNumber { get; set; }
        public IList<IOrder> Orders { get; set; }

        public int GetAge()
        {
            return _age;
        }
    }
}
