using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.Storage
{
    public class FnVersion
    {
        [Key]
        public decimal Version { get; set; }
        public string VersionString { get; set; }
        public string MainKey { get; set; }
        public ICollection<TkKey> TkKeys { get; set; }
        public ICollection<PakFile> PakFiles { get; set; }
    }
}
