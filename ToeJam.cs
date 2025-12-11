using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace ToeJam___Earl_2._0_
{
    public class ToeJam //Animated Texture reused but for ToeJam only.
    {
        private int frameCount; // Number of frames within the animation.
        private Texture2D myTexture; // The animation spritesheet.
        private float timePerFrame; // Number of frames to draw per second.
        private int frame; // The current frame being drawn. 
        private float totalElaspsed; // Total amount of time the animation has been runninhg 
        private bool isPaused; // Is the animation currently running?
        public float Rotation, Scale, Depth;// Current rotation, scale and draw depth for the animation.
        public Vector2 Origin; // The origin point of the animated texture.
        public Rectangle Rect;
        public ToeJam(Vector2 origin, Rectangle rect, float rotation, float scale, float depth)
        {
            this.Origin = origin; // this. Refers to THIS script that is currently writen in. Returns script.
            this.Rotation = rotation;
            this.Scale = scale;
            this.Depth = depth;
            this.Rect = rect;
        }

        public void Load(ContentManager content, string asset, int frameCount, int framesPerSec)
        {
            this.frameCount = frameCount;
            myTexture = content.Load<Texture2D>(asset);
            timePerFrame = (float)1 / framesPerSec;
            frame = 0;
            isPaused = false;
        }

        public void updateFrame(float elapsed)
        {
            if (isPaused)
                return;
            totalElaspsed += elapsed;
            if (totalElaspsed > timePerFrame)
            {
                frame++;
                // Keep the Frame between 0 and the total frames, minus one.
                frame %= frameCount;
                totalElaspsed -= timePerFrame;
            }
        }

        public void DrawFrame(SpriteBatch batch, Vector2 screenPos)
        {
            DrawFrame(batch, frame, screenPos);
        }

        public void DrawFrame(SpriteBatch batch, int frame, Vector2 screenPos)
        {
            int FrameWidth = myTexture.Width / frameCount;
            Rectangle sourcerect = new Rectangle(FrameWidth * frame, 0,
                FrameWidth, myTexture.Height);
            batch.Draw(myTexture, screenPos, sourcerect, Color.White,
                Rotation, Origin, Scale, SpriteEffects.None, Depth);
        }

        public bool IsPaused
        {
            get { return isPaused; }
        }

        public void Reset()
        {
            frame = 0;
            totalElaspsed = 0f;
        }

        public void Stop()
        {
            Pause();
            Reset();
        }

        public void Play()
        {
            isPaused = false;
        }

        public void Pause()
        {
            isPaused = true;
        }

    }













}
