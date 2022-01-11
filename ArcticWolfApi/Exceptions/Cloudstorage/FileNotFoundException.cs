namespace ArcticWolfApi.Exceptions.Cloudstorage
{
    internal class FileNotFoundException : BaseException
    {
        public FileNotFoundException(string file) : base(12004, "Sorry, we couldn't find a system file for {0}", file)
        {
            Status = 404;
        }
    }
}
