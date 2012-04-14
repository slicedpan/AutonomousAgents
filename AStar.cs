using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiniteStateMachine
{

    public interface IHeuristic<T>
    {
        float GetEstimate(T current, T end);
    }

    public interface IMovementCost<T>
    {
        float GetMovementCost(T first, T second);
    }   

    public class AStar<T> : IPathFinder<T>
    {

        class AStarNode<T>
        {
            public AStarNode<T> parentNode;
            public AStarNode<T> childNode;

            public float FValue;
            public float GValue;
            public float HValue;

            public T nodeValue;

            public AStarNode(T nodeValue)
            {
                this.nodeValue = nodeValue;
            }
        }

        IHeuristic<T> heuristic;
        IMovementCost<T> movementCost;
        IComparer<AStarNode<T>> nodeComparer;

        List<AStarNode<T>> open;
        List<AStarNode<T>> closed;

        public bool AllowDiagonals = false;

        public AStar (IHeuristic<T> heuristic, IMovementCost<T> movementCost, IComparer<AStarNode<T>> nodeComparer)
        {
            this.heuristic = heuristic;
            this.movementCost = movementCost;
            this.nodeComparer = nodeComparer;
            open = new List<AStarNode<T>>();
            closed = new List<AStarNode<T>>();
        }
        public List<T> GetPath(T start, T end, I2DGraph<T> grid)
        {
            List<T> path = new List<T>();
            bool finished = false;
            open.Add(new AStarNode<T>(start) { childNode = null, parentNode = null, FValue = 0.0f, GValue = 0.0f, HValue = 0.0f });
            while (!finished)
            {
                AStarNode<T> current = open[0];
                if (current.nodeValue.Equals(end))
                {
                    finished = true;
                    break;
                }
                closed.Add(current);
                foreach (T neighbour in grid.GetNeighbours(current.nodeValue))
                {
                    AStarNode<T> n = new AStarNode<T>(neighbour);
                    n.childNode = current;
                    n.FValue = 0.0f; 
                }
            }
            open.Clear();
            closed.Clear();
            return path;
        }
    }
}
