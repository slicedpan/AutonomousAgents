using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace FiniteStateMachine
{
    public static class AgentManager
    {
        static SortedDictionary<String, Agent> agents = new SortedDictionary<string,Agent>();
        static SortedDictionary<int, Agent> agentsByID = new SortedDictionary<int,Agent>(); //necessary for messages

        public delegate void UpdateCallback(GameTime gameTime);

        static SortedDictionary<String, UpdateCallback> updateCallbacks = new SortedDictionary<string,UpdateCallback>();

        static public void RegisterCallback(String name, UpdateCallback callback)
        {
            updateCallbacks.Add(name, callback);
        }

        static public void RemoveCallback(String name)
        {
            updateCallbacks.Remove(name);
        }

        public static void AddAgent(Agent agent)
        {
            agents.Add(agent.Name, agent);
            agentsByID.Add(agent.Id, agent);
        }

        public static Agent GetAgent(String agentName)
        {
            return agents[agentName];
        }

        public static Agent GetAgent(int AgentID)
        {
            return agentsByID[AgentID];
        }

        public static List<Agent> GetAgentsAt(Location location)
        {
            List<Agent> retList = new List<Agent>();
            foreach (KeyValuePair<String, Agent> kvp in agents)
            {
                if (kvp.Value.Location == location)
                    retList.Add(kvp.Value);
            }
            return retList;
        }

        public static void RemoveAgent(Agent agent)
        {
            agents.Remove(agent.Name);
            agentsByID.Remove(agent.Id);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (Agent agent in agents.Values)
            {
                agent.Draw(spriteBatch);
            }
            spriteBatch.End();
        }

        public static void LoadContent(ContentManager content)
        {
            foreach (Agent agent in agents.Values)
            {
                agent.LoadContent(content);
            }
        }

        public static void Update(GameTime gameTime)
        {
            foreach (Agent agent in agents.Values)
            {
                agent.Update(gameTime);                
            }
            foreach (UpdateCallback callback in updateCallbacks.Values)
            {
                callback.Invoke(gameTime);
            }
        }
    }
}
