using System;
using System.Collections.Generic;

namespace Calculator
{
    public class CustomerRepository
    {
        private readonly ICustomerValidator _customerValidator;
        private readonly List<ICustomer> _allCustomers;

        public CustomerRepository(ICustomerValidator customerValidator)
        {
            _customerValidator = customerValidator;
            _allCustomers = new List<ICustomer>();
        }

        public IReadOnlyList<ICustomer> AllCustomers => _allCustomers.AsReadOnly();

        public void Add(ICustomer customer)
        {
            if (!_customerValidator.Validate(customer)) return;
            _allCustomers.Add(customer);
        }
    }
}
