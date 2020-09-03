using tainicom.Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Net.Mime;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace SpectrumSurfer
{
    public class Particle
    {
        
        private Texture2D cube;
        public Vector2 velocity;
        public Vector2 speed;
        public Vector2 position;
        private Vector2 initailPos;
        public int ColorIndex;
        public Rectanglef objRect;

        public Body _particleBody;
        public bool toBeDestroyed;
        private World _world;


        public tainicom.Aether.Physics2D.Common.Vector2 Rect
        {
            get { return rect; }
            set { rect = value; }
        }
        tainicom.Aether.Physics2D.Common.Vector2 rect;

        public Particle(string PATH, Vector2 Speed, Vector2 Position, int color, World world) {

            cube = Global.content.Load<Texture2D>(PATH);
            speed = Speed;
            position = Position;
            initailPos = Position;
            ColorIndex = color;
            _world = world;
            toBeDestroyed = false;

            Rect = new tainicom.Aether.Physics2D.Common.Vector2(0.1f, 0.1f);

            this._particleBody = world.CreateRectangle(Rect.X, Rect.Y, 1f, new tainicom.Aether.Physics2D.Common.Vector2(Position.X, Position.Y));
            this._particleBody.BodyType = BodyType.Dynamic;
            this._particleBody.SetIsSensor(true);

            // define the rectangle boundary for the particle
            //this.objRect = new Rectanglef(Position.X - cube.Width / 2, Position.Y - cube.Height / 2, cube.Width, cube.Height);

        }

        public void Update() {


            //foreach (Contact contact in _world.ContactList)
            //{
            //    foreach (Platform platform in Game1.platforms)
            //        if ((contact.FixtureA.Body == _particleBody && contact.FixtureB.Body == platform._platformBody) ||
            //            (contact.FixtureB.Body == _particleBody && contact.FixtureA.Body == platform._platformBody))
            //        {
            //            this.toBeDestroyed = true;
            //        }
            //}


            performSineWaveMovement(ColorIndex);
            detectIfOutScreen();
        }

        public void Draw() {
            Global._spriteBatch.Draw(cube, new Vector2(position.X + 0.5f, position.Y), null, new Color(255, 255, 255, 255), 0.0f, new Vector2(cube.Width/2, cube.Height / 2), new Vector2(0.0024f, 0.0024f), SpriteEffects.FlipVertically, 0f);
        }

        public void performSineWaveMovement(int index) {
            switch (index)
            {

                case 0:
                    velocity = new Vector2(this.position.X + 0.1f, ((float)Math.Cos(4 * (this.position.X + 0.1f)))) - position;
                    velocity.Normalize();
                    break;
                case 1:
                    velocity = new Vector2(this.position.X + 0.1f, 2 * ((float)Math.Cos(2 * (this.position.X + 0.1f)))) - position;
                    velocity.Normalize();
                    break;
                case 2:
                    velocity = new Vector2(this.position.X + 0.1f, 4 * ((float)Math.Cos(this.position.X + 0.1f))) - position;
                    velocity.Normalize();
                    break;
            }

            position += velocity / 40;

        }

        public void detectIfOutScreen() {
            if (position.X - initailPos.X > 3) {
                this.position = this.initailPos;
            }
        }

        public bool isTouchingLeft(Rectanglef otherObjRect)
        {
            return (this.objRect.Right + this.velocity.X > otherObjRect.Left &&
                    this.objRect.Left < otherObjRect.Left &&
                    this.objRect.Bottom > otherObjRect.Top &&
                    this.objRect.Top < otherObjRect.Bottom);
        }

        public bool isTouchingRight(Rectanglef otherObjRect)
        {
            return (this.objRect.Right + this.velocity.X < otherObjRect.Right &&
                    this.objRect.Left > otherObjRect.Right &&
                    this.objRect.Bottom > otherObjRect.Top &&
                    this.objRect.Top < otherObjRect.Bottom);
        }

        public bool isTouchingTop(Rectanglef otherObjRect)
        {
            return (this.objRect.Bottom + this.velocity.Y < otherObjRect.Top &&
                    this.objRect.Top > otherObjRect.Top &&
                    this.objRect.Right > otherObjRect.Left &&
                    this.objRect.Left < otherObjRect.Right);
        }

        public bool isTouchingBottom(Rectanglef otherObjRect)
        {
            return (this.objRect.Top + this.velocity.Y > otherObjRect.Bottom &&
                    this.objRect.Bottom < otherObjRect.Bottom &&
                    this.objRect.Right > otherObjRect.Left &&
                    this.objRect.Left < otherObjRect.Right);
        }


    }
}
