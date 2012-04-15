using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FiniteStateMachine
{
    class Lurking : State<Outlaw>
    {
        public void Enter(Outlaw agent)
        {

        }

        public void Execute(Outlaw agent, GameTime gameTime)
        {
            Printer.Print(agent, "Just lurkin' at the " + agent.Location.ToString());
            agent.StateMachine.Sleep(new Random().Next(100) / 20.0d);
            Location nextDest = Location.cemetery;
            if (agent.Location == Location.cemetery)
                nextDest = Location.outlawCamp;
            agent.StateMachine.ChangeState(new Moving<Outlaw>(nextDest, PathFinderProvider.Get(), new Lurking()));            
        }

        public void Exit(Outlaw agent)
        {
            Printer.Print(agent, "Leavin' the " + agent.Location.ToString());
        }

        public bool OnMessage(Outlaw agent, Telegram telegram)
        {
            return false;
        }
    }
    class RobbingBank : State<Outlaw>
    {

        public void Enter(Outlaw agent)
        {
            Printer.Print(agent, "Headin' over to the bank to steal some gold!");
            Message.DispatchMessage(0.0d, agent.Id, AgentManager.GetAgent("Wyatt").Id, MessageType.BankRobbery);
        }

        public void Execute(Outlaw agent, GameTime gameTime)
        {
            agent.StateMachine.Sleep(1.0d);
            agent.StateMachine.ChangeState(new Moving<Outlaw>(Location.outlawCamp, PathFinderProvider.Get(), new Lurking()));
        }

        public void Exit(Outlaw agent)
        {
            int goldStolen = new Random().Next(10) + 1;
            agent.Money += goldStolen;
            Printer.Print(agent, "Stole " + goldStolen + " gold from the bank! I now have " + agent.Money + " gold");
        }

        public bool OnMessage(Outlaw agent, Telegram telegram)
        {
            return false;    
        }
    }    

    class OutlawDead: State<Outlaw>
    {
        public void Enter(Outlaw agent)
        {
            agent.Alive = false;
        }

        public void Execute(Outlaw agent, GameTime gameTime)
        {
            agent.StateMachine.Sleep(40.0d);
            agent.Location = Location.outlawCamp;          
            agent.StateMachine.ChangeState(new Lurking());
        }

        public void Exit(Outlaw agent)
        {
            agent.Alive = true;
        }

        public bool OnMessage(Outlaw agent, Telegram telegram)
        {
            return true;
        }
    }

    class OutlawGlobalState : State<Outlaw>
    {
        public void Enter(Outlaw agent)
        {
        }

        public void Execute(Outlaw agent, GameTime gameTime)
        {
            agent.StateMachine.GlobalSleep(1.0d);
            if (new Random().Next(60) == 0 && !agent.StateMachine.IsInState(new RobbingBank()))
            {
                agent.StateMachine.ChangeState(new Moving<Outlaw>(Location.bank, PathFinderProvider.Get(), new RobbingBank()));
            }
        }

        public void Exit(Outlaw agent)
        {

        }

        public bool OnMessage(Outlaw agent, Telegram telegram)
        {
            if (telegram.messageType == MessageType.KillMessage)
                agent.StateMachine.ChangeState(new OutlawDead());
            return true;
        }
    }

    class OutlawGunfight : State<Outlaw>
    {

        public void Enter(Outlaw agent)
        {
        }

        public void Execute(Outlaw agent, GameTime gameTime)
        {
        }

        public void Exit(Outlaw agent)
        {
        }

        public bool OnMessage(Outlaw agent, Telegram telegram)
        {
            switch (telegram.messageType)
            {
                case MessageType.KillMessage:
                    agent.StateMachine.ChangeState(new OutlawDead());
                    return true; 
                case MessageType.FightOver:
                    agent.StateMachine.ChangeState(new Lurking());
                    Printer.Print(agent, "Take that Sheriff!");
                    return true;
                default:
                    return false;
            }
        }
    }
}
