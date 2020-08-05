using System;

namespace Calculator
{
    public class GenericCalculator<T>
    {
        public T Add(T a, T b)
        {
            return (dynamic) a + b;
        }
    }
}
