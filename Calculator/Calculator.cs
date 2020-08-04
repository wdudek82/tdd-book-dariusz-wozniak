using System;

namespace Calculator
{
    public class Calculator
    {
        public event EventHandler<decimal> Calculated;

        public decimal Divide(decimal dividend, decimal divisor)
        {
            if (divisor == 0)
            {
                throw new DivideByZeroException();
            }

            var quotient = dividend / divisor;
            OnCalculated(quotient);

            return quotient;
        }

        private void OnCalculated(decimal result)
        {
            Calculated?.Invoke(this, result);
        }
    }
}
