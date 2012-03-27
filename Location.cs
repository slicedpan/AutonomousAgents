using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace FiniteStateMachine
{
    // Here is the list of locations that agents can visit
    public class Location
    {
        public static Location shack;
        public static Location cemetery;
        public static Location bank;
        public static Location saloon;
        public static Location outlawCamp;
        public static Location sheriffsOffice;
        public static Location undertakers;
        public static Location goldMine;      
        static Location()
        {
            shack = Game1.grid.AddNamedLocation("shack", "shack");
            cemetery = Game1.grid.AddNamedLocation("cemetery", "cemetery");
            bank = Game1.grid.AddNamedLocation("bank", "bank");
            saloon = Game1.grid.AddNamedLocation("saloon", "saloon");            
            outlawCamp = Game1.grid.AddNamedLocation("outlawCamp", "outlawcamp");
            sheriffsOffice = Game1.grid.AddNamedLocation("sheriff's office", "sheriffsoffice");            
            undertakers = Game1.grid.AddNamedLocation("undertakers", "undertakers");            
            goldMine = Game1.grid.AddNamedLocation("gold mine", "goldmine");
        }

        public int TravelCost = 0;

        private int _X, _Y;
        public int X
        {
            get
            {
                return _X;
            }
            set
            {
                _X = value;
            }
        }
        public int Y
        {
            get
            {
                return _Y;
            }
            set
            {
                _Y = value;
            }
        }
        private String _description;
        public String Description
        {
            get
            {
                return _description;
            }
        }
        private String _textureName;


        public Location(String description, String textureName)
        {
            _description = description;
            _textureName = textureName;
        }
        Texture2D _texture;
        public virtual Texture2D Texture
        {
            get
            {
                return _texture;
            }
        }

        public virtual void LoadContent(ContentManager manager)
        {
            if (_textureName == "")
                return;
            _texture = manager.Load<Texture2D>(_textureName);
        }

        public virtual void Draw(SpriteBatch batch)
        {
            if (_textureName == "")
                return;
            batch.Draw(Texture, new Vector2(_X * Game1.cellWidth, _Y * Game1.cellHeight), Color.White);
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
