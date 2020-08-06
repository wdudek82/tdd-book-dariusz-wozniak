using NUnit.Framework;

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
    }
}
