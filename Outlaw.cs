using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FiniteStateMachine
{
    public class Outlaw : Agent
    {
        private int money = 0;
        public int Money
        {
            get { return money; }
            set { money = value; }
        }

        public bool Alive = true;

        public Outlaw(String name) : base(name)
        {
            location = Location.outlawCamp;
            stateMachine = new StateMachine<Outlaw>(this);
            stateMachine.CurrentState = new Lurking();
            stateMachine.GlobalState = new OutlawGlobalState();
            textureName = "outlaw";
            speed = 4.0d;
        }
        private StateMachine<Outlaw> stateMachine;
        public StateMachine<Outlaw> StateMachine
        {
            get { return stateMachine; }
            set { stateMachine = value; }
        }

        public override void Update(GameTime gameTime)
        {
 
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Alive)
                base.Draw(spriteBatch);
        }

        public override bool HandleMessage(Telegram telegram)
        {
            return stateMachine.HandleMessage(telegram);
        }

        public override void OnSense(Agent other)
        {
            if (other is Sheriff)
            {
                Printer.Print(this, "You'll never take me alive, Sheriff!");
                stateMachine.ChangeState(new OutlawGunfight());
            }
        }
    }
}
