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
    public class ParticleSystem
    {
        public int amount;
        public float gap;
        public List<Particle> Particles;
        public Vector2 position;
        public int ColorIndex;
        private World _world;


        public ParticleSystem(int _amount, float _gap, Vector2 _position, int _ColorIndex, World world) {
            amount = _amount;
            gap = _gap;
            position = _position;
            ColorIndex = _ColorIndex;

            Particles = new List<Particle>();


            for (int i = 0; i < amount; i++)
            {
                Particles.Add(new Particle("cube",new Vector2(0, 0), position, ColorIndex, world));
            }

            _world = world;



        }

        public void Update(List<Rectanglef> objRects) {


            for (int i = 0; i < Particles.Count - 1; i++)
            {


                Particles[i].Update();              
            }

        }

        public void Draw() {
            for (int i = 0; i < Particles.Count - 1; i++)
            {
                Particles[i].Draw();
            }
        }

        public void SpawnParticles() {
            for (int i = 0; i < Particles.Count - 1; i++)
            {

            }
        }
    }
}
