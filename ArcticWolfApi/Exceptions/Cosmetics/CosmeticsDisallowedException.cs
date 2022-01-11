namespace ArcticWolfApi.Exceptions.Cosmetics
{
    internal class CosmeticsDisallowedException : BaseException
    {
        public CosmeticsDisallowedException() : base(19002, "Arctic Wolf does not natively support cosmetics.")
        {
            Status = 403;
        }
    }
}
