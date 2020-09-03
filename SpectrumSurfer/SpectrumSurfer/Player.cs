using tainicom.Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Net;

namespace SpectrumSurfer
{
    public class Player
    {

        // for light indicators
        public int OwnColorAmount;
        public int CurrentColor;
        public bool IsLMBHolding;


        // Textures
        private Texture2D _playerTexture;
        private Vector2 _playerTextureSize;
        private Vector2 _playerTextureOrigin;

        private KeyboardState _oldKeyState;



        // physics properties
        public Body _playerBody;

        // store the player's base position before performing each wave
        public tainicom.Aether.Physics2D.Common.Vector2 _basePlayerPosition;

        private World _world;


        //Mouse wheel value
        int currentScrollValue = 0;
        int previousScrollValue = 0;

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
            _world = world;

            // create player fixture
            this._playerBody = _world.CreateRectangle(Rect.X, Rect.Y, 5f, Position);
            this._playerBody.BodyType = BodyType.Dynamic;

            // bounce and friction
            this._playerBody.SetRestitution(0.1f);
            this._playerBody.SetFriction(0.5f);

            this._playerBody.SleepingAllowed = true;


            // light indicator controls
            CurrentColor = 0;
            IsLMBHolding = false;

            // load textures
            LoadContent();

            OwnColorAmount = 3;
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

            if (state.IsKeyDown(Keys.Space) && IsLMBHolding)
            {
                rideWave(getColorIndex());
            }

            if (state.IsKeyUp(Keys.Space) && !IsLMBHolding)
            {
                _basePlayerPosition = _playerBody.Position;
            }


            // pause the body if player is choosing color
            if (IsLMBHolding)
            {
                _playerBody.Awake = false;
            } else
            {
                _playerBody.Awake = true;
            }


            scrollMouseWheel();

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

        public void scrollMouseWheel()
        {
            MouseState Mstate = Mouse.GetState();
            currentScrollValue = Mstate.ScrollWheelValue;
            if (currentScrollValue > previousScrollValue)
            {
                if (CurrentColor < OwnColorAmount -1)
                {
                    CurrentColor++;
                }
                else 
                {
                    CurrentColor = 0;
                }
                
            }else if(previousScrollValue >currentScrollValue){
                
                if (CurrentColor > 0)
                {
                    CurrentColor--;
                }
                else
                {
                    CurrentColor = OwnColorAmount - 1;
                }
            }
            previousScrollValue = currentScrollValue;

            if (Mstate.LeftButton == ButtonState.Pressed)
            {
                IsLMBHolding = true;
            }
            else {
                IsLMBHolding = false;
            }
        }

        public int getColorIndex() {
            int i = CurrentColor;
            return i;
        }

        public bool getIsHoldingLMB() {
            bool temp = IsLMBHolding;
            return temp;
        }


        public void rideWave(int currColor)
        {
            if (currColor == 0)
            {
                _playerBody.Position =
                    new tainicom.Aether.Physics2D.Common.Vector2(
                        _playerBody.Position.X + 0.02f,
                        _basePlayerPosition.Y + 0.4f + (-(float)Math.Cos(_playerBody.Position.X * 3f) * 0.4f)
                    );
            } 
            else if (currColor == 1)
            {
                _playerBody.Position =
                    new tainicom.Aether.Physics2D.Common.Vector2(
                        _playerBody.Position.X + 0.01f,
                        _basePlayerPosition.Y + 1f + (-(float)Math.Cos(_playerBody.Position.X * 6f) * 1f)
                    );
            }
            else if (currColor == 2)
            {
                _playerBody.Position =
                    new tainicom.Aether.Physics2D.Common.Vector2(
                        _playerBody.Position.X + 0.01f,
                        _basePlayerPosition.Y + 1.8f + (-(float)Math.Cos(_playerBody.Position.X * 6f) * 1.8f)
                    );
            }

            
        }


    }
}
