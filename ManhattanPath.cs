using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiniteStateMachine
{
    public class ManhattanPath : IPathFinder<Location>
    {
        IHeuristic<Location> heuristic;
        public ManhattanPath()
        {
            this.heuristic = new LocationEuclideanHeuristic();
        }
        public List<Location> GetPath(Location start, Location end, IGraph<Location> grid)
        {
            List<Location> path = new List<Location>();

            path.Add(start);

            Location current = start;

            int steps = 0;

            while (steps < 100)
            {
                List<Location> locations = grid.GetNeighbours(current);
                float minimum = float.MaxValue;
                Location currentBest = null;
                for (int i = 0; i < locations.Count; ++i)
                {
                    float estimate = heuristic.GetEstimate(end, locations[i]);
                    if (estimate < minimum)
                    {
                        currentBest = locations[i];
                        minimum = estimate;
                    }
                }
                current = currentBest;
                path.Add(current);
                ++steps;
                if (current == end)
                    break;
            }

            return path;
        }
    }
}
