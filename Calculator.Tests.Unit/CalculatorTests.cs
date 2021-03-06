using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Calculator.Tests.Unit
{
    public class CalculatorTests
    {
        [DatapointSource]
        private readonly double[] _numbers = {-10.0, -6.3, 0, 1, 4.2, 120.7};

        [DatapointSource]
        private readonly double[] _negativeNumbers = {-10.0, -6.3};

        [DatapointSource]
        private readonly double[] _positiveNumbers = {1, 4.2, 120.7};

        [Datapoint]
        private readonly double _zero = 0;

        [Theory]
        public void When_dividing_positive_number_by_zero_then_result_is_positive_infinity(double number)
        {
            Assume.That(number, Is.Positive);

            double quotient = number / 0;

            Assert.That(quotient, Is.EqualTo(double.PositiveInfinity));
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
            // Arrange
            var calculator = new Calculator();

            // Act
            decimal quotient = calculator.Divide(divided, divisor);

            // Assert
            // Assert.AreEqual(expectedQuotient, quotient);
            quotient.Should().Be(expectedQuotient);
        }

        [Test]
        public void When_dividing_one_by_three_then_the_result_is_0_comma_3333333333333333333333333333()
        {
            // Arrange
            var calculator = new Calculator();

            // Arrange
            decimal expectedQuotient = 0.3333333333333333333333333333m;

            // Act
            decimal quotient = calculator.Divide(1, 3);

            // Assert
            // Assert.AreEqual(expectedQuotient, quotient);
            quotient.Should().Be(expectedQuotient);
        }

        [Test]
        public void When_dividing_two_by_three_then_the_result_is_0_comma_6666666666666666666666666667()
        {
            // Arrange
            var calculator = new Calculator();

            // Arrange
            decimal expectedQuotient = 0.6666666666666666666666666667m;

            // Act
            decimal quotient = calculator.Divide(2, 3);

            // Assert
            // Assert.AreEqual(expectedQuotient, quotient);
            quotient.Should().Be(expectedQuotient);
        }

        [Test]
        public void When_dividing_two_by_three_then_the_result_is_0_comma_6666_within_tolerance()
        {
            // Arrange
            var calculator = new Calculator();

            // Arrange
            const decimal expectedQuotient = 0.6666m;

            // Act
            decimal quotient = calculator.Divide(2, 3);

            // Assert
            // Assert.AreEqual(expectedQuotient, (double) quotient, 0.0001);
            quotient.Should().BeApproximately(expectedQuotient, 0.0001m);
        }

        [Test]
        public void When_dividing_by_zero_then_exception_is_thrown()
        {
            // Arrange
            var calculator = new Calculator();

            // Act & Assert
            // Assert.Throws<DivideByZeroException>(() => calculator.Divide(5, 0));
            calculator.Invoking(x => x.Divide(5, 0))
                .Should().Throw<DivideByZeroException>();
        }

        [Test]
        public void When_division_is_complete_then_an_event_is_called()
        {
            // Arrange
            var calculator = new Calculator();
            var hasEventBeenCalled = false;

            // Act
            calculator.Calculated += (sender, result) => hasEventBeenCalled = true;
            calculator.Divide(4, 2);

            // Assert
            // Assert.IsTrue(hasEventBeenCalled);
            hasEventBeenCalled.Should().BeTrue();
        }

        [Test]
        public void When_dividing_is_complete_then_result_is_passed_to_event_args()
        {
            // Arrange
            var calculator = new Calculator();
            decimal? quotient = null;

            // Act
            calculator.Calculated += (sender, result) => quotient = result;
            calculator.Divide(4, 2);

            // Assert
            quotient.Should().NotBeNull();
            quotient!.Value.Should().BeApproximately(4m / 2, 0.001m);

            // Assert.NotNull(quotient);
            // Assert.AreEqual(4 / 2, quotient.Value);
        }

        [TestCase(4, 2, ExpectedResult = 2)]
        [TestCase(-4, 2, ExpectedResult = -2)]
        [TestCase(4, -2, ExpectedResult = -2)]
        [TestCase(0, 3, ExpectedResult = 0)]
        [TestCase(5, 2, ExpectedResult = 2.5)]
        public async Task<decimal> When_dividing_four_by_two_ten_the_result_is_two(decimal dividend, decimal divisor)
        {
            var calculator = new Calculator();

            return await calculator.DivideAsync(dividend, divisor);
        }
    }
}
