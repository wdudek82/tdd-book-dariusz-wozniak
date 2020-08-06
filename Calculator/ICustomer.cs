using System.Collections.Generic;

namespace Calculator
{
    public interface ICustomer
    {
        string FirstName { get; set; }
        public IPhoneNumber PhoneNumber { get; set; }
        public IList<IOrder> Orders { get; set; }

        int GetAge();
    }
}
