using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FiniteStateMachine
{
    class Lurking : State<Outlaw>
    {
        public override void Enter(Outlaw agent)
        {

        }

        public override void Execute(Outlaw agent, GameTime gameTime)
        {
            Printer.Print(agent.Id, "Just lurkin' at the " + agent.Location.ToString());
            Printer.Print(agent.Id, String.Format("Coords {0}, {1}", agent.Location.X, agent.Location.Y));
            agent.StateMachine.Sleep(new Random().Next(100) / 20.0d);
            Location nextDest = Location.cemetery;
            if (agent.Location == Location.cemetery)
                nextDest = Location.outlawCamp;
            agent.StateMachine.ChangeState(new Moving<Outlaw>(nextDest, PathFinderProvider.Get(), new Lurking()));            
        }

        public override void Exit(Outlaw agent)
        {
            Printer.Print(agent.Id, "Leavin' the " + agent.Location.ToString());
        }

        public override bool OnMessage(Outlaw agent, Telegram telegram)
        {
            return true;
        }
    }
    class RobbingBank : State<Outlaw>
    {

        public override void Enter(Outlaw agent)
        {
            Printer.Print(agent.Id, "Headin' over to the bank to steal some gold!");
        }

        public override void Execute(Outlaw agent, GameTime gameTime)
        {
            agent.StateMachine.Sleep(1.0d);
            agent.StateMachine.ChangeState(new Moving<Outlaw>(Location.outlawCamp, PathFinderProvider.Get(), new Lurking()));
        }

        public override void Exit(Outlaw agent)
        {
            int goldStolen = new Random().Next(10) + 1;
            agent.Money += goldStolen;
            Printer.Print(agent.Id, "Stole " + goldStolen + " gold from the bank! I now have " + agent.Money + " gold");
            Printer.Print(agent.Id, "Leaving the bank");
        }

        public override bool OnMessage(Outlaw agent, Telegram telegram)
        {
            return true;
        }
    }

    class Resurrect : State<Outlaw>
    {

        public override void Enter(Outlaw agent)
        {
            Printer.Print(agent.Id, "Resurrecting");
        }

        public override void Execute(Outlaw agent, GameTime gameTime)
        {
            agent.Location = Location.outlawCamp;
            agent.StateMachine.ChangeState(new Lurking());            
        }

        public override void Exit(Outlaw agent)
        {
            
        }

        public override bool OnMessage(Outlaw agent, Telegram telegram)
        {
            return false;
        }
    }

    class DeadState : State<Outlaw>
    {
        public override void Enter(Outlaw agent)
        {
        }

        public override void Execute(Outlaw agent, GameTime gameTime)
        {
            agent.StateMachine.Sleep(20.0d);                        
            agent.StateMachine.ChangeState(new Resurrect());
        }

        public override void Exit(Outlaw agent)
        {

        }

        public override bool OnMessage(Outlaw agent, Telegram telegram)
        {
            return false;
        }
    }

    class OutlawGlobalState : State<Outlaw>
    {
        public override void Enter(Outlaw agent)
        {
        }

        public override void Execute(Outlaw agent, GameTime gameTime)
        {
            agent.StateMachine.GlobalSleep(1.0d);
            if (new Random().Next(30) == 0 && !agent.StateMachine.IsInState(new RobbingBank()))
            {
                agent.StateMachine.ChangeState(new Moving<Outlaw>(Location.bank, PathFinderProvider.Get(), new RobbingBank()));
            }
        }

        public override void Exit(Outlaw agent)
        {

        }

        public override bool OnMessage(Outlaw agent, Telegram telegram)
        {
            return false;
        }
    }
}
