using System;
using System.Collections.Generic;
using System.Collections;
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

    public interface IUIDGenerator<T>
    {
        uint GenerateID(T value);
    }

    public class AStar<T> : IPathFinder<T>
    {

        #region inner classes

        class AStarNode
        {
            public AStarNode parentNode;
            public AStarNode childNode;

            public float FValue;
            public float GValue;
            public float HValue;

            public T nodeValue;

            public AStarNode(T nodeValue)
            {
                this.nodeValue = nodeValue;
            }
        }

        class NodeComparer : IComparer<AStarNode>
        {
            public int Compare(AStarNode x, AStarNode y)
            {
                if (x.FValue > y.FValue)
                    return 1;
                else if (x.FValue < y.FValue)
                    return -1;
                return 0;
            }
        }

        class OpenSet
        {
            SortedList<float, AStarNode> _list;
            IUIDGenerator<T> uidGenerator;
            BitArray memberShip;

            float[] randomFloats = new float[128];
            int randomNum = 0;

            public OpenSet(IUIDGenerator<T> uidGenerator)
            {               
                this.uidGenerator = uidGenerator;
                _list = new SortedList<float, AStarNode>();
                memberShip = new BitArray(1024);
                Random rand = new Random();
                for (int i = 0; i < randomFloats.Length; ++i)
                {
                    randomFloats[i] = (float)(rand.NextDouble() * 0.05d);
                }
            }
            public void Add(AStarNode node)
            {
                _list.Add(node.FValue + randomFloats[randomNum++ % randomFloats.Length], node);
                int id = (int)uidGenerator.GenerateID(node.nodeValue);
                if (id > memberShip.Count)
                    memberShip.Length = id;
                memberShip[(int)uidGenerator.GenerateID(node.nodeValue)] = true;
            }            
            public int Count
            {
                get
                {
                    return _list.Count;
                }
            }
            public AStarNode Pop()
            {
                AStarNode retNode = _list.Values[0];
                memberShip[(int)uidGenerator.GenerateID(retNode.nodeValue)] = false;
                _list.RemoveAt(0);
                return retNode;
            }
            public void Clear()
            {
                _list.Clear();
                memberShip.SetAll(false);
            }
            public bool Contains(AStarNode node)
            {
                return memberShip[(int)uidGenerator.GenerateID(node.nodeValue)];
            }
            public bool Contains(T nodeValue)
            {
                return memberShip[(int)uidGenerator.GenerateID(nodeValue)];
            }
            public AStarNode Find(T nodeValue)
            {
                foreach (AStarNode node in _list.Values)
                {
                    if (node.nodeValue.Equals(nodeValue))
                        return node;
                }
                return null;
            }
            public AStarNode Find(float FValue)
            {
                return _list[FValue];
            }
            public AStarNode Find(T nodeValue, float startFValue)
            {
                float lowest = _list.Keys[0];
                float difference = _list.Keys[_list.Count - 1] - lowest;
                startFValue -= lowest;
                int firstPos = (int)(difference / startFValue) % _list.Count;

                if (_list.Values[firstPos].nodeValue.Equals(nodeValue))
                    return _list.Values[firstPos];

                int offset = 0;

                while (true)
                {
                    if (_list.Values[(firstPos + offset) % _list.Count].Equals(nodeValue))
                        return _list.Values[(firstPos + offset) % _list.Count];
                }

            }
            public void UpdateEntry(float FValue, AStarNode n)
            {
                _list.Remove(FValue);
                _list.Add(n.FValue, n);
            }
        }

        class ClosedSet
        {
            SortedList<uint, AStarNode> _list = new SortedList<uint,AStarNode>();
            IUIDGenerator<T> uidGenerator;

            public ClosedSet(IUIDGenerator<T> uidGenerator)
            {
                this.uidGenerator = uidGenerator;
            }
            public bool Contains(AStarNode node)
            {
                return Contains(node.nodeValue);
            }
            public bool Contains(T nodeValue)
            {
                uint id = uidGenerator.GenerateID(nodeValue);
                int lowPos = 0;
                int highPos = _list.Count - 1;
                int curPos;
                uint curValue;
                if (_list.Count == 0)
                    return false;
                if (_list.Count == 1)
                    return _list.Keys[0] == id;
                while (true)
                {
                    if (highPos == lowPos)
                        return _list.Keys[highPos] == id;
                    curPos = ((highPos - lowPos) / 2) + lowPos;
                    curValue = _list.Keys[curPos];
                    if (id == curValue)
                        return true;
                    if (id < curValue)
                    {
                        highPos = curPos;
                    }
                    else
                    {
                        lowPos = curPos + 1;
                    }
                }
            }
            public void Add(AStarNode node)
            {
                _list.Add(uidGenerator.GenerateID(node.nodeValue), node);
            }
            public void Clear()
            {
                _list.Clear();                
            }
            public AStarNode Find(T nodeValue)
            {
                foreach (AStarNode node in _list.Values)
                {
                    if (node.nodeValue.Equals(nodeValue))
                        return node;
                }
                return null;
            }
        }

        #endregion

        IHeuristic<T> heuristic;
        IMovementCost<T> movementCost;

        OpenSet open;
        ClosedSet closed;

        public bool AllowDiagonals = false;

        public AStar (IHeuristic<T> heuristic, IMovementCost<T> movementCost, IUIDGenerator<T> uidGenerator)
        {
            this.heuristic = heuristic;
            this.movementCost = movementCost;
            open = new OpenSet(uidGenerator);
            closed = new ClosedSet(uidGenerator);        
        }

        List<T> ReconstructPath(AStarNode node, T nodeValue)
        {
            AStarNode current = node;
            List<T> retList = new List<T>();
            while (!current.nodeValue.Equals(nodeValue))
            {
                current.parentNode.childNode = current;
                current = current.parentNode;
            }
            while (current != null)
            {
                retList.Add(current.nodeValue);
                current = current.childNode;
            }
            return retList;
        }

        public List<T> GetPath(T start, T end, IGraph<T> grid)
        {

            open.Clear();
            closed.Clear();

            float estimate = heuristic.GetEstimate(start, end);
            open.Add(new AStarNode(start) { childNode = null, parentNode = null, FValue = estimate, GValue = 0.0f, HValue = estimate });
            AStarNode current = null;

            while (open.Count > 0)
            {
                current = open.Pop();
                if (current.nodeValue.Equals(end))
                {
                    break;
                }
                if (!closed.Contains(current))
                    closed.Add(current);
                foreach (T neighbour in grid.GetNeighbours(current.nodeValue))
                {
                    if (closed.Contains(neighbour))
                        continue;

                    float tmpGValue = current.GValue + movementCost.GetMovementCost(current.nodeValue, neighbour);

                    AStarNode n = null;
                    if (!open.Contains(neighbour))
                    {
                        n = new AStarNode(neighbour);
                        n.HValue = heuristic.GetEstimate(n.nodeValue, end);
                        n.parentNode = current;
                        n.GValue = tmpGValue;
                        n.FValue = n.HValue + n.GValue;
                        open.Add(n);
                    }
                    else
                    {
                        n = open.Find(neighbour);
                        if (tmpGValue < n.GValue)
                        {
                            float oldFValue = n.FValue;
                            n.parentNode = current;
                            n.GValue = tmpGValue;                            
                            n.FValue = n.GValue + n.HValue;
                            open.UpdateEntry(oldFValue, n);
                        }
                    }
                }
            }
            return ReconstructPath(current, start);
        }
    }
}
