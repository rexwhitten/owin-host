using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace apistation.owin.Events
{
    public class NotFoundEvent  : BaseEvent
    {
        public NotFoundEvent() { }

        public PathString Path { get; internal set; }
    }
}
