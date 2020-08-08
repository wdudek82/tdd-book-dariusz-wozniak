using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Calculator.Tests.Unit
{
    public class IsCustomerAdultValidatorTests
    {
        [Test]
        public void When_the_age_is_below_18_then_false_is_returned()
        {
            var customerMock = new CustomerMock(10);
            var validator = new IsCustomerAdultValidator();

            bool isAdult = validator.Validate(customerMock);

            Assert.That(isAdult, Is.False);
        }

        [Test]
        public void When_the_age_is_above_or_equal_18_then_true_is_returned([Values(18, 30)] int age)
        {
            var customerMock = new CustomerMock(age);
            var validator = new IsCustomerAdultValidator();

            bool isAdult = validator.Validate(customerMock);

            Assert.That(isAdult, Is.True);
        }

        [Test]
        public void When_the_age_is_null_then_exception_should_be_thrown()
        {
            var validator = new IsCustomerAdultValidator();

            Assert.That(() => validator.Validate(null), Throws.ArgumentNullException);
        }

        [Test]
        public void mock_get()
        {
            ICustomer customer = Mock.Of<ICustomer>(x => x.FirstName == "John");

            Mock<ICustomer> customerMock = Mock.Get(customer);

            customerMock.Setup(x => x.FirstName).Returns("Jason");

            Assert.That(customer.FirstName, Is.EqualTo("Jason"));
        }

        [Test]
        public void When_validator_always_returns_false_then_customer_is_never_added()
        {
            var validator = Mock.Of<ICustomerValidator>();

            var customerRepository = new CustomerRepository(validator);

            var john = Mock.Of<ICustomer>(customer => customer.FirstName == "John");
            var james = Mock.Of<ICustomer>(customer => customer.FirstName == "James");

            customerRepository.Add(john);
            customerRepository.Add(james);

            Assert.That(customerRepository.AllCustomers, Is.Empty);
        }

        [Test]
        public void When_validator_always_returns_true_then_customer_is_always_added()
        {
            var validator = Mock.Of<ICustomerValidator>(v =>
                v.Validate(It.IsAny<ICustomer>()));

            var john = Mock.Of<ICustomer>(customer => customer.FirstName == "John");
            var james = Mock.Of<ICustomer>(customer => customer.FirstName == "James");

            var customerRepository = new CustomerRepository(validator);
            customerRepository.Add(john);
            customerRepository.Add(james);

            Assert.That(customerRepository.AllCustomers, Has.Count.EqualTo(2));
            Assert.That(customerRepository.AllCustomers, HasCustomerWithFirstName("John"));
            Assert.That(customerRepository.AllCustomers, HasCustomerWithFirstName("James"));
        }

        private static Constraint HasCustomerWithFirstName(string firstName)
        {
            return Has
                .Exactly(1)
                .Matches<ICustomer>(customer => customer.FirstName == firstName);
        }

        [Test]
        public void Customer_is_added_depending_on_validation_result()
        {
            var validator = Mock.Of<ICustomerValidator>(v =>
                v.Validate(It.Is<ICustomer>(customer =>
                    customer.FirstName == "John")));

            var customerRepository = new CustomerRepository(validator);

            var john = Mock.Of<ICustomer>(customer => customer.FirstName == "John");
            var james = Mock.Of<ICustomer>(customer => customer.FirstName == "James");

            customerRepository.Add(john);
            customerRepository.Add(james);

            Assert.That(customerRepository.AllCustomers, Has.Count.EqualTo(1));
            Assert.That(customerRepository.AllCustomers,
                Has.Exactly(1).Matches<ICustomer>(customer => customer.FirstName == "John"));
        }
    }
}
