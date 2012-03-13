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

        // Every agent has a numerical id that is set when it is created
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public Agent()
        {
            id = agents++;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public virtual void LoadContent(ContentManager contentManager)
        {

        }

        // Any agent must implement these methods
        abstract public void Update();
        abstract public bool HandleMessage(Telegram telegram);
    }
}
