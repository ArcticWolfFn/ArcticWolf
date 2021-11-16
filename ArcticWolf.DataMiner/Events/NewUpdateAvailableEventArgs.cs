using ArcticWolf.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Events
{
    public class NewUpdateAvailableEventArgs : EventArgs
    {
        public FnVersion UpdateVersion { get; set; }
    }
}
