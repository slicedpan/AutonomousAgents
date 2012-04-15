using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FiniteStateMachine
{

    public class RetrieveCorpse : State<Undertaker>
    {

        public void Enter(Undertaker agent)
        {
            Printer.Print(agent, "I'm at the " + agent.Location.Description + ", let's get this fella out of here");
            agent.StateMachine.Sleep(1.0d);
        }

        public void Execute(Undertaker agent, GameTime gameTime)
        {
            if (agent.Location.Corpses > 0)
                agent.Location.Corpses -= 1;
            agent.StateMachine.RevertToPreviousState();
        }

        public void Exit(Undertaker agent)
        {
        }

        public bool OnMessage(Undertaker agent, Telegram telegram)
        {
            return true;
        }
    }

    public class BuryCorpse : State<Undertaker>
    {

        public void Enter(Undertaker agent)
        {
            Printer.Print(agent, "Let's find a grave for you");
            agent.StateMachine.Sleep(1.0d);
        }

        public void Execute(Undertaker agent, GameTime gameTime)
        {
            Printer.Print(agent, "That'll do nicely. Rest in peace");
            agent.StateMachine.RevertToPreviousState();
        }

        public void Exit(Undertaker agent)
        {
        }

        public bool OnMessage(Undertaker agent, Telegram telegram)
        {
            return true;
        }
    }

    public class RetrieveAndBuryCorpse : State<Undertaker>
    {

        Location corpseLocation;

        public RetrieveAndBuryCorpse(Location corpseLocation)
        {
            this.corpseLocation = corpseLocation;
        }

        bool reEntry = false;

        public void Enter(Undertaker agent)
        {
            if (reEntry)
                return;
            Printer.Print(agent, "Got word of a body at the " + corpseLocation.Description + ", better go get it");
            reEntry = true;
        }

        bool retrievedCorpse = false;
        bool buriedCorpse = false;

        public void Execute(Undertaker agent, GameTime gameTime)
        {
            if (!retrievedCorpse)
            {                
                retrievedCorpse = true;
                agent.StateMachine.ChangeState(new Moving<Undertaker>(corpseLocation, PathFinderProvider.Get(), new RetrieveCorpse()));
                return;
            }
            if (!buriedCorpse)
            {
                buriedCorpse = true;
                agent.StateMachine.ChangeState(new Moving<Undertaker>(Location.cemetery, PathFinderProvider.Get(), new BuryCorpse()));
                return;
            }
            agent.StateMachine.ChangeState(new Moving<Undertaker>(Location.undertakers, PathFinderProvider.Get(), new WorkingInUndertakers()));
        }

        public void Exit(Undertaker agent)
        {
        }

        public bool OnMessage(Undertaker agent, Telegram telegram)
        {
            return true;
        }
    }

    public class WorkingInUndertakers : State<Undertaker>
    {
        Random rand = new Random();
        public void Enter(Undertaker agent)
        {

        }

        public void Execute(Undertaker agent, GameTime gameTime)
        {
            int job = rand.Next(3);
            switch (job)
            {
                case 0:
                    Printer.Print(agent, "Jus' doing some embalming");
                    break;
                case 1:
                    Printer.Print(agent, "Making a coffin");
                    break;
                case 2:
                    Printer.Print(agent, "Cleaning up a bit");
                    break;
            }
            agent.StateMachine.Sleep(3.0d);
        }

        public void Exit(Undertaker agent)
        {

        }

        public bool OnMessage(Undertaker agent, Telegram telegram)
        {
            return false;
        }
    }

    public class UndertakerGlobalState : State<Undertaker>
    {

        public void Enter(Undertaker agent)
        {
        }

        public void Execute(Undertaker agent, GameTime gameTime)
        {
            agent.StateMachine.GlobalSleep(0.2d);
        }

        public void Exit(Undertaker agent)
        {
        }

        public bool OnMessage(Undertaker agent, Telegram telegram)
        {
            if (telegram.messageType == MessageType.CorpseLocation)
            {
                Location corpseLocation = telegram.extraInfo as Location;
                agent.StateMachine.ChangeState(new RetrieveAndBuryCorpse(corpseLocation));
                return true;
            }
            else if (telegram.messageType == MessageType.Howdy)
            {
                Printer.Print(agent, "Howdy Sheriff!");
                return true;
            }
            return false;
        }   
    }
}
