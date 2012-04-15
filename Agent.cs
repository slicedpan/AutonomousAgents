using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace FiniteStateMachine
{
    abstract public class Agent
    {
        private static int agents = 0;
        Texture2D _texture;
        Rectangle spriteRect;
        // Every agent has a numerical id that is set when it is created
        private int id;
        protected double speed = 1.0d; //amount of cells that the agent can pass through in one second unhindered

        protected int sightRange = 8;
        public int SightRange
        {
            get
            {
                return sightRange;
            }
        }

        public double Speed
        {
            get
            {
                return speed;
            }
        }

        private List<Agent> sensedAgents;
        public List<Agent> SensedAgents
        {
            get
            {
                return sensedAgents;
            }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private String name;
        public String Name
        {
            get
            {
                return name;
            }
        }
        public Agent(String agentName)
        {
            id = agents++;
            name = agentName;
            spriteRect = new Rectangle(0, 0, Game1.cellWidth, Game1.cellHeight);
            sensedAgents = new List<Agent>();
        }

        protected String textureName = "";

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (textureName != "")
            {
                spriteRect.X = location.X * Game1.cellWidth;
                spriteRect.Y = location.Y * Game1.cellHeight;
                spriteBatch.Draw(_texture, spriteRect, Color.White);
            }
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            if (textureName != "")
                _texture = contentManager.Load<Texture2D>(textureName);
        }

        protected Location location;
        public Location Location
        {
            get
            {
                return location;
            }
            set
            {
                Location lastLocation = location;
                location = value;
                UpdateLocation(lastLocation);
            }
        }

        void UpdateLocation(Location lastLocation)
        {
            AgentManager.UpdateLocation(this, location, lastLocation);
        }

        // Any agent must implement these methods
        abstract public void Update(GameTime gameTime);
        abstract public bool HandleMessage(Telegram telegram);

        public void SetStateMachine(Object obj)
        {
            _stateMachine = obj;
        }

        public Object _StateMachine { get { return _stateMachine; } }
        private Object _stateMachine;

        public virtual void OnSense(Agent other)
        {
            
        }

        public virtual void OnUnsense(Agent other)
        {

        }

    }
}
