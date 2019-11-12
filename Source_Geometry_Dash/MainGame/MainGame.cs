using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using XMLData;

namespace MainGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MapData mapPos;
        MapContent levelmenu;
        SpriteFont font;
        Unit unit;
        List<Paths> pathBase;
        List<Deter> deterBase;
        backGround background;
        Paths curPath;
        Deter curDeter;
        Menu menu;
        Song song;
        SoundEffect clicks;


        int move, hold, point, sta;
        private MouseState lastMouse = new MouseState();
        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 700;
        }
        protected override void Initialize()
        {
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            menu = new Menu(spriteBatch, Content);
            unit = new Unit(spriteBatch, Content);
            background = new backGround(spriteBatch, Content);
            pathBase = new List<Paths>();
            deterBase = new List<Deter>();
            curPath = new Paths(spriteBatch, Content);
            curDeter = new Deter(spriteBatch, Content);
            for (int i = 0; i < 12; i++)
            {
                Paths _path = new Paths(spriteBatch, Content);
                Deter _barrier = new Deter(spriteBatch, Content);
                pathBase.Add(_path);
                deterBase.Add(_barrier);
            }

            move = 0;
            point = 0;
            hold = -1;
            base.Initialize();
        }
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            // TODO: use this.Content to load your game content here
            List<int> temp;
            levelmenu = Content.Load<MapContent>("Levels/data");
            song = Content.Load<Song>("Music-SoundEffect/BaseAfterBase");
            clicks = Content.Load<SoundEffect>("Music-SoundEffect/Click");
            for (int i = 0; i < levelmenu.amount; i++) 
            {
                mapPos = Content.Load<MapData>($"Levels/level{i + 1}");
                pathBase[i].setSpeed(mapPos.speed);
                deterBase[i].setSpeed(mapPos.speed);
                temp = mapPos.typeObj;
                
                for (int j = 0; j < mapPos.amount; j++)
                {
                    if (i == 6 && temp[j] == 10) 
                    {

                    }
                    switch (temp[j])
                    {
                        case 1:
                            pathBase[i].loadcontent(1, mapPos.pos[j]);
                            break;
                        case 2:
                            pathBase[i].loadcontent(2, mapPos.pos[j]);
                            break;
                        case 3:
                            pathBase[i].loadcontent(3, mapPos.pos[j]);
                            break;
                        case 4:
                            pathBase[i].loadcontent(4, mapPos.pos[j]);
                            break;
                        case 5:
                            pathBase[i].loadcontent(5, mapPos.pos[j]);
                            break;
                        case 6:
                            deterBase[i].loadcontent(6, mapPos.pos[j]);
                            break;
                        case 7:
                            deterBase[i].loadcontent(7, mapPos.pos[j]);
                            break;
                        case 8:
                            deterBase[i].loadcontent(8, mapPos.pos[j]);
                            break;
                        case 9:
                            deterBase[i].loadcontent(9, mapPos.pos[j]);
                            break;
                        case 10:
                            deterBase[i].loadcontent(10, mapPos.pos[j]);
                            break;
                    }
                }
            }          
            unit.loadcontent();
            menu.loadcontent(levelmenu.sta, levelmenu.amount);
            background.loadcontent();
            font = Content.Load<SpriteFont>("fontText");
            MediaPlayer.Play(song);
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
        }
                                             
        protected override void UnloadContent()
        {
            // test luu file xml
            //GameFileSerialization.Save(path);
        }
        void MediaPlayer_MediaStateChanged(object sender, System.
                                           EventArgs e)
        {
            // 0.0f is silent, 1.0f is full volume
            MediaPlayer.Volume -= 0.1f;
            MediaPlayer.Play(song);
        }
        protected override void Update(GameTime gameTime)
        {
            MouseState curMouse = Mouse.GetState();
            // start game

            if (hold != -1)
            {
                MouseState currentMouse = Mouse.GetState();
                if (curMouse.LeftButton == ButtonState.Pressed && lastMouse.LeftButton == ButtonState.Released)
                {
                    clicks.Play(); 
                    switch (unit.getForm())
                    {
                        case 0:
                            move = 1;
                            break;
                        case 1:
                            move = 2;
                            break;
                        case 2:
                            move = 3;
                            break;
                        case 3:
                            move = 3;
                            break;
                    }
                }
                
                switch (curPath.update(unit.possition(), sta, unit.getForm()))
                {
                    case -1:
                        if(unit.getForm() != 2)
                        {
                            unit.setFall(0);
                        }
                        break;
                    case 1:
                        if(unit.getForm()!=2)
                        {
                            unit.setFall(1);
                        }
                       
                        break;
                    case 2:
                        menu.setGameSta(-1);
                        unit.reset();
                        hold = -1;
                        break;
                }
                switch(curDeter.update(unit.possition()))
                {
                    case -2:
                        // tranform
                        int form = unit.setTranform(curPath.getSpeed());
                        if (form == 2) 
                        {
                            move = 3;
                        }
                        if (form == 0) 
                        {
                            // tang speed
                            curPath.setSpeed3(1);
                            curDeter.setSpeed3(1);
                        }
                        break;
                    case -1:
                        // lose game
                        menu.setGameSta(-1);
                        unit.reset();
                        hold = -1;
                        //menu.unlock(levelmenu);
                        break;
                    case 0:
                        //wingame
                        menu.setGameSta(1);
                        unit.reset();
                        hold = -1;
                        break;
                }
                sta = unit.update(move);
                move = 0;
            }
            else
            {
                hold = menu.update();
                if (hold != -1)
                {
                    curPath =pathBase[hold].copyMap(pathBase[hold]);
                    curDeter = deterBase[hold].copyMap(deterBase[hold]);
                }
            }
            lastMouse = curMouse;
            background.update(hold % 2);
            base.Update(gameTime);
        }
        

        /// <summary>
        /// This is called hen the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
           
            GraphicsDevice.Clear(Color.CornflowerBlue);
            background.draw(hold % 2);
            if (hold != -1) 
            {
                unit.draw();
                curPath.draw();
                curDeter.draw();
            }
            else
            {
                menu.draw();
            }
            //
            spriteBatch.Begin(sortMode: SpriteSortMode.Immediate);
            spriteBatch.DrawString(font, $"Point: {unit.getspeed()}", new Vector2(0, 0), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
