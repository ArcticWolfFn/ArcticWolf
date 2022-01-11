namespace ArcticWolfApi.Exceptions.OAuth
{
    internal class InvalidClientException : BaseException
    {
        public InvalidClientException() : base(1011, "It appears that your Authorization header may be invalid or not present, please verify that you are sending the correct headers.")
        {
            Status = 400;
        }
    }
}
