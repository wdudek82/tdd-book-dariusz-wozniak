namespace Calculator.Tests.Unit
{
    internal class CustomerMock : ICustomer
    {
        private readonly int _age;

        public CustomerMock(int age)
        {
            _age = age;
        }
        public int GetAge()
        {
            return _age;
        }
    }
}
