using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Exceptions
{
    internal class NotFoundException : BaseException
    {
        public NotFoundException()
          : base(1004, "Sorry the resource you were trying to find could not be found")
        {
            this.Status = 404;
        }
    }
}
