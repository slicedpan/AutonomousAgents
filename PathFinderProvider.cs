using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiniteStateMachine
{
    public class PathFinderProvider
    {
        static LocationEuclideanHeuristic h = new LocationEuclideanHeuristic();
        static IPathFinder<Location> current = new AStar<Location>(h, h, new LocationIDGen());
        public static IPathFinder<Location> Get()
        {
            return current;
        }
    }
}
