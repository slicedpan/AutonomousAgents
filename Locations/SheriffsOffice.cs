﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace FiniteStateMachine
{
    class SheriffsOffice : Location
    {
        static Texture2D texture;

        public SheriffsOffice(int x, int y, String description) : base(x, y, description)
        {

        }

        public override void LoadContent(ContentManager manager)
        {
            if (texture == null)
                texture = manager.Load<Texture2D>("sheriffsoffice");
        }
        public override Texture2D Texture
        {
            get
            {
                return texture;
            }
        }
    }
}