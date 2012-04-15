using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FiniteStateMachine
{
    public class Undertaker : Agent
    {

        StateMachine<Undertaker> stateMachine;
        public StateMachine<Undertaker> StateMachine
        {
            get
            {
                return stateMachine;
            }
        }

        public Undertaker(String name)
            : base(name)
        {
            textureName = "undertaker";
            stateMachine = new StateMachine<Undertaker>(this);
            stateMachine.GlobalState = new UndertakerGlobalState();
            stateMachine.CurrentState = new WorkingInUndertakers();
            Location = Location.undertakers;
            speed = 4.0d;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override bool HandleMessage(Telegram telegram)
        {
            return stateMachine.HandleMessage(telegram);
        }
    }
}
