using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Abstractions.Interfaces
{
    public abstract class GenericCommand<T>
    {
        public virtual async Task ExecuteAsync(T parameter)
        {
        }
    }
}
