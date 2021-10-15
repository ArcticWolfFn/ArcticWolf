using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Exceptions.OAuth
{
    internal class UnsupportedGrantTypeException : BaseException
    {
        public UnsupportedGrantTypeException(string grantType)
          : base(1013, "Unsupported grant type: " + grantType)
        {
            this.Status = 400;
        }
    }
}
