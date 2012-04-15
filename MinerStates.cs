using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace FiniteStateMachine
{
    // This class implements the state in which the Miner agent mines for gold
    public class EnterMineAndDigForNugget : State<Miner>
    {
        public void Enter(Miner miner)
        {
            Printer.Print(miner, "Walkin' to the goldmine");
            miner.Location = Location.goldMine;
        }

        public void Execute(Miner miner, GameTime gameTime)
        {
            miner.GoldCarrying += 1;
            miner.HowFatigued += 1;
            Printer.Print(miner, "Pickin' up a nugget");
            miner.StateMachine.Sleep(1.0d);
            if (miner.PocketsFull())
            {
                miner.StateMachine.ChangeState(new Moving<Miner>(Location.bank, PathFinderProvider.Get(), new VisitBankAndDepositGold()));
            }
            if (miner.Thirsty() && miner.MoneyInBank > 2)
            {
                miner.StateMachine.ChangeState(new Moving<Miner>(Location.saloon, PathFinderProvider.Get(), new QuenchThirst()));
            }
        }

        public void Exit(Miner miner)
        {
            Printer.Print(miner, "Ah'm leaving the gold mine with mah pockets full o' sweet gold");
        }

        public bool OnMessage(Miner agent, Telegram telegram)
        {
            return false;    
        }
    }

    // In this state, the miner goes to the bank and deposits gold
    public class VisitBankAndDepositGold : State<Miner>
    {
        public void Enter(Miner miner)
        {
            Printer.Print(miner, "Goin' to the bank. Yes siree");
            miner.Location = Location.bank;
        }

        public void Execute(Miner miner, GameTime gameTime)
        {
            miner.MoneyInBank += miner.GoldCarrying;
            miner.GoldCarrying = 0;
            Printer.Print(miner, "Depositing gold. Total savings now: " + miner.MoneyInBank);
            miner.StateMachine.Sleep(1.0d);
            if (miner.Rich())
            {
                Printer.Print(miner, "WooHoo! Rich enough for now. Back home to mah li'lle lady");
                miner.StateMachine.ChangeState(new Moving<Miner>(Location.shack, PathFinderProvider.Get(), new GoHomeAndSleepTillRested()));
            }
            else
            {
                miner.StateMachine.ChangeState(new Moving<Miner>(Location.goldMine, PathFinderProvider.Get(), new EnterMineAndDigForNugget()));
            }
        }

        public void Exit(Miner miner)
        {
            Printer.Print(miner, "Leavin' the Bank");
        }

        public bool OnMessage(Miner agent, Telegram telegram)
        {
            return false;
        }
    }

    // In this state, the miner goes home and sleeps
    public class GoHomeAndSleepTillRested : State<Miner>
    {
        public void Enter(Miner miner)
        {
            Printer.Print(miner, "Time for a nap");
            Message.DispatchMessage(0, miner.Id, miner.WifeId, MessageType.HiHoneyImHome);
        }

        public void Execute(Miner miner, GameTime gameTime)
        {
            miner.StateMachine.Sleep(1.0d);
            if (miner.HowFatigued < miner.TirednessThreshold)
            {
                Printer.Print(miner, "All mah fatigue has drained away. Time to find more gold!");
                miner.StateMachine.ChangeState(new Moving<Miner>(Location.goldMine, PathFinderProvider.Get(), new EnterMineAndDigForNugget()));
            }
            else
            {
                miner.HowFatigued--;
                Printer.Print(miner, "ZZZZZ....");
            }
        }

        public void Exit(Miner miner)
        {

        }

        public bool OnMessage(Miner miner, Telegram telegram)
        {
            switch (telegram.messageType)
            {
                case MessageType.HiHoneyImHome:
                    return false;
                case MessageType.StewsReady:
                    Printer.PrintMessageData("Message handled by " + miner.Id + " at time ");
                    Printer.Print(miner, "Okay Hun, ahm a comin'!");
                    miner.StateMachine.ChangeState(new EatStew());
                    return true; 
                default:
                    return false;
            }
        }
    }

    // In this state, the miner goes to the saloon to drink
    public class QuenchThirst : State<Miner>
    {
        public void Enter(Miner miner)
        {
            
        }

        public void Execute(Miner miner, GameTime gameTime)
        {
            // Buying whiskey costs 2 gold but quenches thirst altogether
            miner.StateMachine.Sleep(2.0d);
            miner.HowThirsty = 0;
            miner.MoneyInBank -= 2;
            Printer.Print(miner, "That's mighty fine sippin' liquer");
            miner.StateMachine.ChangeState(new Moving<Miner>(Location.goldMine, PathFinderProvider.Get(), new EnterMineAndDigForNugget()));
        }

        public void Exit(Miner miner)
        {
            Printer.Print(miner, "Leaving the saloon, feelin' good");
        }

        public bool OnMessage(Miner agent, Telegram telegram)
        {
            return false;
        }
    }

    // In this state, the miner eats the food that Elsa has prepared
    public class EatStew : State<Miner>
    {
        public void Enter(Miner miner)
        {
            Printer.Print(miner, "Smells Reaaal goood Elsa!");
        }

        public void Execute(Miner miner, GameTime gameTime)
        {
            miner.StateMachine.Sleep(1.0d);
            Printer.Print(miner, "Tastes real good too!");
            miner.StateMachine.RevertToPreviousState();
        }

        public void Exit(Miner miner)
        {
            Printer.Print(miner, "Thankya li'lle lady. Ah better get back to whatever ah wuz doin'");
        }

        public bool OnMessage(Miner agent, Telegram telegram)
        {
            return false;
        }
    }

    // If the agent has a global state, then it is executed every Update() cycle
    public class MinerGlobalState : State<Miner>
    {
        public void Enter(Miner miner)
        {

        }

        public void Execute(Miner miner, GameTime gameTime)
        {
            miner.StateMachine.GlobalSleep(1.0d);
            miner.HowThirsty += 1;
        }
        
        public void Exit(Miner miner)
        {

        }

        public bool OnMessage(Miner agent, Telegram telegram)
        {
            if (telegram.messageType == MessageType.Howdy)
            {
                Printer.Print(agent, "Howdy Sheriff!");
            }
            return false;
        }
    }
}
