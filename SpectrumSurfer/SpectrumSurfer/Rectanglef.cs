using System;
using System.Collections.Generic;
using System.Text;

namespace SpectrumSurfer
{
    public class Rectanglef
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public Rectanglef(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public float Top
        {
            get { return Y + Height; }
        }
        public float Bottom
        {
            get { return Y; }
        }
        public float Left
        {
            get { return X; }
        }
        public float Right
        {
            get { return X + Width; }
        }
    }
}
