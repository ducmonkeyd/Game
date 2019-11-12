using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MainGame
{
    class Unit
    {
        protected static readonly Random rnd = new Random();
        protected SpriteBatch spriteBatch;
        protected ContentManager Content;
        static int force = 160;
        List<Texture2D> player;
        List<Texture2D> player1;
        List<Texture2D> player2;
        Rectangle pos;
        int moveState, move, hight, form, animation, speed;
        public Unit(SpriteBatch sprite, ContentManager content)
        {
            Content = content;
            spriteBatch = sprite;
            pos = new Rectangle(100, 500, 50, 50);
            player = new List<Texture2D>();
            player1 = new List<Texture2D>();
            player2 = new List<Texture2D>();
            move = 0; // trang thai trong phuong thuc di chuyen (len-xuong)
            moveState = 0;// kieu di chuyen cua unit
            hight = 600;// chieu cao cua nen (base ground)
            form = 0;
            animation = 0;
            speed = 10;
        }
        public void loadcontent()
        {
            for (int i = 0; i < 4; i++) 
            {
                player.Add(Content.Load<Texture2D>($"Units/1unit{i}"));
            }
            for (int i = 0; i < 2; i++) 
            {
                player1.Add(Content.Load<Texture2D>($"Units/2unit{i}"));
            }
            for (int i = 0; i < 2; i++) 
            {
                player2.Add(Content.Load<Texture2D>($"Units/3unit{i}"));
            }
        }
        // cac phuong thuc di chuyen

        public int update(int state)
        {
            setAnimation();
            switch (state)
            {
                case 1:
                    moveState = state;
                    if (move == 0)
                    {
                        move = 1;
                    }
                    break;
                case 2:
                    moveState = state;
                    move = 1;
                    break;
                case 3:
                    moveState = state;
                    if (move == 1)
                    {
                        move = -1;
                    }
                    else
                    {
                        move = 1;
                    }
                    break;
            }
            switch (moveState)
            {
                case 1:
                    Jump();
                    break;
                case 2:
                    Jump();
                    break;
                case 3:
                    HeartBeat();
                    break;
                case 0:
                    break;
            }
            if(pos.Y>600)
            {
                return -2;
            }
            return move;
        }
        public void draw()
        {
            spriteBatch.Begin(sortMode: SpriteSortMode.Immediate);
            switch(form)
            {
                case 0:
                    spriteBatch.Draw(player[animation], pos, Color.White);
                    break;
                case 1:
                    spriteBatch.Draw(player1[animation], pos, Color.White);
                    break;
                case 2:     
                    spriteBatch.Draw(player2[animation], pos, Color.White);
                    break;
            }         
            spriteBatch.End();
        }
        public void Jump()
        {
            if (moveState == 1 || moveState == 2) 
            {
                if (move == 1)
                {
                    pos.Y = pos.Y - speed;
                    force = force - speed;
                    if (force <= 0)
                    {
                        move = -1;
                        return;
                    }       
                }
                if (move == -1)
                {
                    if (form == 1)
                    {
                        force = 70;
                    }
                    else
                    {
                        force = 160;
                    }
                    pos.Y = pos.Y + speed;
                    if (pos.Y == hight)
                    {
                        moveState = 0;
                        move = -2;
                        return;
                    }
                }
            }
        }
        public void HeartBeat()
        {
            if (move == -1)
            {
                pos.Y += speed;
            }
            else
            {
                pos.Y -= speed;
            }
        }
        public Rectangle possition()
        {
            return pos;
        }
        public void setFall(int fall)
        {
            if (fall == 1)
            {
                // cham phia tren object
                move = 0;
            }
            if (fall == 0)
            {
                // khong cham object
                if (moveState != 0)
                {
                    move = -1;
                }
            }
        }
        public int setTranform(int sp)
        {
            form = (form + 1) % 3;
            if (form == 2) 
            {
                animation = 0;
                speed = sp;
                move = 0;
            }
            if (form == 1) 
            {
                animation = 0;
                speed = 5;
                if (force != 160)
                {
                    if (force < 100)
                    {
                        // da nhay qua portal quang kha dai roi
                        force = 0;
                    }
                    else
                    {
                        force = 70 - 160 - force;
                    }
                }
                else
                {
                    force = 70;
                }
            }
            if (form == 0)
            {
                animation = 0;
                moveState = 1;
                move = -1;
                force = 160;
                speed = 10;
                // reset posplayer
                if (pos.Y % 10 != 0) 
                {
                    for(int i=0;i<10;i++)
                    {
                        pos.Y = pos.Y + 1;
                        if (pos.Y % 10 == 0)
                        {
                            i = 10;
                        }
                    }
                }
            }
            return form;
        }
        public void setAnimation()
        {
            switch (form)
            {
                case 0:
                    if (force == 10)
                    {
                        animation = (animation + 1) % 4;
                    }
                    break;
                case 1:
                    if (move == 1) 
                    {
                        animation = 0;
                    }
                    if (move == -1) 
                    {
                        animation = 1;
                    }
                    break;
                case 2:
                    if (move == 1)
                    {
                        animation = 0;
                    }
                    if (move == -1)
                    {
                        animation = 1;
                    }
                    break;
            }
        }
        public void reset()
        {
            form = 0;
            pos.Y = 500;
            animation = 0;
            move = 0;
            moveState = 0;
            force = 150;
            speed = 10;
        }
        void setSpedd(int n)
        {
            speed = n;
        }
        // cac ham de kiem tra khong co gia tri
        public int getForm()
        {
            return form;
        }
        public int getanimation()
        {
            return animation;
        }
        public int getspeed()
        {
            return speed;
        }
    }
}
