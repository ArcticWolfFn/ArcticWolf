namespace ArcticWolfApi.Exceptions
{
    internal class UnhandledErrorException : BaseException
    {
        public UnhandledErrorException(string message) : base(1012, "Sorry, an error occurred and we were unable to resolve it. Error: {0}", message)
        {
            Status = 500;
        }
    }
}
