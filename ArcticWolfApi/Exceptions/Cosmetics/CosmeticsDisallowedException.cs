using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Exceptions.Cosmetics
{
    internal class CosmeticsDisallowedException : BaseException
    {
        public CosmeticsDisallowedException()
          : base(19002, "Arctic Wolf does not natively support cosmetics.")
        {
            this.Status = 403;
        }
    }
}
