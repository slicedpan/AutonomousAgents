using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiniteStateMachine
{
    public class PathFinderProvider
    {
        static IPathFinder<Location> current = new ManhattanPath();
        public static IPathFinder<Location> Get()
        {
            return current;
        }
    }
}
