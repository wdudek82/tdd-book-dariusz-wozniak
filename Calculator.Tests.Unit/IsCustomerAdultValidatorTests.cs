using System;
using System.ComponentModel.DataAnnotations;
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
        public void property_behavior_imperative()
        {
            var customerMock = new Mock<ICustomer>();
            customerMock.SetupProperty(x => x.FirstName, "John");

            ICustomer customer = customerMock.Object;
            customer.FirstName = "Jason";

            string firstName = customer.FirstName;

            Assert.That(firstName, Is.EqualTo("Jason"));
        }

        [Test]
        public void property_behavior_with_setup_all_properties__imperative()
        {
            var customerMock = new Mock<ICustomer>();
            customerMock.SetupAllProperties();

            ICustomer customer = customerMock.Object;
            customer.FirstName = "Jason";

            string firstName = customer.FirstName;

            Assert.That(firstName, Is.EqualTo("Jason"));
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

        [Test]
        public void sequential_mock()
        {
            var customerMock = new Mock<ICustomer>();
            customerMock.SetupSequence(x => x.FirstName)
                .Returns("John")
                .Returns("James")
                .Returns("Adam");

            ICustomer customer = customerMock.Object;

            Assert.That(customer.FirstName, Is.EqualTo("John"));
            Assert.That(customer.FirstName, Is.EqualTo("James"));
            Assert.That(customer.FirstName, Is.EqualTo(("Adam")));
            Assert.That(customer.FirstName, Is.Null);
        }

        [Test]
        public void customer_is_added_depending_on_validation_result()
        {
            var validator = Mock.Of<ICustomerValidator>(v =>
                v.Validate(StartsWithJ));

            var customerRepository = new CustomerRepository(validator);

            var john = Mock.Of<ICustomer>(customer =>
                customer.FirstName == "John");
            var james = Mock.Of<ICustomer>(customer =>
                customer.FirstName == "james");
            var ken = Mock.Of<ICustomer>(customer =>
                customer.FirstName == "Ken");

            customerRepository.Add(john);
            customerRepository.Add(james);
            customerRepository.Add(ken);

            Assert.That(customerRepository.AllCustomers,
                Has.Exactly(1).Matches<ICustomer>(customer => customer.FirstName == "John"));
        }

        private static ICustomer StartsWithJ => It.Is<ICustomer>(x =>
            x != null &&
            !string.IsNullOrEmpty(x.FirstName) &&
            x.FirstName.StartsWith("J", StringComparison.InvariantCulture));

        // Alternative implementation
        private ICustomer StartsWithJMatch => Match.Create<ICustomer>(x =>
            !string.IsNullOrEmpty(x?.FirstName) &&
            x.FirstName.StartsWith("J", StringComparison.InvariantCulture));

        [Test]
        public void when_customer_is_added_then_validate_is_called_once()
        {
            var validator = Mock.Of<ICustomerValidator>();

            var john = Mock.Of<ICustomer>(customer => customer.FirstName == "John");

            var customerRepository = new CustomerRepository(validator);
            customerRepository.Add(john);

            // For any object fulfilling ICustomer interface
            // Mock.Get(validator).Verify(x => x.Validate(It.IsAny<ICustomer>()), Times.Once);

            // For a customer with a specific property value
            Mock.Get(validator).Verify(x =>
                x.Validate(It.Is<ICustomer>(customer => customer.FirstName == "John")), Times.Once());
        }

        [Test]
        public void verify_set()
        {
            var customer = Mock.Of<ICustomer>();
            customer.FirstName = "John";

            Mock.Get(customer).VerifySet(x => x.FirstName = "John", Times.Once);
        }

        [Test]
        public void callback_test()
        {
            var timesCalled = 0;

            var customerValidatorMock = new Mock<ICustomerValidator>();

            customerValidatorMock
                .Setup(x => x.Validate(It.IsAny<ICustomer>()))
                .Callback(() => timesCalled++);

            var customerRepository = new CustomerRepository(customerValidatorMock.Object);

            customerRepository.Add(Mock.Of<ICustomer>());
            customerRepository.Add(Mock.Of<ICustomer>());

            Assert.That(timesCalled, Is.EqualTo(2));
        }
    }
}
