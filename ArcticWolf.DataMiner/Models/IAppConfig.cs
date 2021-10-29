using Config.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Models
{
    public interface IAppConfig
    {
        [Option(DefaultValue = "0")]
        decimal LastCheckedFnVersion { get; set; }

        [Option(DefaultValue = "db.sqlite")]
        string DatabasePath { get; set; }
    }
}
