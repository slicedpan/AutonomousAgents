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
    abstract public class State<T>
    {
        // This will be executed when the state is entered
        abstract public void Enter(T agent);

        // This is called by the Agent's update function each update step
        abstract public void Execute(T agent, GameTime gameTime);

        // This will be executed when the state is exited
        abstract public void Exit(T agent);

        // This will be executed when the agent receives a message
        abstract public bool OnMessage(T agent, Telegram telegram);
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
        public override void Enter(T agent)
        {
            Printer.Print(agent.Id, "Moving to " + destination.Description);
            if (destination == agent.Location)
            {
                StateMachine<T> stateMachine = agent._StateMachine as StateMachine<T>;
                if (stateMachine != null)
                {
                    stateMachine.ChangeState(destinationState);
                }
                return;
            }                
            path = pathFinder.GetPath(agent.Location, destination, LocationGrid.Locations);
            currentPathPos = 0;
            gridTime = 0.0d;
        }

        public override void Execute(T agent, GameTime gameTime)
        {
            gridTime += gameTime.ElapsedGameTime.TotalSeconds;
            if (gridTime > 1.0d / agent.Speed)
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

        public override void Exit(T agent)
        {
            Printer.Print(agent.Id, "Arrived at " + destination.Description);
        }

        public override bool OnMessage(T agent, Telegram telegram)
        {
            return false;
        }
    }

}
