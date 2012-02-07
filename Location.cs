using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        static int maxX, maxY;        
        static void Init(int x, int y)
        {
            maxX = x;
            maxY = y;
            rand = new Random();

        }

        private int _X, _Y;
        public int X
        {
            get
            {
                return _X;
            }
        }
        public int Y
        {
            get
            {
                return _Y;
            }
        }

        public Location(int x, int y)
        {
            _X = x;
            _Y = y;
        }
    }
}
