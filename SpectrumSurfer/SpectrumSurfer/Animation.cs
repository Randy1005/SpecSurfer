using tainicom.Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Xml.Serialization;

namespace SpectrumSurfer
{
    class Animation
    {
        float Alpha = 0.0f;
        float temp = 0.0f;
        float Amount = 0.0f;
        bool start = false;

        public float AlphaSmoothTransition(GameTime gameTime, float slowDown, bool reverse) {
            if (start && !reverse)
            {
                if (Amount >= 0 && Amount <= 1)
                {
                    var deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds / slowDown;
                    Alpha += deltaSeconds;
                    Amount = (float)(-(1.0f / 3.0f) * Math.Pow(Alpha, 3) + Alpha);
                    return Amount;
                }
                else
                {
                    start = false;
                    Alpha = 0.0f;
                    Amount = 0.0f;
                    return 0.0f;
                }
            }
            else if (start && reverse)
            {
                if (Amount >= -1.0f && Amount <= 0.0f)
                {
                    var deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds / slowDown;
                    Alpha -= deltaSeconds;
                    Amount = (float)(-(1.0f / 3.0f) * Math.Pow(Alpha, 3) + Alpha);
                    return Amount;
                }
                else{
                    Debug.WriteLine("111");
                    Amount = 0.0f;
                    Alpha = 0.0f;
                    Global.game.amStart = false;
                    return 0.0f;
                }
            }
            else 
            {
                return 0;
                
            }        
        }

        public float getAlpha() {
            return Alpha;
        }

        public bool timer(GameTime gameTime, float Duration, Game1 game) {

            var deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            temp += deltaSeconds;

            Debug.WriteLine(temp);

            if (temp > Duration)
            {
                temp = 0.0f;
                game.witch = false;
                return false;
            }
            else 
            {
                return true;
            }
        }
    }
}
