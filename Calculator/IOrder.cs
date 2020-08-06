namespace Calculator
{
    public interface IOrder
    {
        public int Id { get; set; }
        decimal Price { get; set; }
    }
}
