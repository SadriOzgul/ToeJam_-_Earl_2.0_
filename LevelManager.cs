using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ToeJam___Earl_2._0_
{
    public static class LevelManager
    {
        public static int CurrentLevel = 1;

        // Required fields
        public static Texture2D LevelBackground;
        public static List<Vector2> EnemyPositions = new List<Vector2>();
        public static Rectangle ElevatorRect;

        // Create a simple 1x1 colored texture
        public static Texture2D CreateColorTexture(GraphicsDevice device, Color color)
        {
            Texture2D tex = new Texture2D(device, 1, 1);
            tex.SetData(new[] { color });
            return tex;
        }

        public static void LoadLevel(ContentManager content, GraphicsDevice graphics, int level)

        {
            CurrentLevel = level;
            EnemyPositions.Clear();
          

            GraphicsDevice device = graphics;

            switch (level)
            {
                case 1:
                    LevelBackground = content.Load<Texture2D>("Level_One");
                    ElevatorRect = new Rectangle(900, 260, 100, 200);
                   
                    break;

                 

                case 2:
                    LevelBackground = content.Load<Texture2D>("Level_Two");
                    ElevatorRect = new Rectangle(900, 260, 100, 200);
                    EnemyPositions.Add(new Vector2(600, 300));


                    break;

                case 3:
                    LevelBackground = CreateColorTexture(device, Color.MediumPurple);
                    ElevatorRect = new Rectangle(900, 200, 130, 180);

                   
                    break;

                default:
                    LevelBackground = CreateColorTexture(device, Color.DarkSlateGray);
                    ElevatorRect = new Rectangle(800, 200, 130, 180);

                    break;
            }
        }
    }
}
