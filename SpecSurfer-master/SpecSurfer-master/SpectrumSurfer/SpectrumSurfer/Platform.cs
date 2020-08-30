using tainicom.Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace SpectrumSurfer
{
    class Platform
    {

        // Textures
        private Texture2D _platformTexture;
        private Vector2 _platformTextureSize;
        private Vector2 _platformTextureOrigin;

        // physics properties
        public Body _platformBody;

        public tainicom.Aether.Physics2D.Common.Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        tainicom.Aether.Physics2D.Common.Vector2 position;

        public tainicom.Aether.Physics2D.Common.Vector2 Rect
        {
            get { return rect; }
            set { rect = value; }
        }
        tainicom.Aether.Physics2D.Common.Vector2 rect;

        // constructor
        public Platform(
            tainicom.Aether.Physics2D.Common.Vector2 pos,
            tainicom.Aether.Physics2D.Common.Vector2 rect,
            World world)
        {
            this.Position = pos;
            this.Rect = rect;

            // create platform fixture
            this._platformBody = world.CreateRectangle(Rect.X, Rect.Y, 1f, Position);
            this._platformBody.BodyType = BodyType.Static;

            // bounce and friction
            this._platformBody.SetRestitution(0.3f);
            this._platformBody.SetFriction(0.5f);

            LoadContent();
        }


        public void LoadContent()
        {
            // load textures
            _platformTexture = Global.content.Load<Texture2D>("GroundSprite");

            // scale player body textures
            _platformTextureSize = new Vector2(_platformTexture.Width, _platformTexture.Height);

            // set the textures at the center
            _platformTextureOrigin = _platformTextureSize / 2f;
        }


        public void Draw(GameTime gameTime)
        {
            Global._spriteBatch.Draw(
                _platformTexture,
                new Vector2(_platformBody.Position.X,
                _platformBody.Position.Y),
                null,
                Color.White,
                _platformBody.Rotation,
                _platformTextureOrigin,
                new Vector2(Rect.X, Rect.Y) / _platformTextureSize,
                SpriteEffects.FlipVertically, 0f);
        }

    }
}
