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

        protected String textureName = "";

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (textureName != "")
                spriteBatch.Draw(_texture, new Vector2(location.X * Game1.cellWidth, location.Y * Game1.cellHeight), Color.White);
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
                location = value;
            }
        }

        // Any agent must implement these methods
        abstract public void Update();
        abstract public bool HandleMessage(Telegram telegram);
    }
}
