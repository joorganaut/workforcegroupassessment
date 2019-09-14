using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF.Core.Contract
{
    public interface IRequest
    {

    }
    [Serializable]
    public abstract class Request : IRequest
    {
    }
}
