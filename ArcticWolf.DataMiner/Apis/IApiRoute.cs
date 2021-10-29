using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Apis
{
    public interface IApiRoute
    {
        bool SupportsPreviousFnVersions { get; }
    }
}
