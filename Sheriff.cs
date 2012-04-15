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
            stateMachine.CurrentState = new Patrolling();
            stateMachine.GlobalState = new SheriffGlobalState();
            location = Location.sheriffsOffice;
            speed = 4.0f;
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
            return stateMachine.HandleMessage(telegram);
        }

        public override void OnSense(Agent other)
        {
            if (other is Outlaw)
            {
                stateMachine.ChangeState(new SheriffGunfight(other));
            }
            else
            {
                Printer.Print(this, "Howdy, " + other.Name);
                Message.DispatchMessage(0.0d, Id, other.Id, MessageType.Howdy);
            }
        }
        public override void OnUnsense(Agent other)
        {
            if (!(other is Outlaw))
            {
                Printer.Print(this, "So long, " + other.Name);
            }
        }
    }
}
