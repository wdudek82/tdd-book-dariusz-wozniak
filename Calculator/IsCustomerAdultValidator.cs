using System;
using System.ComponentModel.DataAnnotations;

namespace Calculator
{
    public class IsCustomerAdultValidator
    {
        public bool Validate(ICustomer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            const int adultAge = 18;
            return customer.GetAge() >= adultAge;
        }
    }
}
