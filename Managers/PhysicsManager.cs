using OpenGL_Game.Components;
using OpenGL_Game.Scenes;
using OpenGL_Game.Objects;
using OpenTK;
using OpenTK.Graphics.ES10;

namespace OpenGL_Game.Managers
{
    class PhysicsManager
    {
        public void Float(ComponentPosition pos, ComponentVelocity vel)
        {
            if (pos.Position.Y >= 1.0f || pos.Position.Y <= 0.5f)
            {
                vel.velocity.Y = -vel.velocity.Y;
            }
            pos.Position += vel.Velocity *  GameScene.dt;
        }

        public void RollBall(ComponentPosition pos, ComponentVelocity vel, float rad)
        {
            float right = 29.5f, left = 14.0f, top = -29.5f, bot = -14.5f;

            BoundryBasedCollision(pos, vel, rad, right, left, top, bot);
            pos.Position += vel.Velocity * GameScene.dt;
        }

        public void BounceBall(ComponentPosition pos, ComponentVelocity vel, ComponentAudio audio, float rad)
        {
            pos.Position += vel.Velocity * GameScene.dt;

            BoundryBasedCollision(pos, vel, rad, 29.5f, 14.0f, 14.5f, 29.5f);
            vel.velocity.Y -= vel.mass;

            if (pos.position.Y <= 0 )
            {
                vel.velocity.Y = -vel.velocity.Y;
            }
        }

        private static void BoundryBasedCollision(ComponentPosition pos, ComponentVelocity vel, float rad, float right, float left, float top, float bot)
        {
            // Check x & z axis boundaries
            // Reverse velocity when hitting a position boundary
            if (pos.Position.X + rad >= right || pos.Position.X - rad <= left)
            {
                vel.velocity.X = -vel.Velocity.X;
            }
            if (pos.Position.Z - rad <= top || pos.Position.Z + rad >= bot)
            {
                vel.velocity.Z = -vel.Velocity.Z;
            }
        }
    }
}
