using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Text;
using System.Xml;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace MainGame
{
    class Menu
    {
        protected SpriteBatch spriteBatch;
        protected ContentManager Content;
        protected List<Texture2D> obj;
        protected List<Rectangle> pos;
        List<int> levelSta;
        private MouseState lastMouse = new MouseState();
        int curPos, lasPos, gameSta, level;
        public Menu(SpriteBatch sprite, ContentManager content)
        {
            spriteBatch = sprite;
            Content = content;
            obj = new List<Texture2D>();
            pos = new List<Rectangle>();
            curPos = lasPos = -1;
            gameSta = 0;
        }
        public void loadcontent(List<int> sta, int n)
        {
            levelSta = sta;
            level = n;
            Rectangle temp = new Rectangle(100, 100, 70, 70); ;
            int x, y;
            x = 1;y = 0;
            for (int i = 0; i < level; i++) 
            {
                obj.Add(Content.Load<Texture2D>($"Menu/level{3}"));
                if(x!=0)
                {
                    temp.X = temp.X + 150;
                    x = (x + 1) % 5;
                }
                else
                {
                    temp.X = 250;
                    temp.Y = temp.Y + 150 ;
                    x = 2;
                }
                pos.Add(temp);
            }
            //
            temp.X = 300;
            temp.Y = 400;
            temp.Width = 100;
            temp.Height = 100;
            obj.Add(Content.Load<Texture2D>("Menu/restart"));
            pos.Add(temp);
            obj.Add(Content.Load<Texture2D>("Menu/back"));
            temp.X = 600;
            pos.Add(temp);
            //
            temp.X = 400;
            temp.Y = 100;
            temp.Width = 200;
            temp.Height = 200;
            obj.Add(Content.Load<Texture2D>("Menu/win"));
            obj.Add(Content.Load<Texture2D>("Menu/lose"));
            pos.Add(temp);
            pos.Add(temp);
        }
        public int update()
        {
            MouseState curMouse = Mouse.GetState();
            Rectangle mousePos = new Rectangle(curMouse.X, curMouse.Y, 5, 5);
            if (gameSta == 0) 
            {
                if (curMouse.LeftButton == ButtonState.Pressed && lastMouse.LeftButton == ButtonState.Released)
                {
                    for (int i = 0; i < level; i++)
                    {
                        if (pos[i].Intersects(mousePos) && levelSta[i] == 1) 
                        {
                            curPos = i;
                            return i;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < level; i++)
                    {
                        if (pos[i].Intersects(mousePos))
                        {
                            curPos = i;
                            return -1;
                        }
                        curPos = -1;
                    }
                }
            }
            else
            {
                if (curMouse.LeftButton == ButtonState.Pressed && lastMouse.LeftButton == ButtonState.Released)
                {
                    if (pos[level].Intersects(mousePos)) 
                    {
                        return curPos;
                    }
                    if (pos[level + 1].Intersects(mousePos)) 
                    {
                        gameSta = 0;
                    }
                }
            }
            lastMouse = curMouse;
            return -1;
        }
        public void draw()
        {
            spriteBatch.Begin(sortMode: SpriteSortMode.BackToFront);
            if (gameSta == 0)
            {
                for (int i = 0; i < level; i++)
                {
                    if (i == curPos)
                    {
                        spriteBatch.Draw(obj[i], pos[i], Color.Gray);
                    }
                    else
                    {
                        spriteBatch.Draw(obj[i], pos[i], Color.White);
                    }

                }
            }
            if (gameSta == 1) 
            {
                spriteBatch.Draw(obj[level], pos[level], Color.White);
                spriteBatch.Draw(obj[level + 1], pos[level + 1], Color.White);
                spriteBatch.Draw(obj[level + 2], pos[level + 2], Color.White * 0.5f);
                // win game
            }
            if (gameSta == -1) 
            {
                // lose game
                spriteBatch.Draw(obj[level], pos[level], Color.White);
                spriteBatch.Draw(obj[level + 1], pos[level + 1], Color.White);
                spriteBatch.Draw(obj[level + 3], pos[level + 3], Color.White * 0.5f);
            }
            spriteBatch.End();
        }
        public void setGameSta(int n)
        {
            gameSta = n;
        }
        public void unlock(XMLData.MapContent data)
        {
            levelSta[curPos + 1] = 1;
            // luu file data
            data.sta[curPos + 1] = 1;
            XmlWriterSettings setting = new XmlWriterSettings();
            setting.Indent = true;
            using (XmlWriter writer = XmlWriter.Create("test.xml", setting))
            {
                Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate.IntermediateSerializer.Serialize(writer, data, null);
            }
        }
    }
}
