using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ToeJam___Earl_2._0_
{
    public class BeeEnemy
    {
        public enum BeeState { Patrol, Alert, Attack }
        private BeeState currentState = BeeState.Patrol;

        private AnimatedTexture anim;

        private Vector2 position;
        private Vector2 pointA;
        private Vector2 pointB;
        private Vector2 target;

        private float speed = 90f;
        private float alertRange = 200f;
        private float attackRange = 120f;

        private bool goingToB = true;

        // Track last animation so we don’t reload constantly
        private string currentAnimation = "";

        // Animation names
        private string animIdle = "IdleBees";
        private string animLeft = "LeftBees";
        private string animRight = "RightBees";
        private string animForward = "ForwardBees";
        private string animBackward = "BackwardBees";

        private ContentManager _content;

        public BeeEnemy(Vector2 startPos, Vector2 patrolPointA, Vector2 patrolPointB)
        {
            position = startPos;
            pointA = patrolPointA;
            pointB = patrolPointB;
            target = pointB;

            anim = new AnimatedTexture(Vector2.Zero, Rectangle.Empty, 0f, 2f, 0.5f);
        }

        public void Load(ContentManager content)
        {
            _content = content;  // save reference

            anim.Load(content, animIdle, 3, 2);
            currentAnimation = animIdle;
        }

        public void Update(GameTime gameTime, Vector2 playerPos)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float dist = Vector2.Distance(position, playerPos);

            switch (currentState)
            {
                case BeeState.Patrol:
                    Patrol(dt);
                    ChooseAnimation(target - position);
                    if (dist < alertRange)
                        currentState = BeeState.Alert;
                    break;

                case BeeState.Alert:
                    LookAtPlayer(playerPos);
                    if (dist < attackRange)
                        currentState = BeeState.Attack;
                    else if (dist > alertRange)
                        currentState = BeeState.Patrol;
                    break;

                case BeeState.Attack:
                    Chase(dt, playerPos);
                    ChooseAnimation(playerPos - position);
                    if (dist > alertRange)
                        currentState = BeeState.Patrol;
                    break;
            }

            anim.updateFrame(dt);
        }

        private void Patrol(float dt)
        {
            Vector2 dir = target - position;

            if (dir.Length() < 5)
            {
                goingToB = !goingToB;
                target = goingToB ? pointB : pointA;
            }
            else
            {
                dir.Normalize();
                position += dir * speed * dt;
            }
        }

        private void LookAtPlayer(Vector2 playerPos)
        {
            ChooseAnimation(playerPos - position);
        }

        private void Chase(float dt, Vector2 playerPos)
        {
            Vector2 dir = playerPos - position;
            dir.Normalize();
            position += dir * (speed * 1.4f) * dt;
        }

        private void ChooseAnimation(Vector2 dir)
        {
            float absX = Math.Abs(dir.X);
            float absY = Math.Abs(dir.Y);

            string newAnim = currentAnimation;

            if (absX > absY)
            {
                if (dir.X > 0)
                    newAnim = animRight;
                else
                    newAnim = animLeft;
            }
            else
            {
                if (dir.Y > 0)
                    newAnim = animBackward;
                else
                    newAnim = animForward;
            }

            // Only reload animation if it changed
            if (newAnim != currentAnimation)
            {
                currentAnimation = newAnim;
                anim.Load(_content, newAnim, 4, 2); // adjust frames as needed
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            anim.DrawFrame(spriteBatch, position);
        }
    }
}
