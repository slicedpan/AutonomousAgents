using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FiniteStateMachine
{
    public class Sheriff : Agent
    {
        StateMachine<Sheriff> stateMachine;

        public Sheriff(String name) : base(name)
        {
            textureName = "sheriff";
            stateMachine = new StateMachine<Sheriff>(this);            
        }
        public StateMachine<Sheriff> StateMachine
        {
            get { return stateMachine; }           
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override bool HandleMessage(Telegram telegram)
        {
            return true;
        }
    }
}
