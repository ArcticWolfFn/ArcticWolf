using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Exceptions.OAuth
{
    internal class InvalidClientException : BaseException
    {
        public InvalidClientException()
          : base(1011, "It appears that your Authorization header may be invalid or not present, please verify that you are sending the correct headers.")
        {
            this.Status = 400;
        }
    }
}
