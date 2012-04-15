using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FiniteStateMachine
{

    public class PatrolMove : Moving<Sheriff>
    {
        public PatrolMove(Location destination, IPathFinder<Location> pathFinder, State<Sheriff> destinationState)
            : base(destination, pathFinder, destinationState)
        {}
        public override bool OnMessage(Sheriff agent, Telegram telegram)
        {
            return true;
        }
    }

    public class ReturnMoneyToBank : State<Sheriff>
    {

        int amount;

        public ReturnMoneyToBank(int amount)
        {
            this.amount = amount;
        }

        public void Enter(Sheriff agent)
        {
            Printer.Print(agent, "I'll put this gold back where it belongs");
            agent.StateMachine.Sleep(1.0d);
        }

        public void Execute(Sheriff agent, GameTime gameTime)
        {
            agent.StateMachine.RevertToPreviousState();
        }

        public void Exit(Sheriff agent)
        {
        }

        public bool OnMessage(Sheriff agent, Telegram telegram)
        {
            return true;
        }
    }

    public class DrinkInSaloon : State<Sheriff>
    {

        public void Enter(Sheriff agent)
        {
            Printer.Print(agent, "A nice cool drink will soothe mah frayed nerves");
            agent.StateMachine.Sleep(1.0d);
        }

        public void Execute(Sheriff agent, GameTime gameTime)
        {
            agent.StateMachine.Sleep(1.0d);
            Printer.Print(agent, "That's some good liquor!");
            agent.StateMachine.ChangeState(new Patrolling());
        }

        public void Exit(Sheriff agent)
        {
        }

        public bool OnMessage(Sheriff agent, Telegram telegram)
        {
            return false;
        }
    }

    public class VisitUndertaker : State<Sheriff>
    {
        Location corpseLocation;
        public VisitUndertaker(Location corpseLocation)
        {
            this.corpseLocation = corpseLocation;
        }

        public void Enter(Sheriff agent)
        {
            Printer.Print(agent, "Howdy Mort, Got a dead one up at the " + corpseLocation.Description);
            //Message.DispatchMessage(0.0d, agent.Id, AgentManager.GetAgent("Mort").Id, MessageType.CorpseLocation, corpseLocation);
            agent.StateMachine.Sleep(1.0d);
        }

        public void Execute(Sheriff agent, GameTime gameTime)
        {
            agent.StateMachine.RevertToPreviousState();
        }

        public void Exit(Sheriff agent)
        {
        }

        public bool OnMessage(Sheriff agent, Telegram telegram)
        {
            return true;
        }
    }

    public class Cleanup : State<Sheriff>
    {
        int money = 0;
        Location corpseLocation;
        public Cleanup(Agent other)
        {
            if (other is Outlaw)
            {
                Outlaw outlaw = other as Outlaw;
                money = outlaw.Money;
                returnedMoney = false;
            }
            corpseLocation = other.Location;
        }

        bool reEntry = false;

        public void Enter(Sheriff agent)
        {
            if (reEntry)
                return;
            Printer.Print(agent, "That'll learn ya!");
            reEntry = true;
        }

        bool returnedMoney = false;
        bool visitedUndertaker = false;

        public void Execute(Sheriff agent, GameTime gameTime)
        {
            agent.StateMachine.Sleep(0.5d);
            if (!returnedMoney)
            {
                agent.StateMachine.ChangeState(new PatrolMove(Location.bank, PathFinderProvider.Get(), new ReturnMoneyToBank(money)));
                returnedMoney = true;
                return;
            }
            if (!visitedUndertaker)
            {
                agent.StateMachine.ChangeState(new PatrolMove(Location.undertakers, PathFinderProvider.Get(), new VisitUndertaker(corpseLocation)));
                visitedUndertaker = true;
                return;
            }
            agent.StateMachine.ChangeState(new DrinkInSaloon());
        }

        public void Exit(Sheriff agent)
        {
        }

        public bool OnMessage(Sheriff agent, Telegram telegram)
        {
            return true;    //ignore messages until we're finished
        }
    }

    public class SheriffDead : State<Sheriff>
    {

        public void Enter(Sheriff agent)
        {
            agent.StateMachine.Sleep(15.0d);
            agent.StateMachine.GlobalSleep(15.0d);
        }

        public void Execute(Sheriff agent, GameTime gameTime)
        {
            agent.Location = Location.sheriffsOffice;
            agent.StateMachine.ChangeState(new Patrolling());
        }

        public void Exit(Sheriff agent)
        {
        }

        public bool OnMessage(Sheriff agent, Telegram telegram)
        {
            return true;
        }
    }

    public class SheriffGunfight : State<Sheriff>
    {

        Agent other;
        Random rand = new Random();

        public SheriffGunfight(Agent other)
        {
            this.other = other;
        }

        public void Enter(Sheriff agent)
        {
            Printer.Print(agent, "I'm gonna get you, ya pesky varmint!");
            agent.StateMachine.Sleep(1.0d);
        }

        public void Execute(Sheriff agent, GameTime gameTime)
        {
            if (rand.Next(10) >= 3)
            {
                Message.DispatchMessage(0.0d, agent.Id, other.Id, MessageType.KillMessage);
                agent.StateMachine.ChangeState(new Cleanup(other));                
            }
            else
            {
                Message.DispatchMessage(0.0d, agent.Id, other.Id, MessageType.FightOver);
                agent.StateMachine.ChangeState(new SheriffDead());
            }
        }

        public void Exit(Sheriff agent)
        {
        }

        public bool OnMessage(Sheriff agent, Telegram telegram)
        {
            return true;
        }
    }

    public class Patrolling : State<Sheriff>
    {
        public void Enter(Sheriff agent)
        {
            agent.StateMachine.Sleep(1.0d);
        }

        public void Execute(Sheriff agent, GameTime gameTime)
        {
            Random rand = new Random();
            Location location = Game1.grid.NamedLocations.Values.ElementAt<Location>(rand.Next(Game1.grid.NamedLocations.Count - 1));
            if (location == Location.outlawCamp)
                location = Location.sheriffsOffice;
            Printer.Print(agent, "Gonna go check out the " + location.Description);
            agent.StateMachine.ChangeState(new Moving<Sheriff>(location, PathFinderProvider.Get(), new Patrolling()));
        }

        public void Exit(Sheriff agent)
        {

        }

        public bool OnMessage(Sheriff agent, Telegram telegram)
        {
            return true;
        }
    }
    public class SheriffGlobalState : State<Sheriff>
    {

        public void Enter(Sheriff agent)
        {
        }

        public void Execute(Sheriff agent, GameTime gameTime)
        {
            agent.StateMachine.GlobalSleep(1.0d);
        }

        public void Exit(Sheriff agent)
        {
        }

        public bool OnMessage(Sheriff agent, Telegram telegram)
        {
            return false;
        }
    }
}
