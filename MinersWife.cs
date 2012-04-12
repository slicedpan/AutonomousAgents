using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace FiniteStateMachine
{
    // This class implements a MinersWife agent; the agent creates and maintains its own 
    // StateMachine that it invokes whenever the game asks it to update (triggered
    // by XNA's Update() method)
    public class MinersWife : Agent
    {
        private StateMachine<MinersWife> stateMachine;
        public StateMachine<MinersWife> StateMachine
        {
            get { return stateMachine; }
            set { stateMachine = value; }
        }

        // This is used to keep track of which other agent is our husband
        private Agent husband;
        public Agent Husband
        {
            get { return husband; }
        }

        private Boolean cooking;
        public Boolean Cooking
        {
            get { return cooking; }
            set { cooking = value; }
        }

        // The constructor invokes the base class constructor, which then creates 
        // an id for the new agent object and then creates and initalises the agent's
        // StateMachine
        public MinersWife(String name) : base(name)
        {
            stateMachine = new StateMachine<MinersWife>(this);
            stateMachine.CurrentState = new DoHouseWork();
            stateMachine.GlobalState = new WifesGlobalState();
            husband = AgentManager.GetAgent("Bob");
            textureName = "minerswife";
            location = Location.shack;
        }

        // This method is invoked by the Game object as a result of XNA updates 
        public override void Update(GameTime gameTime)
        {

        }

        // This method is invoked when the agent receives a message
        public override bool HandleMessage(Telegram telegram)
        {
            return stateMachine.HandleMessage(telegram);    
        }
    }
}
