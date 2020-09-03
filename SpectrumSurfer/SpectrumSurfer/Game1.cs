using tainicom.Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using System.Linq;

// to fix the ambiguous reference for Vector2 between XNA and Aether
// use "Vector2" for XNA 
// use "tainicom.Aether.Physics2D.Common.Vector2" for Aether


// Reference: wave function
/*
private tainicom.Aether.Physics2D.Common.Vector2 _basePlayerPosition;
if (isWaving)
{
    _playerBody.Position =
        new tainicom.Aether.Physics2D.Common.Vector2(
            _playerBody.Position.X + 0.03f,
            _basePlayerPosition.Y + 0.5f + (-(float)Math.Cos(_playerBody.Position.X * 3f) * 0.5f)
            );
}
*/




namespace SpectrumSurfer
{
    public class Game1 : Game
    {
        // temporarily useless
        private bool isWaving = false;

        //New
        public bool amStart = true;
        public bool amReverse = false;
        public bool witch = false;
        public ParticleSystem PS;


        // Simple camera controls
        private Vector3 _cameraPosition = new Vector3(0, 1.70f, 0); // camera is 1.7 meters above the ground
        float cameraViewWidth = 12.5f; // camera is 12.5 meters wide.


        // physics
        private World _world;
        public List<Rectanglef> objRects;
   
        // wavelength path indicator
        public Indicator indicator;
        public Texture2D _idr;

        // player 
        public Player player;

        // the platforms will need their own class too
        static public List<Platform> platforms;

        public Game1()
        {
            Global._graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Global._graphics.GraphicsProfile = GraphicsProfile.Reach;
            Global._graphics.PreferredBackBufferWidth = 800;
            Global._graphics.PreferredBackBufferHeight = 480;
            Global._graphics.IsFullScreen = false;
            Global._graphics.ApplyChanges();
            Global.game = this;
        }

        protected override void Initialize()
        {

            base.Initialize();


            // Create a world
            _world = new World();

            objRects = new List<Rectanglef>();
            platforms = new List<Platform>();

            // Create a player
            tainicom.Aether.Physics2D.Common.Vector2 playerPos = new tainicom.Aether.Physics2D.Common.Vector2(0f, 1f);
            tainicom.Aether.Physics2D.Common.Vector2 playerRect = new tainicom.Aether.Physics2D.Common.Vector2(0.7f , 0.7f);
            player = new Player(playerPos, playerRect, _world);

            // and give that player a velocity to move left or right
            player.Velocity = new tainicom.Aether.Physics2D.Common.Vector2(0.06f, 0);

            // create the wavelength indicator
            // Instanciate Indicator
            indicator = new Indicator(player.CurrentColor, new Vector2(100, 100));

            /* Platforms */
            tainicom.Aether.Physics2D.Common.Vector2 platRect1 = new tainicom.Aether.Physics2D.Common.Vector2(15f, 2f);
            tainicom.Aether.Physics2D.Common.Vector2 platPos1 = new tainicom.Aether.Physics2D.Common.Vector2(0f, -1f);
            platforms.Add(new Platform(platPos1, platRect1, _world));

            tainicom.Aether.Physics2D.Common.Vector2 platRect2 = new tainicom.Aether.Physics2D.Common.Vector2(13f, 1f);
            tainicom.Aether.Physics2D.Common.Vector2 platPos2 = new tainicom.Aether.Physics2D.Common.Vector2(15f, 3f);
            platforms.Add(new Platform(platPos2, platRect2, _world));




            Global.am = new Animation();
            Global.gameTime = new GameTime();

            PS = new ParticleSystem(15, 0.1f, new Vector2(player._playerBody.Position.X, player._playerBody.Position.Y), player.getColorIndex(), _world);

        }

        protected override void LoadContent()
        {

          
            Global.content = this.Content;
            Global._spriteBatch = new SpriteBatch(Global._graphics.GraphicsDevice);

            // We use a BasicEffect to pass our view/projection in _spriteBatch
            Global._spriteBatchEffect = new BasicEffect(Global._graphics.GraphicsDevice);
            Global._spriteBatchEffect.TextureEnabled = true;

            
        }

        protected override void Update(GameTime gameTime)
        {
    

            KeyboardState state = Keyboard.GetState();
            HandleKeyboard(gameTime);

            float totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // We update the world
            _world.Step(totalSeconds);

            // update the player
            player.Update(gameTime, state, Window.CurrentOrientation);

            // call indicator follow function
            indicator.Follow(new Vector2(player._playerBody.Position.X, player._playerBody.Position.Y));
            indicator.setColorIndex(player.getColorIndex());
            indicator.setToggle(player.getIsHoldingLMB());

            indicator.Update();


            PS.Update(objRects);


            cameraTranslate(gameTime);

            base.Update(gameTime);
            

        }

        protected override void Draw(GameTime gameTime)
        {
            // Default color, could change it
            GraphicsDevice.Clear(Color.FromNonPremultiplied(40,40,40,255)) ;

            // Update camera View and Projection.
            var vp = GraphicsDevice.Viewport;
            Global._spriteBatchEffect.View = Matrix.CreateLookAt(_cameraPosition, _cameraPosition + Vector3.Forward, Vector3.Up);
            Global._spriteBatchEffect.Projection = Matrix.CreateOrthographic(cameraViewWidth, cameraViewWidth / vp.AspectRatio, 0f, -1f);

            
            // Our View/Projection requires RasterizerState.CullClockwise and SpriteEffects.FlipVertically.
            Global._spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, RasterizerState.CullClockwise, Global._spriteBatchEffect);

            // remember to draw everything 
            foreach (var platform in platforms)
            {
                platform.Draw(gameTime);
            }

            player.Draw(gameTime);
            indicator.Draw();

            // New
            PS.Draw();
            Global._spriteBatch.End();

            

            base.Draw(gameTime);
        }


        private void HandleKeyboard(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            float totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move camera
            if (state.IsKeyDown(Keys.Left))
                _cameraPosition.X -= totalSeconds * cameraViewWidth;

            if (state.IsKeyDown(Keys.Right))
                _cameraPosition.X += totalSeconds * cameraViewWidth;

            if (state.IsKeyDown(Keys.Up))
                _cameraPosition.Y += totalSeconds * cameraViewWidth;

            if (state.IsKeyDown(Keys.Down))
                _cameraPosition.Y -= totalSeconds * cameraViewWidth;


            if (state.IsKeyDown(Keys.Escape))
                Exit();

            if (state.IsKeyDown(Keys.Space))
                if (!amStart)
                    amStart = true;
                else
                    amStart = false;

            if (state.IsKeyDown(Keys.F1))
                if (amReverse)
                    amReverse = false;
                else
                    amReverse = true;

            if (state.IsKeyDown(Keys.LeftAlt)) {
                witch = true;
            }

            
        }


        private void cameraTranslate(GameTime gameTime)
        {
            float totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (player.Velocity.X > 0 && Math.Abs(_cameraPosition.X - player._playerBody.Position.X) > 0.2f)
            {
                _cameraPosition.X += totalSeconds * cameraViewWidth;
            }

            if (player.Velocity.X < 0 && Math.Abs(_cameraPosition.X - player._playerBody.Position.X) > 0.2f)
            {
                _cameraPosition.X -= totalSeconds * cameraViewWidth;
            }



            if (_cameraPosition.Y - player._playerBody.Position.Y < 0 && Math.Abs(_cameraPosition.Y - player._playerBody.Position.Y) > 0.3f)
            {
                _cameraPosition.Y += totalSeconds * cameraViewWidth;
            }

            if (_cameraPosition.Y - player._playerBody.Position.Y > 0 && Math.Abs(_cameraPosition.Y - player._playerBody.Position.Y) > 0.3f)
            {
                _cameraPosition.Y -= totalSeconds * cameraViewWidth;
            }



        }

        

    }
}
