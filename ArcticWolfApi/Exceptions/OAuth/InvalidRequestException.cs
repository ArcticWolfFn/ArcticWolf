using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Exceptions.OAuth
{
    internal class InvalidRequestException : BaseException
    {
        public InvalidRequestException(string field)
          : base(1016, field + " is required.")
        {
            this.Status = 400;
        }
    }
}
