using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ToeJam___Earl_2._0_
{
    public class StaticObjects
    {
        private int frameCount;
        private Texture2D myTexture; // The animation spritesheet.
        private float timePerFrame; // Number of frames to draw per second.
        private int frame; // The current frame being drawn.  
        public float Rotation, Scale, Depth;// Current rotation, scale and draw depth for the animation.
        public Vector2 Origin; // The origin point of the animated texture.
        private Rectangle rect; // Certain area of the sprite image/ texture.
        
       
      
        public StaticObjects(Vector2 origin, Rectangle rect, float rotation, float scale, float depth)
        {
            this.Origin = origin;
            this.rect = rect;
            this.Rotation = rotation;
            this.Scale = scale;
            this.Depth = depth;
        }

        public void Load(ContentManager content, string asset, int frameCount, int framesPerSec)
        {
            this.frameCount = frameCount;
            myTexture = content.Load<Texture2D>(asset);
            timePerFrame = (float)1 / framesPerSec;
            frame = 0;
        }
        public void DrawFrame(SpriteBatch batch, Vector2 screenPos)
        {
            DrawFrame(batch, frame, screenPos);
        }

        public void DrawFrame(SpriteBatch batch, int frame, Vector2 screenPos)
        {
            int FrameWidth = myTexture.Width;
            Rectangle sourcerect = new Rectangle(FrameWidth * frame, 0,
                FrameWidth, myTexture.Height);
            batch.Draw(myTexture, screenPos, sourcerect, Color.White,
                Rotation, Origin, Scale, SpriteEffects.None, Depth);
        }

    }


}
    
    

    
    



 
   
  


    


