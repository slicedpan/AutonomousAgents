using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace FiniteStateMachine
{
    public class PathEntry
    {
        public List<Location> path;
        public Color pathColour;
        public List<Rectangle> spriteRects;
        public List<Texture2D> sprites;
        public List<float> angles;
    }
    public class PathDrawer
    {        
        List<PathEntry> paths;

        Texture2D horizontal, vertical, cwcorner, ccwcorner;

        Rectangle GetRectFromLocation(Location location)
        {
            return new Rectangle(location.X * Game1.cellWidth, location.Y * Game1.cellHeight, Game1.cellWidth, Game1.cellHeight);
        }

        Random rand = new Random();

        public PathDrawer()
        {
            paths = new List<PathEntry>();
        }
        public void AddPath(List<Location> path)
        {
            PathEntry p = new PathEntry()
            {
                path = path,
                pathColour = new Color(rand.Next(255), rand.Next(255), rand.Next(255))
            };
            CreatePaths(p);
            paths.Add(p);
        }
        public void RemovePath(List<Location> path)
        {
            for (int i = 0; i < paths.Count; ++i)
            {
                if (paths[i].path == path)
                {
                    paths.Remove(paths[i]);
                    return;
                }
            }
        }
        public void LoadContent(ContentManager cm)
        {
            horizontal = cm.Load<Texture2D>("horizontal");
            vertical = cm.Load<Texture2D>("vertical");
            cwcorner = cm.Load<Texture2D>("cwcorner");
            ccwcorner = cm.Load<Texture2D>("ccwcorner");
        }
        public void DrawPaths(SpriteBatch sb)
        {
            sb.Begin();
            foreach (PathEntry p in paths)
            {
                for (int i = 0; i < p.angles.Count; ++i)
                {
                    sb.Draw(p.sprites[i], p.spriteRects[i], null, p.pathColour, p.angles[i], Vector2.Zero, SpriteEffects.None, 0.0f);
                }
            }
            sb.End();
        }

        Rectangle ShiftRect(Rectangle baseRect, int index)
        {
            Rectangle retRect = baseRect;
            switch (index)
            {
                case 1:
                    retRect.X += Game1.cellWidth;
                    break;
                case 2:
                    retRect.X += Game1.cellWidth;
                    retRect.Y += Game1.cellHeight;
                    break;
                case 3:
                    retRect.Y += Game1.cellHeight;
                    break;
                default:
                    break;                    
            }
            return retRect;
        }

        public void CreatePaths(PathEntry p)
        { 
            Location next, current, last;
            p.sprites = new List<Texture2D>();
            p.spriteRects = new List<Rectangle>();
            p.angles = new List<float>();

            float ninetyDegrees = 0.5f * (float)Math.PI;

            for (int i = 1; i < p.path.Count - 1; ++i)
            {
                last = p.path[i - 1];
                next = p.path[i + 1];
                current = p.path[i];
                p.spriteRects.Add(GetRectFromLocation(current));
                if (current.X > last.X)
                {
                    if (current.Y < next.Y)
                    {
                        p.sprites.Add(cwcorner);
                        p.angles.Add(0.0f);
                    }
                    else if (current.Y > next.Y)
                    {
                        p.sprites.Add(cwcorner);
                        p.angles.Add(ninetyDegrees);
                        p.spriteRects[i - 1] = ShiftRect(p.spriteRects[i - 1], 1);
                    }
                    else
                    {
                        p.sprites.Add(horizontal);
                        p.angles.Add(0.0f);
                    }
                }
                else if (current.X < last.X)
                {
                    if (current.Y < next.Y)
                    {
                        p.sprites.Add(cwcorner);
                        p.angles.Add(3.0f * ninetyDegrees);
                        p.spriteRects[i - 1] = ShiftRect(p.spriteRects[i - 1], 3);
                    }
                    else if (current.Y > next.Y)
                    {
                        p.sprites.Add(cwcorner);
                        p.angles.Add(2.0f * ninetyDegrees);
                        p.spriteRects[i - 1] = ShiftRect(p.spriteRects[i - 1], 2);
                    }
                    else
                    {
                        p.sprites.Add(horizontal);
                        p.angles.Add(0.0f);
                    }
                }
                else if (current.Y > last.Y)
                {
                    if (current.X > next.X)
                    {
                        p.sprites.Add(cwcorner);
                        p.angles.Add(ninetyDegrees);
                        p.spriteRects[i - 1] = ShiftRect(p.spriteRects[i - 1], 1);
                    }
                    else if (current.X < next.X)
                    {
                        p.sprites.Add(cwcorner);
                        p.angles.Add(2.0f * ninetyDegrees);
                        p.spriteRects[i - 1] = ShiftRect(p.spriteRects[i - 1], 2);
                    }
                    else
                    {
                        p.sprites.Add(vertical);
                        p.angles.Add(0.0f);
                    }
                }
                else
                {
                    if (current.X > next.X)
                    {
                        p.sprites.Add(cwcorner);
                        p.angles.Add(0.0f);
                    }
                    else if (current.X < next.X)
                    {
                        p.sprites.Add(cwcorner);
                        p.angles.Add(3.0f * ninetyDegrees);
                        p.spriteRects[i - 1] = ShiftRect(p.spriteRects[i - 1], 3);
                    }
                    else
                    {
                        p.sprites.Add(vertical);
                        p.angles.Add(0.0f);
                    }
                }
            }
            
        }
    }
}
