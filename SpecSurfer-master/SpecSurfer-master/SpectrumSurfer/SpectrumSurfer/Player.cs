using tainicom.Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace SpectrumSurfer
{
    class Player
    {

        // for light indicators
        public int CurrentColor;
        public bool IsRMBHolding;


        // Textures
        private Texture2D _playerTexture;
        private Vector2 _playerTextureSize;
        private Vector2 _playerTextureOrigin;

        private KeyboardState _oldKeyState;



        // physics properties
        public Body _playerBody;
        

        public tainicom.Aether.Physics2D.Common.Vector2 Position
        {
            get { return position;  }
            set { position = value; }
        }
        tainicom.Aether.Physics2D.Common.Vector2 position;

        public tainicom.Aether.Physics2D.Common.Vector2 Velocity
        {
            get { return velocity;  }
            set { velocity = value;  }
        }
        tainicom.Aether.Physics2D.Common.Vector2 velocity;


        public tainicom.Aether.Physics2D.Common.Vector2 Rect
        {
            get { return rect;  }
            set { rect = value;  }
        }
        tainicom.Aether.Physics2D.Common.Vector2 rect;

        // constructor here
        public Player(
            tainicom.Aether.Physics2D.Common.Vector2 pos,
            tainicom.Aether.Physics2D.Common.Vector2 rect,
            World world)
        {
            this.Position = pos;
            this.Rect = rect;

            // create player fixture
            this._playerBody = world.CreateRectangle(Rect.X, Rect.Y, 1f, Position);
            this._playerBody.BodyType = BodyType.Dynamic;

            // bounce and friction
            this._playerBody.SetRestitution(0.5f);
            this._playerBody.SetFriction(0.8f);


            // light indicator controls
            CurrentColor = 0;
            IsRMBHolding = false;

            // load textures
            LoadContent();
        }



        public void LoadContent()
        {
            // load textures
            _playerTexture = Global.content.Load<Texture2D>("CircleSprite");

            // scale player body textures
            _playerTextureSize = new Vector2(_playerTexture.Width, _playerTexture.Height);

            // set the textures at the center
            _playerTextureOrigin = _playerTextureSize / 2f;
        }

        // update function for the player
        public void Update(GameTime gameTime,
            KeyboardState state,
            DisplayOrientation orientation)
        {

            HandleInput(state, orientation);
        
        }

        public void HandleInput(
            KeyboardState state,
            DisplayOrientation orientation)
        {
            // player moves left or right
            if (state.IsKeyDown(Keys.D))
                _playerBody.LinearVelocity = new tainicom.Aether.Physics2D.Common.Vector2(1f, 0f);

            if (state.IsKeyDown(Keys.A))
                _playerBody.LinearVelocity = new tainicom.Aether.Physics2D.Common.Vector2(-1f, 0f);




            _oldKeyState = state;
        }

        public void Draw(GameTime gameTime)
        {
            Global._spriteBatch.Draw(
                _playerTexture, 
                new Vector2(_playerBody.Position.X, 
                _playerBody.Position.Y), 
                null, 
                Color.White, 
                _playerBody.Rotation, 
                _playerTextureOrigin, 
                new Vector2(Rect.X, Rect.Y) / _playerTextureSize, 
                SpriteEffects.FlipVertically, 0f);
        }

    }
}
