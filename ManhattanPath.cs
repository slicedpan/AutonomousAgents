using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiniteStateMachine
{
    public class ManhattanPath : IPathFinder<Location>
    {
        public List<Location> GetPath(Location start, Location end, List<List<Location>> grid)
        {
            List<Location> path = new List<Location>();
            int startX = start.X;
            int xOffset = 1, yOffset = 1;

            if (start.X > end.X)
                xOffset = -1;
            if (start.Y > end.Y)
                yOffset = -1;

            int currentX = start.X;
            int currentY = start.Y;

            path.Add(start);

            while (currentX != end.X)
            {
                currentX += xOffset;
                path.Add(grid[currentX][currentY]);                
            }
            while (currentY != end.Y)
            {
                currentY += yOffset;
                path.Add(grid[currentX][currentY]);
            }
            return path;
        }
    }
}
