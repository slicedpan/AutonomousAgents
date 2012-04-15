using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace FiniteStateMachine
{
    public static class Printer
    {
        public static int max = 20;
        public static String[] display = new String[max];
        public static int[] displaytype = new int[max];
        public static int top = 0;
        public static int amount = 0;
        public static Vector2 offset = new Vector2(0.0f, 0.0f);
        static int lastColour = 0;
        static Color[] colours = new Color[8]
            {
                Color.White,
                Color.Red,
                Color.Purple,
                Color.Gray,
                Color.Gold,
                Color.Green,
                Color.Honeydew,
                Color.Indigo
            };

        public static void Print(int id, string message)
        {
            System.Console.WriteLine(id + " " + message + "\n");
            top++;
            if (top >= max) { top = 0; }
            if (amount < 20) { amount++; }
            display[top] = id + " " + message;
            displaytype[top] = id;
        }

        public static void Print(Agent agent, string message)
        {
            String completeMessage = agent.Name + ": " + message;
            System.Console.WriteLine(completeMessage + "\n");
            top++;
            if (top >= max) { top = 0; }
            if (amount < 20) { amount++; }
            display[top] = completeMessage;
            displaytype[top] = agent.Id;
        }

        public static void PrintMessageData(string message)
        {
            System.Console.WriteLine("M " + message + "\n");
            top++;
            if (top >= max) { top = 0; }
            if (amount < 20) { amount++; }
            display[top] =  message;
            displaytype[top] = 5;
        }

        public static void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            spriteBatch.Begin();
            int index = top;
            for (int i = 0; i < amount; i++)
            {
                spriteBatch.DrawString(spriteFont, display[index], offset + new Vector2(0, i * 20), colours[displaytype[index] % colours.Length]);
                index--;
                if (index < 0)
                {
                    index = max - 1;
                }
            }
            spriteBatch.End();
        }
    }
}
