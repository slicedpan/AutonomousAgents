using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace FiniteStateMachine
{
    // In this state, the MinersWife agent does random house work
    public class DoHouseWork : State<MinersWife>
    {
        static Random rand = new Random();

        public void Enter(MinersWife minersWife)
        {
            Printer.Print(minersWife, "Time to do some more housework!");
            minersWife.StateMachine.Sleep(0.5d);
        }

        public void Execute(MinersWife minersWife, GameTime gameTime)
        {
            switch (rand.Next(3))
            {
                case 0:
                    Printer.Print(minersWife, "Moppin' the floor");
                    break;
                case 1:
                    Printer.Print(minersWife, "Washin' the dishes");
                    break;
                case 2:
                    Printer.Print(minersWife, "Makin' the bed");
                    break;
                default:
                    break;
            }
            minersWife.StateMachine.Sleep(3.0d);

        }

        public void Exit(MinersWife minersWife)
        {

        }

        public bool OnMessage(MinersWife minersWife, Telegram telegram)
        {
            return false;
        }
    }

    // In this state, the MinersWife agent goes to the loo
    public class VisitBathroom : State<MinersWife>
    {
        public void Enter(MinersWife minersWife)
        {
            Printer.Print(minersWife, "Walkin' to the can. Need to powda mah pretty li'lle nose");
        }

        public void Execute(MinersWife minersWife, GameTime gameTime)
        {
            Printer.Print(minersWife, "Ahhhhhh! Sweet relief!");
            minersWife.StateMachine.Sleep(4.0d);
            minersWife.StateMachine.RevertToPreviousState();  // this completes the state blip
        }

        public void Exit(MinersWife minersWife)
        {
            Printer.Print(minersWife, "Leavin' the Jon");
        }

        public bool OnMessage(MinersWife minersWife, Telegram telegram)
        {
            return false;
        }
    }

    // In this state, the MinersWife prepares food
    public class CookStew : State<MinersWife>
    {
        public void Enter(MinersWife minersWife)
        {
            if (!minersWife.Cooking)
            {
                // MinersWife sends a delayed message to herself to arrive when the food is ready
                Printer.Print(minersWife, "Putting the stew in the oven");
                Message.DispatchMessage(4.0d, minersWife.Id, minersWife.Id, MessageType.StewsReady);
                minersWife.Cooking = true;
            }
        }

        public void Execute(MinersWife minersWife, GameTime gameTime)
        {
            Printer.Print(minersWife, "Fussin' over food");
            minersWife.StateMachine.Sleep(4.0d);
        }

        public void Exit(MinersWife minersWife)
        {
            Printer.Print(minersWife, "Puttin' the stew on the table");
        }

        public bool OnMessage(MinersWife minersWife, Telegram telegram)
        {
            switch (telegram.messageType)
            {
                case MessageType.HiHoneyImHome:
                    // Ignored here; handled in WifesGlobalState below
                    return false;
                case MessageType.StewsReady:
                    // Tell Miner that the stew is ready now by sending a message with no delay
                    Printer.PrintMessageData("Message handled by " + minersWife + " at time ");
                    Printer.Print(minersWife, "StewReady! Lets eat");
                    Message.DispatchMessage(0, minersWife.Id, minersWife.Husband.Id, MessageType.StewsReady);
                    minersWife.Cooking = false;
                    minersWife.StateMachine.ChangeState(new DoHouseWork());
                    return true;
                default:
                    return false;
            }
        }
    }

    // If the agent has a global state, then it is executed every Update() cycle
    public class WifesGlobalState : State<MinersWife>
    {
        static Random rand = new Random();

        public void Enter(MinersWife minersWife)
        {
           
        }

        public void Execute(MinersWife minersWife, GameTime gameTime)
        {
            // There's always a 10% chance of a state blip in which MinersWife goes to the bathroom
            if (rand.Next(10) == 1 && !minersWife.StateMachine.IsInState(new VisitBathroom()))
            {
                minersWife.StateMachine.ChangeState(new VisitBathroom());
            }
            minersWife.StateMachine.GlobalSleep(2.0d);
        }

        public void Exit(MinersWife minersWife)
        {

        }

        public bool OnMessage(MinersWife minersWife, Telegram telegram)
        {
            switch (telegram.messageType)
            {
                case MessageType.HiHoneyImHome:
                    Printer.Print(minersWife, "Hi honey. Let me make you some of mah fine country stew");
                    minersWife.StateMachine.ChangeState(new CookStew());
                    return true;
                case MessageType.StewsReady:
                    return false;
                case MessageType.Howdy:
                    Printer.Print(minersWife, "Howdy Sheriff!");
                    return true;
                default:
                    return false;
            }                 
        }
    }
}
