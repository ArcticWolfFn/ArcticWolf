namespace ArcticWolfApi.Exceptions.OAuth
{
    internal class InvalidRequestException : BaseException
    {
        public InvalidRequestException(string field) : base(1016, field + " is required.")
        {
            Status = 400;
        }
    }
}
