using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Fluids;
using System.Diagnostics;

namespace SpectrumSurfer
{
    public class Camera
    {
        
        public Matrix Transform
        {
            get;
            private set;
        }

        public void Follow(Body target)
        {
            var offset = Matrix.CreateTranslation(Game1.ScreenWidth / 2f, Game1.ScreenHeight / 2f, 0f);
            var position = Matrix.CreateTranslation(
                -target.Position.X,
                -target.Position.Y,
                0f);

            var zoom = Matrix.CreateScale(1.5f, 1.5f, 1f);


            Transform = position * offset * zoom;
        }
    }
}
