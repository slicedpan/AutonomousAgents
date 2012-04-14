using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiniteStateMachine
{
    public interface IGraph<T>
    {
        List<T> GetNeighbours(T node);
    }
    public interface IPathFinder<T>
    {
        List<T> GetPath(T start, T end, IGraph<T> grid);       
    }
}
