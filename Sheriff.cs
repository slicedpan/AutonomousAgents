using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiniteStateMachine
{
    public class Sheriff : Agent
    {
        private StateMachine<Sheriff> stateMachine;
        public Sheriff()
        {
            textureName = "sheriff";
        }
        public StateMachine<Sheriff> StateMachine
        {
            get { return stateMachine; }
            set { stateMachine = value; }
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public override bool HandleMessage(Telegram telegram)
        {
            throw new NotImplementedException();
        }
    }
}
