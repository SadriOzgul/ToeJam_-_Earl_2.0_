using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ToeJam___Earl_2._0_
{
    public class Shapes
{
      Texture2D Texture { get; set; }
      public Vector2 Position { get; set; }
      public Color Color { get; set; } = Color.White;

      public Rectangle Rectangle { get; set; }

        

      public Shapes(Texture2D texture, Vector2 position, Rectangle rect, Color color)
      {
        Texture = texture;
        Position = position;
        Rectangle = rect;
        Color = color;
       
      }

    public void Draw (SpriteBatch spritebatch)
        {
            spritebatch.Draw(Texture, Position, Rectangle, Color);
        }

      



    }




}
