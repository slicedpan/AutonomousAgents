using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace FiniteStateMachine
{
    public interface State<T>
    {
        // This will be executed when the state is entered
        void Enter(T agent);

        // This is called by the Agent's update function each update step
        void Execute(T agent, GameTime gameTime);

        // This will be executed when the state is exited
        void Exit(T agent);

        // This will be executed when the agent receives a message
        bool OnMessage(T agent, Telegram telegram);
    }


    public class Moving<T> : State<T> where T : Agent
    {
        Location destination;
        IPathFinder<Location> pathFinder;
        List<Location> path;
        State<T> destinationState;
        double gridTime;
        int currentPathPos;

        public Moving(Location destination, IPathFinder<Location> pathFinder, State<T> destinationState)
        {
            this.destination = destination;
            this.pathFinder = pathFinder;
            this.destinationState = destinationState;
        }
        public virtual void Enter(T agent)
        {
            //Printer.Print(agent.Id, "Moving to " + destination.Description);
            if (destination == agent.Location)
            {
                StateMachine<T> stateMachine = agent._StateMachine as StateMachine<T>;
                if (stateMachine != null)
                {
                    stateMachine.ChangeState(destinationState);
                }
                return;
            }                
            path = pathFinder.GetPath(agent.Location, destination, Game1.grid);
            Game1.pathDrawer.AddPath(path);
            currentPathPos = 0;
            gridTime = 0.0d;
        }

        public virtual void Execute(T agent, GameTime gameTime)
        {
            gridTime += gameTime.ElapsedGameTime.TotalSeconds;
            if (gridTime > (1.0d * agent.Location.TravelCost) / agent.Speed)
            {
                gridTime = 0.0d;
                ++currentPathPos;
                if (currentPathPos >= path.Count)
                {
                    StateMachine<T> stateMachine = agent._StateMachine as StateMachine<T>;
                    if (stateMachine != null)
                    {
                        stateMachine.ChangeState(destinationState);
                    }
                }
                else
                    agent.Location = path[currentPathPos];
            }
        }

        public virtual void Exit(T agent)
        {
            Game1.pathDrawer.RemovePath(path);
            //Printer.Print(agent.Id, "Arrived at " + destination.Description);
        }

        public virtual bool OnMessage(T agent, Telegram telegram)
        {
            return false;
        }
    }

}
