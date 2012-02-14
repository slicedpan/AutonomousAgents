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
        static Random rand;       
        static Location()
        {
            rand = new Random();
            shack = new Shack(0, 0, "shack");
            Game1.grid.AddLocation(shack);
            cemetery = new Cemetery(0, 0, "cemetery");
            Game1.grid.AddLocation(cemetery);
            bank = new Bank(0, 0, "bank");
            Game1.grid.AddLocation(bank);
            saloon = new Saloon(0, 0, "saloon");
            Game1.grid.AddLocation(saloon);
            outlawCamp = new OutlawCamp(0, 0, "outlaw camp");
            Game1.grid.AddLocation(outlawCamp);
            sheriffsOffice = new SheriffsOffice(0, 0, "sheriff's office");
            Game1.grid.AddLocation(sheriffsOffice);
            undertakers = new UnderTakers(0, 0, "undertakers");
            Game1.grid.AddLocation(undertakers);
            goldMine = new GoldMine(0, 0, "gold mine");
            Game1.grid.AddLocation(goldMine);
        }

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

        public Location(int x, int y, String description)
        {
            _X = x;
            _Y = y;
            _description = description;
        }

        public virtual Texture2D Texture
        {
            get
            {
                return null;
            }
        }

        public virtual void LoadContent(ContentManager manager)
        {

        }

        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(Texture, new Vector2(_X * Game1.cellWidth, _Y * Game1.cellHeight), Color.White);
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
