using System;
using System.Threading.Tasks;

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

        public async Task<decimal> DivideAsync(decimal dividend, decimal divisor)
        {
            return await GetResultFromTimeConsumingDivision(dividend, divisor);
        }

        private static async Task<decimal> GetResultFromTimeConsumingDivision(decimal dividend, decimal divisor)
        {
            // Simulate time consuming operation
            await Task.Delay(300);

            return dividend / divisor;
        }
    }
}
