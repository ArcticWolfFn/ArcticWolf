namespace ArcticWolfApi.Exceptions
{
    internal class MethodNotAllowedException : BaseException
    {
        public MethodNotAllowedException() : base(1009, "Sorry the resource you were trying to access cannot be accessed with the HTTP method you used.")
        {
            Status = 405;
        }
    }
}
