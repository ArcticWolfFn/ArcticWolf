using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.Storage
{
    public class PakFile
    {
        public int Id { get; set; }
        public string File { get; set; }
        public string AesKey { get; set; }

        public int FnVersionId { get; set; }
        public FnVersion FnVersion { get; set; }
    }
}
