using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiniteStateMachine
{
    class Lurking : State<Outlaw>
    {
        int lurkCycles;
        int cycleCounter;
        public override void Enter(Outlaw agent)
        {
            lurkCycles = new Random().Next(5);

            if (agent.Location == Location.cemetery)
                agent.Location = Location.outlawCamp;
            else
                agent.Location = Location.cemetery;

            cycleCounter = 0;
            Printer.Print(agent.Id, "Headin' to the " + agent.Location.ToString() + " to hang out for a while");
        }

        public override void Execute(Outlaw agent)
        {
            Printer.Print(agent.Id, "Just lurkin' at the " + agent.Location.ToString());
            Printer.Print(agent.Id, String.Format("Coords {0}, {1}", agent.Location.X, agent.Location.Y));
            ++cycleCounter;
            if (cycleCounter > lurkCycles)
            {
                agent.StateMachine.ChangeState(new Lurking());
            }
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

        public override void Execute(Outlaw agent)
        {
            int goldStolen = new Random().Next(10) + 1;
            agent.Money += goldStolen;
            Printer.Print(agent.Id, "Stole " + goldStolen + " gold from the bank! I now have " + agent.Money + " gold");
            agent.StateMachine.RevertToPreviousState();
        }

        public override void Exit(Outlaw agent)
        {
            Printer.Print(agent.Id, "Leaving the bank");
        }

        public override bool OnMessage(Outlaw agent, Telegram telegram)
        {
            return true;
        }
    }

    class GlobalState : State<Outlaw>
    {
        public override void Enter(Outlaw agent)
        {
        }

        public override void Execute(Outlaw agent)
        {
            if (new Random().Next(10) == 0 && !agent.StateMachine.IsInState(new RobbingBank()))
            {
                agent.StateMachine.ChangeState(new RobbingBank());
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
