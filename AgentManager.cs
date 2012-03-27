using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace FiniteStateMachine
{
    public static class AgentManager
    {
        static List<Agent> listOfAgents = new List<Agent>();
        public static int AddAgent(Agent agent)
        {
            listOfAgents.Add(agent);
            return listOfAgents.IndexOf(agent);
        }

        public static Agent GetAgent(int id)
        {
            return listOfAgents[id];
        }

        public static void RemoveAgent(Agent agent)
        {
            listOfAgents.Remove(agent);
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (Agent agent in listOfAgents)
            {
                agent.Draw(spriteBatch);
            }
            spriteBatch.End();
        }
        public static void LoadContent(ContentManager content)
        {
            foreach (Agent agent in listOfAgents)
            {
                agent.LoadContent(content);
            }
        }
    }
}
