using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        private Location location;
        public Location OutlawLocation
        {
            get { return location; }
            set { location = value; }
        }
        public Outlaw()
        {
            location = Location.outlawCamp;
            stateMachine = new StateMachine<Outlaw>(this);
            stateMachine.CurrentState = new Lurking();
            stateMachine.GlobalState = new GlobalState();
        }
        private StateMachine<Outlaw> stateMachine;
        public StateMachine<Outlaw> StateMachine
        {
            get { return stateMachine; }
            set { stateMachine = value; }
        }

        public override void Update()
        {
            stateMachine.Update();
        }

        public override bool HandleMessage(Telegram telegram)
        {
            return stateMachine.HandleMessage(telegram);
        }
    }
}
