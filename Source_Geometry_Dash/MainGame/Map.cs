using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MainGame
{
    class Map
    {
        protected static readonly Random rnd = new Random();
        protected SpriteBatch spriteBatch;
        protected ContentManager Content;
        protected List<Texture2D> obj;
        protected List<Rectangle> pos;
        protected List<int> objType;
        protected int n, speed, curSpeed;
        protected static int mapTyp;
        public Map(SpriteBatch sprite, ContentManager content)
        {
            spriteBatch = sprite;
            Content = content;
            obj = new List<Texture2D>();
            pos = new List<Rectangle>();
            objType = new List<int>();
            n = 0;
            speed = 0;
            mapTyp = 0;
        }
        public void loadcontent()
        {
        }
        public int update()
        {
            return 0;
        }
        public void draw()
        {
        }
        public void setSpeed(int n)
        {
            speed = n;
            curSpeed = speed;
        }
        public void setSpeed3(int n)
        {
            if (n == 0)
            {
                curSpeed = speed;
            }
            else
            {
                curSpeed = curSpeed + 2;
            }
        }
        public int getSpeed()
        {
            return speed;
        }
        
    }
    class Paths : Map
    {
        int mark = 0;
        public Paths(SpriteBatch sprite, ContentManager content) : base(sprite, content)
        {
        }
        public void loadcontent(int typ, Rectangle position)
        {
            obj.Add(Content.Load<Texture2D>($"Map/map{typ}"));
            pos.Add(position);
            objType.Add(typ);
            n++;
        }
        public int update(Rectangle posPlayer, int sta, int objForm)
        {
            if (sta == -2) 
            {
                return 2;
            }
            int lastSta, curSta;
            lastSta = curSta = 0;
            for (int i = 0; i < n; i++)
            {
                Rectangle temp = pos[i];
                temp.X = temp.X - curSpeed;
                pos[i] = temp;
            }
            Rectangle scale = posPlayer;
            scale.Width = scale.Width + 1;
            scale.Height = scale.Height + 2;
            scale.Y = scale.Y - 1;
            for (int i = 0; i < n; i++)
            {
                switch (objForm)
                {
                    case 0:
                        if (pos[i].Intersects(scale))
                        {
                            // contact rectangle
                            if(i==25)
                            {

                            }
                            if (posPlayer.Y + 50 == pos[i].Y || sta == -1)
                            {
                                // phia ben tren
                                curSta = 1;
                                break;
                            }
                            if (pos[i].Y + pos[i].Height == posPlayer.Y)
                            {
                                // cham phia duoi
                                curSta = -1;
                                break;
                            }
                            return 2;
                        }
                        break;
                    case 1:
                        if (pos[i].Intersects(scale))
                        {
                            // contact rectangle
                            if (objType[i] != 1)
                            {
                                break;
                            }
                            if (posPlayer.Y + 50 == pos[i].Y || sta == -1)
                            {
                                // phia ben tren
                                curSta = 1;
                                break;
                            }
                            if (pos[i].Y + pos[i].Height == posPlayer.Y)
                            {
                                // cham phia duoi
                                curSta = -1;
                                break;
                            }
                            Rectangle temp = pos[i];
                            return 2;
                        }
                        
                        break;
                    case 2:
                        if (pos[i].Intersects(scale))
                        {
                            // contact triangle
                            int x, y;
                            switch (objType[i])
                            {
                                case 1:
                                    return 2;
                                    break;
                                case 2:
                                    // up up
                                    break;
                                case 3:
                                    // down up
                                    Rectangle temp = pos[i];
                                    x = posPlayer.X + posPlayer.Width - pos[i].X;
                                    y = posPlayer.Y + posPlayer.Height - pos[i].Y;
                                    if (x + y >= pos[i].Width)
                                    {
                                        return 2;
                                    }
                                    break;
                                case 4:
                                    x = posPlayer.X + posPlayer.Width - pos[i].X;
                                    y = pos[i].Y + pos[i].Height - posPlayer.Y;
                                    if (x + y >= pos[i].Width)
                                    {
                                        return 2;
                                    }
                                    // up down
                                    break;
                                case 5:
                                    // down down
                                    break;
                            }
                        }
                        break;
                }
            }
            if (sta == 0 && curSta != 1) 
            {
                return -1;
            }
            return curSta;
        }
        //int isIntersectecs(Rectangle pos, Rectangle player)
        //{
        //    Rectangle scale = player;
        //    scale.Width = scale.Width + 1;
        //    scale.Height = scale.Height + 1;
        //    if (pos.Intersects(scale))
        //    {
        //        if (player.Y + 50 == pos.Y)
        //        {
        //            return 1;
        //        }
        //    }
        //    return 0;
        //}
        public void draw()
        {
            spriteBatch.Begin(sortMode: SpriteSortMode.Immediate);
            for (int i = 0; i < n; i++)
            {
                spriteBatch.Draw(obj[i], pos[i], Color.White);
            }
            spriteBatch.End();
        }
        public Paths copyMap(Paths sour)
        {
            Paths des;
            des = new Paths(spriteBatch, Content);
            des.obj.AddRange(sour.obj);
            des.pos.AddRange(sour.pos);
            des.objType.AddRange(sour.objType);
            des.n = sour.n;
            des.speed = sour.speed;
            des.curSpeed = sour.curSpeed;
            return des;
        }
    }
    
    class Deter : Map
    {
        int tranform = 0;
        public Deter(SpriteBatch sprite, ContentManager content) : base(sprite, content)
        {
        }
        public void loadcontent(int typ, Rectangle position)
        {
            switch (typ)
            {
                case 6:
                    obj.Add(Content.Load<Texture2D>("Map/spike"));                 
                    break;
                case 7:
                    obj.Add(Content.Load<Texture2D>("Map/bullet"));
                    break;
                case 8:
                    obj.Add(Content.Load<Texture2D>("Map/meteor"));
                    break;
                case 9:
                    obj.Add(Content.Load<Texture2D>("Map/portal"));
                    break;
                case 10:
                    obj.Add(Content.Load<Texture2D>("Map/endpoint"));
                    break;
            }
            pos.Add(position);
            objType.Add(typ);
            n++;
        }
        public int update(Rectangle posPlayer)
        {
            for (int i = 0; i < n; i++)
            {
                Rectangle temp = pos[i];
                temp.X = temp.X - curSpeed;
                if (objType[i] == 7 && temp.X < 1000)
                {
                    temp.X = temp.X - 5;
                }
                if (objType[i] == 8 && temp.X < 350 && temp.Y < 900)
                {
                    temp.Y = temp.Y + 10;
                }
                pos[i] = temp;
            }
            for (int i = 0; i < n; i++) 
            {
                if (pos[i].Intersects(posPlayer))
                {
                    switch (objType[i])
                    {
                        case 6:
                            return -1;
                        case 7:
                            return -1;
                        case 8:
                            return -1;
                        case 9:
                            // chuyen form
                            tranform = 1;
                            break;
                    }
                }
                if (objType[i] == 9)
                {
                    if (pos[i].X + pos[i].Width < posPlayer.X && tranform == 1)
                    {
                        tranform = 0;
                        obj.RemoveAt(i);
                        pos.RemoveAt(i);
                        objType.RemoveAt(i);
                        n--;
                        return -2;
                    }
                }
                if (objType[i] == 10 && pos[i].X <= posPlayer.X + 20)  
                {
                    //win game 
                    return 0;
                }
            }
            return 1;
        }
        public void draw()
        {
            spriteBatch.Begin(sortMode: SpriteSortMode.Immediate);
            for (int i = 0; i < n; i++)
            {
                spriteBatch.Draw(obj[i], pos[i], Color.White);
            }
            spriteBatch.End();
        }
        public Deter copyMap(Deter sour)
        {
            Deter des;
            des = new Deter(spriteBatch, Content);
            des.obj.AddRange(sour.obj);
            des.pos.AddRange(sour.pos);
            des.objType.AddRange(sour.objType);
            des.n = sour.n;
            des.speed = sour.speed;
            des.curSpeed = sour.curSpeed;
            return des;
        }
    }
    class backGround : Map
    {
        int n;
        public backGround(SpriteBatch sprite, ContentManager content) : base(sprite, content)
        {
            n = 1;
        }
        public void loadcontent()
        {
            Rectangle temp = new Rectangle(0, 0, 1400, 700);
            obj.Add(Content.Load<Texture2D>("BackGround/background"));
            pos.Add(temp);
            temp.Width = 700;
            for (int i = 0; i < 6; i++) 
            {
                int j = i % 3;
                temp.X = 0 + 700 * j;
                
                obj.Add(Content.Load<Texture2D>($"BackGround/background{i}"));
                pos.Add(temp);
            }
        }
        public void update(int view)
        {
            Rectangle temp;
            if (view==0)
            {
                for (int i = 1; i < 4; i++)
                {
                    // view hang dong
                    temp = pos[i];
                    temp.X = temp.X - 5;
                    pos[i] = temp;
                }
                for (int i = 1; i < 4; i++)
                {
                    temp = pos[i];
                    if (pos[i].X < -700)
                    {
                        temp.X = 1395;
                    }
                    pos[i] = temp;
                }
            }
            if(view==1)
            {
                // view rung cay
                for (int i = 4; i < 7; i++)
                {
                    temp = pos[i];
                    temp.X = temp.X - 5;
                    pos[i] = temp;
                }
                for (int i = 4; i < 7; i++)
                {
                    temp = pos[i];
                    if (pos[i].X < -700)
                    {
                        temp.X = 1395;
                    }
                    pos[i] = temp;
                }
            }
            temp = pos[0];
            if (temp.X == -400 || temp.X == 0) 
            {
                n = n * -1;
            }
            temp.X = temp.X + n;
            pos[0] = temp;
        }
        public void draw(int view)
        {
            spriteBatch.Begin(sortMode: SpriteSortMode.Immediate);
            if (view == 0) 
            {
                for (int i = 1; i < 4; i++)
                {
                    spriteBatch.Draw(obj[i], pos[i], Color.White * 0.5f);
                }
            }
            if (view == 1)
            {
                for (int i = 4; i < 7; i++)
                {
                    spriteBatch.Draw(obj[i], pos[i], Color.White * 0.5f);
                }
            }
            if (view == -1)
            {
                spriteBatch.Draw(obj[0], pos[0], Color.White);
            }
            spriteBatch.End();
        }
    }
}
