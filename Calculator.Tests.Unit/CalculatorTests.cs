using System;
using FluentAssertions;
using NUnit.Framework;

namespace Calculator.Tests.Unit
{

    public class CalculatorTests
    {
        private Calculator _calculator;

        [SetUp]
        public void BaseSetUp() {
            // Arrange
            _calculator = new Calculator();
        }


        [TestCase(4, 2, 2)]
        [TestCase(-4, 2, -2)]
        [TestCase(0, 3, 0)]
        [TestCase(5, 2, 2.5)]
        // [TestCase(3, 0, typeof(DivideByZeroException))]
        // [TestCase(2, 3, 0.6)]
        public void When_dividing_two_numbers_then_result_is_properly_calculated(decimal divided, decimal divisor,
            decimal expectedQuotient)
        {
            // Act
            decimal quotient = _calculator.Divide(divided, divisor);

            // Assert
            // Assert.AreEqual(expectedQuotient, quotient);
            quotient.Should().Be(expectedQuotient);
        }

        [Test]
        public void When_dividing_one_by_three_then_the_result_is_0_comma_3333333333333333333333333333()
        {
            // Arrange
            decimal expectedQuotient = 0.3333333333333333333333333333m;

            // Act
            decimal quotient = _calculator.Divide(1, 3);

            // Assert
            // Assert.AreEqual(expectedQuotient, quotient);
            quotient.Should().Be(expectedQuotient);
        }

        [Test]
        public void When_dividing_two_by_three_then_the_result_is_0_comma_6666666666666666666666666667()
        {
            // Arrange
            decimal expectedQuotient = 0.6666666666666666666666666667m;

            // Act
            decimal quotient = _calculator.Divide(2, 3);

            // Assert
            // Assert.AreEqual(expectedQuotient, quotient);
            quotient.Should().Be(expectedQuotient);
        }

        [Test]
        public void When_dividing_two_by_three_then_the_result_is_0_comma_6666_within_tolerance()
        {
            // Arrange
            const decimal expectedQuotient = 0.6666m;

            // Act
            decimal quotient = _calculator.Divide(2, 3);

            // Assert
            // Assert.AreEqual(expectedQuotient, (double) quotient, 0.0001);
            quotient.Should().BeApproximately(expectedQuotient, 0.0001m);
        }

        [Test]
        public void When_dividing_by_zero_then_exception_is_thrown()
        {
            // Arrange
            // Act & Assert
            // Assert.Throws<DivideByZeroException>(() => calculator.Divide(5, 0));
            _calculator.Invoking(x => x.Divide(5, 0))
                .Should().Throw<DivideByZeroException>();
        }

        [Test]
        public void When_division_is_complete_then_an_event_is_called()
        {
            // Arrange
            var hasEventBeenCalled = false;

            // Act
            _calculator.Calculated += (sender, result) => hasEventBeenCalled = true;
            _calculator.Divide(4, 2);

            // Assert
            // Assert.IsTrue(hasEventBeenCalled);
            hasEventBeenCalled.Should().BeTrue();
        }

        [Test]
        public void When_dividing_is_complete_then_result_is_passed_to_event_args()
        {
            // Arrange
            decimal? quotient = null;

            // Act
            _calculator.Calculated += (sender, result) => quotient = result;
            _calculator.Divide(4, 2);

            // Assert
            quotient.Should().NotBeNull();
            quotient!.Value.Should().BeApproximately(4m / 2, 0.001m);

            // Assert.NotNull(quotient);
            // Assert.AreEqual(4 / 2, quotient.Value);
        }
    }
}
