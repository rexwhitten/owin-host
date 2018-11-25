using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apistation.owin.Models
{
    public interface IModelBuilder
    {
        IResourceModelBuilder Resource();
    }

    public interface IResourceModelBuilder
    {
        
    }

}
