using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace FiniteStateMachine
{

    public class LocationGrid
    {        
        public LocationGrid(int width, int height)
        {
            locations = new List<List<Location>>();
            for (int i = 0; i < width; ++i)
            {
                locations.Add(new List<Location>());
                for (int j = 0; j < height; ++j)
                {
                    locations[i].Add(null);
                }
            }
            _width = width;
            _height = height;
        }
        int _width, _height;
        List<List<Location>> locations;

        Random rand = new Random();

        public void AddLocation(Location locationToAdd)
        {
            int x = rand.Next(Game1.numCellsX);
            int y = rand.Next(Game1.numCellsY);
            while (locations[x][y] != null)
            {
                x = rand.Next(Game1.numCellsX);
                y = rand.Next(Game1.numCellsY);
            }
            locations[x][y] = locationToAdd;
            locationToAdd.X = x;
            locationToAdd.Y = y;
        }

        public void LoadContent(ContentManager manager)
        {
            for (int i = 0; i < locations.Count; ++i)
            {
                for (int j = 0; j < locations[i].Count; ++j)
                {
                    if (locations[i][j] != null)
                        locations[i][j].LoadContent(manager);
                }
            }
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Begin();
            for (int i = 0; i < locations.Count; ++i)
            {
                for (int j = 0; j < locations[i].Count; ++j)
                {
                    batch.Draw(Game1.bg, new Vector2(i * Game1.cellWidth, j * Game1.cellHeight), Color.White);
                    if (locations[i][j] != null)
                        locations[i][j].Draw(batch);
                }
            }
            batch.End();
        }
    }
}
