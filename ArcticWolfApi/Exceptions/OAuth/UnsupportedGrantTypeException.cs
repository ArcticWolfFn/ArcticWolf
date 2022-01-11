namespace ArcticWolfApi.Exceptions.OAuth
{
    internal class UnsupportedGrantTypeException : BaseException
    {
        public UnsupportedGrantTypeException(string grantType) : base(1013, "Unsupported grant type: " + grantType)
        {
            Status = 400;
        }
    }
}
