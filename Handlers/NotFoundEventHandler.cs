using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apistation.owin.Handlers
{
    public class NotFoundEventHandler : BaseHandler<Events.NotFoundEvent>
    {
        public NotFoundEventHandler() : base((e) =>
        {
            Console.WriteLine("Not found Event Handler");
            Console.WriteLine(e.Path);
        })
        {

        }
    }
}
