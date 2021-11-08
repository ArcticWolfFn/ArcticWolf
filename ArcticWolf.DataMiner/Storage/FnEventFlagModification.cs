using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Storage
{
    public class FnEventFlagModification
    {
        public int Id { get; set; }

        public FnEventFlagTimeSpan ModifiedTimeSpan { get; set; }

        public DateTime OverriddenStartTime { get; set; }
        public DateTime NewStartTime { get; set; }

        public DateTime OverriddenEndTime { get; set; }
        public DateTime NewEndTime { get; set; }
    }
}
