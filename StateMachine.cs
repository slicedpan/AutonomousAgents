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
    // This class implements normal states, global states and state blips for a given agent.
    // The agent should create its own StateMachine when its constructor is called.
    public class StateMachine<T> where T : Agent
    {
        private T owner;

        // This holds the current state for the state machine
        private State<T> currentState = null;
        public State<T> CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        // The agent's previous state is needed to implement state blips
        private State<T> previousState = null;
        public State<T> PreviousState
        {
            get { return previousState; }
            set { previousState = value; }
        }

        // The agent's global state is always executed, if it exists
        private State<T> globalState = null;
        public State<T> GlobalState
        {
            get { return globalState; }
            set { globalState = value; }
        }

        // What a lovely constructor
        public StateMachine(T agent)
        {
            owner = agent;
            owner.SetStateMachine(this as Object);
            AgentManager.RegisterCallback(owner.Name, new AgentManager.UpdateCallback(Update));
            Sleep(1.0d);
            GlobalSleep(1.0d);
        }

        public void Cleanup()
        {
            AgentManager.RemoveCallback(owner.Name);
        }

        bool changingState = false;
        State<T> nextState;
        double nextUpdate = 0.0d;
        double lastUpdate = 0.0d;
        double globalNextUpdate = 0.0d;
        double globalLastUpdate = 0.0d;

        // This is called by the Agent whenever the Game invokes the Agent's Update() method
        public void Update(GameTime gameTime)
        {            
            lastUpdate += gameTime.ElapsedGameTime.TotalSeconds;
            globalLastUpdate += gameTime.ElapsedGameTime.TotalSeconds;

            if (globalLastUpdate > globalNextUpdate)
            {
                globalLastUpdate = 0.0d;
                globalNextUpdate = 0.0d;
                if (globalState != null)
                {
                    globalState.Execute(owner, gameTime);
                }
            }
            if (lastUpdate > nextUpdate)
            {
                if (changingState)
                {
                    changingState = false;
                    previousState = currentState;
                    currentState.Exit(owner);
                    currentState = nextState;
                    currentState.Enter(owner);
                }
                else
                {
                    nextUpdate = 0.0d;
                    lastUpdate = 0.0d;
                    if (currentState != null)
                    {
                        currentState.Execute(owner, gameTime);
                    }                    
                }
            }
        }

        public void Sleep(double delayTime)
        {
            lastUpdate = 0.0d;
            nextUpdate = delayTime;
        }

        public void GlobalSleep(double delayTime)
        {
            globalLastUpdate = 0.0d;
            globalNextUpdate = delayTime;
        }

        // This method attempts to deliver a message first via the global state, and if that fails
        // via the current state
        public bool HandleMessage(Telegram telegram)
        {
            if (globalState != null)
            {
                if (globalState.OnMessage(owner, telegram))
                {
                    return true;
                }

            }
            if (currentState != null)
            {
                if (currentState.OnMessage(owner, telegram))
                {
                    return true;
                }
            }
            return false;
        }

        // Switch to a new state and save the old one, so we can revert to it later if it's a state blip
        public void ChangeState(State<T> newState)
        {
            nextState = newState;
            changingState = true;
        }

        // Invoked when a state blip is finished
        public void RevertToPreviousState()
        {
            ChangeState(previousState);
        }

        // Checks whether the machine is in a given state
        public Boolean IsInState(State<T> state)
        {
            return state.GetType().Equals(currentState.GetType());
        }	
    }
}
