using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.Storage
{
    public class TkKey
    {
        public int Id { get; set; }
        public string Key { get; set; }

        public int FnVersionId { get; set; }
        public FnVersion FnVersion { get; set; }
    }
}
