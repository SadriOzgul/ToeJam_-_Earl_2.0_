using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ToeJam___Earl_2._0_
{
    public class EnemyDemon
    {
        public enum State { Patrol, Alert, Attack }
        public State CurrentState = State.Patrol;

        private Vector2 position;
        private Vector2 patrolPointA;
        private Vector2 patrolPointB;
        private Vector2 velocity;

        private float patrolSpeed = 40f;
        private float alertSpeed = 70f;
        private float attackSpeed = 120f;

        private float alertRange = 200f;
        private float attackRange = 90f;

        // Idle animation names per state
        private string idlePatrol = "IdleDemon";
        private string idleAlert = "IdleDemonTwo";
        private string idleAttack = "IdleDemon";

        private AnimatedTexture anim; // From Animated texture scrpit. 
        private string currentAnimKey = ""; // Created as part of preventing animated sprites from reseting.

        public EnemyDemon(Vector2 startPos, Vector2 pointA, Vector2 pointB)
        {
            position = startPos;
            patrolPointA = pointA;
            patrolPointB = pointB;
            velocity = new Vector2(1, 0);

            anim = new AnimatedTexture(Vector2.Zero, new Rectangle(), 0f, 3f, 0.6f);

        }

        public void Load(ContentManager content)
        {
            LoadAnim(content, idlePatrol, 4, 4); // Start idle
        }

        private void LoadAnim(ContentManager content, string name, int frames, int fps)
        {
            if (currentAnimKey == name) return;   // Prevent reload spam

            currentAnimKey = name;
            anim.Load(content, name, frames, fps);
        }

        public void Update(GameTime gameTime, Vector2 playerPos, ContentManager content)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float distToPlayer = Vector2.Distance(position, playerPos);

            // ---------- STATE MACHINE ----------
            switch (CurrentState)
            {
                case State.Patrol:
                    PatrolState(dt, content);
                    if (distToPlayer < alertRange)
                        CurrentState = State.Alert;
                    break;

                case State.Alert:
                    AlertState(dt, playerPos, content);
                    if (distToPlayer < attackRange)
                        CurrentState = State.Attack;
                    else if (distToPlayer > alertRange)
                        CurrentState = State.Patrol;
                    break;

                case State.Attack:
                    AttackState(dt, playerPos, content);
                    if (distToPlayer > attackRange)
                        CurrentState = State.Alert;
                    break;
            }

            anim.updateFrame(dt);
        }

        // ---------------- PATROL ----------------
        private void PatrolState(float dt, ContentManager content)
        {
            Vector2 target = velocity.X > 0 ? patrolPointB : patrolPointA;

            Vector2 dir = target - position;

            // If very close → idle
            if (dir.Length() < 5f)
            {
                velocity *= -1;
                LoadAnim(content, idlePatrol, 3, 4);
                return;
            }

            dir.Normalize();
            position += dir * patrolSpeed * dt;

            PlayDirectionalAnim(dir, content);
        }

        // ---------------- ALERT ----------------
        private void AlertState(float dt, Vector2 playerPos, ContentManager content)
        {
            Vector2 dir = playerPos - position;

            if (dir.Length() < 10f)
            {
                LoadAnim(content, idleAlert, 2, 4);
                return;
            }

            dir.Normalize();
            position += dir * alertSpeed * dt;

            PlayDirectionalAnim(dir, content);
        }

        // ---------------- ATTACK ----------------
        private void AttackState(float dt, Vector2 playerPos, ContentManager content)
        {
            Vector2 dir = playerPos - position;

            if (dir.Length() < 5f)
            {
                LoadAnim(content, idleAttack, 3, 3);
                return;
            }

            dir.Normalize();
            position += dir * attackSpeed * dt;

            PlayDirectionalAnim(dir, content);
        }

        // ---------- DIRECTIONAL MOVEMENT ----------
        private void PlayDirectionalAnim(Vector2 dir, ContentManager content)
        {
            // Horizontal priority
            if (dir.X > 0.4f)
                LoadAnim(content, "RightDemon", 4, 6);
            else if (dir.X < -0.4f)
                LoadAnim(content, "LeftDemon", 4, 6);
            else if (dir.Y < 0)
                LoadAnim(content, "BackDemon", 3, 6);
            else
                LoadAnim(content, "FrontDemon", 3, 6);
        }

        public Rectangle Hitbox
        {
            get
            {
                int hitWidth = 30;   // ✅ SMALL horizontal range
                int hitHeight = 30;  // ✅ SMALL vertical range

                return new Rectangle(
                    (int)position.X - hitWidth / 2,
                    (int)position.Y - hitHeight / 2,
                    hitWidth,
                    hitHeight
                );
            }
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            anim.DrawFrame(spriteBatch, position);
        }
    }
}
