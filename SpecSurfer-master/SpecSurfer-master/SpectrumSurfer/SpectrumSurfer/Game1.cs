using tainicom.Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
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



        // Simple camera controls
        private Vector3 _cameraPosition = new Vector3(0, 1.70f, 0); // camera is 1.7 meters above the ground
        float cameraViewWidth = 12.5f; // camera is 12.5 meters wide.


        // physics
        private World _world;
   

        // wavelength paht indicator
        public Indicator indicator;
        public Texture2D _idr;

        // player 
        private Player player;

        // the platform will need its own class too
        private Platform platform1;

       
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

        }

        protected override void Initialize()
        {

            base.Initialize();

            // Create a world
            _world = new World();

            // Create a player
            tainicom.Aether.Physics2D.Common.Vector2 playerPos = new tainicom.Aether.Physics2D.Common.Vector2(0f, 1f);
            tainicom.Aether.Physics2D.Common.Vector2 playerRect = new tainicom.Aether.Physics2D.Common.Vector2(0.7f , 0.7f);
            player = new Player(playerPos, playerRect, _world);

            // and give that player a velocity to move left or right
            player.Velocity = new tainicom.Aether.Physics2D.Common.Vector2(0.06f, 0);

            // create the wavelength indicator
            // Instanciate Indicator
            indicator = new Indicator(player.CurrentColor, player.IsRMBHolding, new Vector2(100, 100));

            /* Platforms */
            tainicom.Aether.Physics2D.Common.Vector2 platRect1 = new tainicom.Aether.Physics2D.Common.Vector2(15f, 1f); // 15 x 1
            tainicom.Aether.Physics2D.Common.Vector2 platPos1 = new tainicom.Aether.Physics2D.Common.Vector2(0, -(platRect1.Y / 2f));
            platform1 = new Platform(platPos1, platRect1, _world);


        }

        protected override void LoadContent()
        {
            Global.content = this.Content;
            Global._spriteBatch = new SpriteBatch(Global._graphics.GraphicsDevice);

            // We use a BasicEffect to pass our view/projection in _spriteBatch
            Global._spriteBatchEffect = new BasicEffect(Global._graphics.GraphicsDevice);
            Global._spriteBatchEffect.TextureEnabled = true;


            // Load sprites
            //_groundTexture = Global.content.Load<Texture2D>("GroundSprite");
            _idr = Global.content.Load<Texture2D>("Indicator");
            
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


            Debug.WriteLine(player._playerBody.Position);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Default color, could change it
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Update camera View and Projection.
            var vp = GraphicsDevice.Viewport;
            Global._spriteBatchEffect.View = Matrix.CreateLookAt(_cameraPosition, _cameraPosition + Vector3.Forward, Vector3.Up);
            Global._spriteBatchEffect.Projection = Matrix.CreateOrthographic(cameraViewWidth, cameraViewWidth / vp.AspectRatio, 0f, -1f);

            
            // Our View/Projection requires RasterizerState.CullClockwise and SpriteEffects.FlipVertically.
            Global._spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, RasterizerState.CullClockwise, Global._spriteBatchEffect);

            // remember to draw everything 
            platform1.Draw(gameTime);
            player.Draw(gameTime);
            indicator.Draw();
            
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
            
        }

    }
}
