using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.Storage
{
    public class FnEventFlag
    {
        [Key]
        public string Event { get; set; }

        public ICollection<FnEventFlagTimeSpan> TimeSpans { get; set; } = new Collection<FnEventFlagTimeSpan>();
        public ICollection<FnEventFlagModification> Modifications { get; set; } = new Collection<FnEventFlagModification>();
    }
}
