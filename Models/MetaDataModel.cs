using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apistation.owin.Models
{
    public class MetaDataModel
    {
        public Uri MetaUri { get; set; }
        
        public MetaDataModel() {
            this.MetaUri = new Uri("http://127.0.0.1");
        }
    }
}
