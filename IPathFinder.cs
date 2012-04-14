using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiniteStateMachine
{
    public interface I2DGraph<T>
    {
        List<T> GetNeighbours(T node);
        T GetNode(int x, int y);
    }
    public interface IPathFinder<T>
    {
        List<T> GetPath(T start, T end, I2DGraph<T> grid);       
    }
}
