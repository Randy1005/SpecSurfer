using tainicom.Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
// to fix the ambiguous reference for Vector2 between XNA and Aether
// use "Vector2" for XNA 
// use "tainicom.Aether.Physics2D.Common.Vector2" for Aether


namespace SpectrumSurfer
{
    public class Game1 : Game
    {

        private Camera _camera;

        public static int ScreenWidth;
        public static int ScreenHeight;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private BasicEffect _spriteBatchEffect;
        private bool isWaving = false;

        private Texture2D _playerTexture;
        private Texture2D _groundTexture;
        private Vector2 _playerTextureSize;
        private Vector2 _groundTextureSize;
        private Vector2 _playerTextureOrigin;
        private Vector2 _groundTextureOrigin;


        public static float playerSpeed = 0.06f;

        private KeyboardState _oldKeyState;
        

        // Simple camera controls
        //private Vector3 _cameraPosition = new Vector3(0, 1.70f, 0); // camera is 1.7 meters above the ground
        //float cameraViewWidth = 12.5f; // camera is 12.5 meters wide.


        // physics
        private World _world;
        public static Body _playerBody;
        private Body _groundBody;
        public static Vector2 _playerBodySize = new Vector2(0.7f, 0.7f); // 0.7 x 0.7
        private Vector2 _groundBodySize = new Vector2(15f, 1f); // ground is 15x1 meters
        private tainicom.Aether.Physics2D.Common.Vector2 _basePlayerPosition;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;


            _graphics.GraphicsProfile = GraphicsProfile.Reach;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

        }

        protected override void Initialize()
        {

            base.Initialize();


            // initialize screen width / height
            ScreenHeight = _graphics.PreferredBackBufferHeight;
            ScreenWidth = _graphics.PreferredBackBufferWidth;

            // Create a world
            _world = new World();

            /* Circle */
            tainicom.Aether.Physics2D.Common.Vector2 playerPosition = new tainicom.Aether.Physics2D.Common.Vector2(0, _playerBodySize.X);

            // Create the player fixture
            _playerBody = _world.CreateRectangle(_playerBodySize.X, _playerBodySize.Y, 1f, playerPosition);
            _playerBody.BodyType = BodyType.Dynamic;

            // Give it some bounce and friction
            _playerBody.SetRestitution(0.5f);
            _playerBody.SetFriction(0.8f);

            /* Ground */
            tainicom.Aether.Physics2D.Common.Vector2 groundPosition = new tainicom.Aether.Physics2D.Common.Vector2(0, -(_groundBodySize.Y / 2f));

            // Create the ground fixture
            _groundBody = _world.CreateRectangle(_groundBodySize.X, _groundBodySize.Y, 1f, groundPosition);
            _groundBody.BodyType = BodyType.Static;

            _groundBody.SetRestitution(0.3f);
            _groundBody.SetFriction(0.5f);

        }

        protected override void LoadContent()
        {

            _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);

            // load camera
            _camera = new Camera();



            // Load sprites
            _playerTexture = Content.Load<Texture2D>("CircleSprite");
            _groundTexture = Content.Load<Texture2D>("GroundSprite");

            // We scale the ground and player textures at body dimensions
            _playerTextureSize = new Vector2(_playerTexture.Width, _playerTexture.Height);
            _groundTextureSize = new Vector2(_groundTexture.Width, _groundTexture.Height);

            // We draw the ground and player textures at the center of the shapes
            _playerTextureOrigin = _playerTextureSize / 2f;
            _groundTextureOrigin = _groundTextureSize / 2f;
        }

        protected override void Update(GameTime gameTime)
        {
            HandleKeyboard(gameTime);

            float totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // We update the world
            _world.Step(totalSeconds);


            //// just a test of sine wave
            //if (isWaving)
            //{
            //    _playerBody.Position =
            //        new tainicom.Aether.Physics2D.Common.Vector2(
            //            _playerBody.Position.X + 0.03f,
            //            _basePlayerPosition.Y + 0.5f + (-(float)Math.Cos(_playerBody.Position.X * 3f) * 0.5f)
            //            );
            //}

            _camera.Follow(_playerBody);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Default color, could change it
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Update camera View and Projection.
            //var vp = GraphicsDevice.Viewport;
            //_spriteBatchEffect.View = Matrix.CreateLookAt(_cameraPosition, _cameraPosition + Vector3.Forward, Vector3.Up);
            //_spriteBatchEffect.Projection = Matrix.CreateOrthographic(cameraViewWidth, cameraViewWidth / vp.AspectRatio, 0f, -1f);

            // Draw player and ground. 
            // Our View/Projection requires RasterizerState.CullClockwise and SpriteEffects.FlipVertically.
            _spriteBatch.Begin(transformMatrix: _camera.Transform);
            _spriteBatch.Draw(_playerTexture, new Vector2(_playerBody.Position.X, _playerBody.Position.Y), null, Color.White, _playerBody.Rotation, _playerTextureOrigin, _playerBodySize / _playerTextureSize, SpriteEffects.FlipVertically, 0f);
            _spriteBatch.Draw(_groundTexture, new Vector2(_groundBody.Position.X, _groundBody.Position.Y), null, Color.White, _groundBody.Rotation, _groundTextureOrigin, _groundBodySize / _groundTextureSize, SpriteEffects.FlipVertically, 0f);
            _spriteBatch.End();



            base.Draw(gameTime);
        }


        private void HandleKeyboard(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            float totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            var vp = GraphicsDevice.Viewport;

            // Move camera
            //if (state.IsKeyDown(Keys.Left))
            //    _cameraPosition.X -= totalSeconds * cameraViewWidth;

            //if (state.IsKeyDown(Keys.Right))
            //    _cameraPosition.X += totalSeconds * cameraViewWidth;

            //if (state.IsKeyDown(Keys.Up))
            //    _cameraPosition.Y += totalSeconds * cameraViewWidth;

            //if (state.IsKeyDown(Keys.Down))
            //    _cameraPosition.Y -= totalSeconds * cameraViewWidth;


            // We make it possible to move the player left and right
            if (state.IsKeyDown(Keys.A))
                _playerBody.Position = new tainicom.Aether.Physics2D.Common.Vector2(_playerBody.Position.X - playerSpeed, _playerBody.Position.Y);

            if (state.IsKeyDown(Keys.D))
                _playerBody.Position = new tainicom.Aether.Physics2D.Common.Vector2(_playerBody.Position.X + playerSpeed, _playerBody.Position.Y);

            if (state.IsKeyDown(Keys.Space) && _oldKeyState.IsKeyUp(Keys.Space)) 
            {
                isWaving = true;
                _basePlayerPosition = _playerBody.Position;
            }
            if (state.IsKeyDown(Keys.Escape))
                Exit();

            _oldKeyState = state;
        }


    }
}
