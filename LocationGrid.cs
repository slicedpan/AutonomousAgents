using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace FiniteStateMachine
{
    public class RandomLocation
    {
        public String name = "";
        public String texture = "";
        public int travelCost = 0;
        public double frequency = 0.0d;
    }
    public class LocationGrid
    {
        private Dictionary<String, Location> namedLocations = new Dictionary<string,Location>();

        public Dictionary<String, Location> NamedLocations
        {
            get
            {
                return namedLocations;
            }
        }

        private Dictionary<String, RandomLocation> randomLocations = new Dictionary<string,RandomLocation>();

        public Location AddNamedLocation(String name, String texture)
        {
            Location location = new Location(name, texture);
            namedLocations.Add(name, location);
            AddLocation(location);
            return location;
        }

        public void AddRandomLocation(String name, String texture, int travelCost, double frequency)
        {
            RandomLocation r = new RandomLocation();
            r.name = name;
            r.texture = texture;
            r.travelCost = travelCost;
            r.frequency = frequency;
            randomLocations.Add(name, r);
        }

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

        bool randomiserInitialised = false;

        void InitialiseRandomiser()
        {
            double totalFreq = 0.0d;
            foreach (KeyValuePair<String, RandomLocation> r in randomLocations)
            {
                totalFreq += r.Value.frequency;
            }
            double runningTotal = 0.0d;
            foreach (KeyValuePair<String, RandomLocation> r in randomLocations)
            {
                r.Value.frequency /= totalFreq;
                double oldFreq = r.Value.frequency;
                r.Value.frequency += runningTotal;
                runningTotal += oldFreq;
            }
            randomiserInitialised = true;
        }

        Location NextRandomLocation()
        {
            Location retLoc = new Location("blah", "blah");
            if (!randomiserInitialised)
            {
                InitialiseRandomiser();
            }
            double randomDouble = rand.NextDouble();
            RandomLocation chosenLocation = new RandomLocation();
            foreach (KeyValuePair<String, RandomLocation> r in randomLocations)
            {
                if (randomDouble < r.Value.frequency)
                {
                    chosenLocation = r.Value;
                    break;
                }
            }
            retLoc = new Location(chosenLocation.name, chosenLocation.texture);
            retLoc.TravelCost = chosenLocation.travelCost;
            return retLoc;
        }

        public void Populate()
        {
            for (int i = 0; i < _width; ++i)
            {
                for (int j = 0; j < _height; ++j)
                {
                    if (locations[i][j] == null)
                    {
                        locations[i][j] = NextRandomLocation();
                        locations[i][j].X = i;
                        locations[i][j].Y = j;
                    }
                }
            }
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
