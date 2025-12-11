using Gum.Forms;
using Gum.Forms.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using SharpDX.X3DAudio;
using Syncfusion.XForms.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms.Xaml;


namespace ToeJam___Earl_2._0_
{
    public class PresentsScrpit
{
        public Texture2D Texture { get; set; } // get and set this varible. 
        public Vector2 Position { get; set; }
       public Vector2 Origin { get; set; } // Optional, for rotation/scaling around a point
        public Color Color { get; set; }
        public float Scale { get; set; }

        public Rectangle Rect { get; set; }
        public PresentsScrpit(Texture2D texture, Vector2 position, float scale, Rectangle rect)
        {
            Texture = texture;
            Position = position;
           Origin = Vector2.Zero; // Default origin to top-left
            Color = Color.White;
            Scale = scale;
            Rect = rect;
        }
    }





}
