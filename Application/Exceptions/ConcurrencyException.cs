namespace Application.Exceptions
{
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException(string Message, Exception ex) : base(Message, ex)
        {
        }
    }
}
