using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiniteStateMachine
{
    public class Sheriff : Agent
    {
        private StateMachine<Sheriff> stateMachine;
        public StateMachine<Sheriff> StateMachine
        {
            get { return stateMachine; }
            set { stateMachine = value; }
        }
        private Location location;
        public Location SheriffLocation
        {
            get { return location; }
            set { location = value; }
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
