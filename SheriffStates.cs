using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiniteStateMachine
{
    class Patrolling : State<Sheriff>
    {
        public override void Enter(Sheriff agent)
        {

        }

        public override void Execute(Sheriff agent, Microsoft.Xna.Framework.GameTime gameTime)
        {

        }

        public override void Exit(Sheriff agent)
        {

        }

        public override bool OnMessage(Sheriff agent, Telegram telegram)
        {
            return true;
        }
    }
}
