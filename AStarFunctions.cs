using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FiniteStateMachine
{
    public class LocationEuclideanHeuristic : IHeuristic<Location>, IMovementCost<Location>
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
            return (float)(Math.Abs(current.X - end.X) + Math.Abs(current.Y - end.Y));
        }

        public float GetMovementCost(Location first, Location second)
        {
            return GetEstimate(first, second) * (first.TravelCost + second.TravelCost);
        }

    }

    public class LocationIDGen : IUIDGenerator<Location>
    {
        public uint GenerateID(Location value)
        {
            return (uint)(value.Y * Game1.numCellsX + value.X);
        }
    }

}
