using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Exceptions.Cloudstorage
{
    internal class FileNotFoundException : BaseException
    {
        public FileNotFoundException(string file)
          : base(12004, "Sorry, we couldn't find a system file for {0}", file)
        {
            this.Status = 404;
        }
    }
}
