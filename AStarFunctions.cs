using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FiniteStateMachine
{
    public class LocationDistanceHeuristic : IHeuristic<Location>, IMovementCost<Location>
    {
        public float GetEstimate(Location current, Location end)
        {
            return new Vector2(current.X - end.X, current.Y - end.Y).Length();
        }

        public float GetMovementCost(Location first, Location second)
        {
            return GetEstimate(first, second) * (first.TravelCost + second.TravelCost);
        }
    }

    public class LocationManhattanHeuristic : IHeuristic<Location>, IMovementCost<Location>
    {

        public float GetEstimate(Location current, Location end)
        {
            return Math.Abs((float)(current.X - end.X + current.Y - end.Y));
        }

        float GetMovementCost(Location first, Location second)
        {
            return GetEstimate(first, second) * (first.TravelCost + second.TravelCost);
        }
    }

    public class LocationNodeComparer : IComparer<AStarNode<Location>>
    {
        public int Compare(AStarNode<Location> x, AStarNode<Location> y)
        {
            if (x.FValue < y.FValue)
                return -1;
            else
                return 1;
        }
    }

}
