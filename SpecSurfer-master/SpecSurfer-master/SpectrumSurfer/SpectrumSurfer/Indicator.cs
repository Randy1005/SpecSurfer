﻿using tainicom.Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace SpectrumSurfer
{
    public class Indicator
    {
        private Texture2D RedLight, YellowLight, BlueLight;
        private Texture2D _IdrFrame;
        private List<Texture2D> LightList = new List<Texture2D>();
        public int _lightIndex;
        public bool _toggle;
        public Vector2 _playerPos, _Dims;
        public Vector2 _anchor;

        public Indicator(int lightIndex, bool toggle, Vector2 Dims)
        {
            _lightIndex = lightIndex;
            _toggle = toggle;
            _Dims = Dims;

            LightList.Add(RedLight);
            LightList.Add(YellowLight);
            LightList.Add(BlueLight);
            _anchor = new Vector2(-0.05f, -0.05f);

            _IdrFrame = Global.content.Load<Texture2D>("Indicator");
            LightList[0] = Global.content.Load<Texture2D>("RedLight");
            LightList[1] = Global.content.Load<Texture2D>("YellowLight");
            LightList[2] = Global.content.Load<Texture2D>("BlueLight");
        }

        public void Update()
        { 
            
        }

        public void Draw()
        {
            if (_IdrFrame != null) {

                Global._spriteBatch.Draw(_IdrFrame, new Vector2(_playerPos.X - 0.8f, _playerPos.Y), null, Color.White, 0.0f, new Vector2(_IdrFrame.Width/2, _IdrFrame.Height/2), new Vector2(0.012f, 0.012f), SpriteEffects.FlipVertically, 0f);
            }

            if (LightList[0] != null && LightList[1] != null && LightList[2] != null) {
                changeLightColorIdr(_lightIndex);
            }
        }

        public void changeLightColorIdr(int index) {

            switch (index)
            {
                case 0:
                    Global._spriteBatch.Draw(LightList[0], new Vector2(_playerPos.X - 0.8f, _playerPos.Y), null, Color.White, 0.0f, new Vector2(LightList[0].Width / 2, LightList[0].Height / 2), new Vector2(0.012f, 0.012f), SpriteEffects.FlipVertically, 0f);
                    //Debug.WriteLine("Red");
                    break;
                case 1:
                    Global._spriteBatch.Draw(LightList[1], new Vector2(_playerPos.X - 0.8f, _playerPos.Y), null, Color.White, 0.0f, new Vector2(LightList[1].Width / 2, LightList[1].Height / 2), new Vector2(0.012f, 0.012f), SpriteEffects.FlipVertically, 0f);
                    //Debug.WriteLine("Yellow");
                    break;
                case 2:
                    Global._spriteBatch.Draw(LightList[2], new Vector2(_playerPos.X - 0.8f, _playerPos.Y), null, Color.White, 0.0f, new Vector2(LightList[2].Width / 2, LightList[2].Height / 2), new Vector2(0.012f, 0.012f), SpriteEffects.FlipVertically, 0f);
                    //Debug.WriteLine("Blue");
                    break;

            }
        }

        public void Follow(Vector2 playerPos) {
            _playerPos = playerPos;
        }

        }
}