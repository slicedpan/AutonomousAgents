using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiniteStateMachine
{
    public interface IPathFinder<T>
    {
        List<T> GetPath(T start, T end, List<List<T>> grid);       
    }
}
