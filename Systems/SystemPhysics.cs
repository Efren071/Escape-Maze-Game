using System.Collections.Generic;
using OpenGL_Game.Components;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;
using OpenTK;

namespace OpenGL_Game.Systems
{
    class SystemPhysics : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_VELOCITY);

        PhysicsManager physicsManager;

        public string Name
        {
            get { return "System Physics"; }
        }

        public SystemPhysics(PhysicsManager manager)
        {
            this.physicsManager = manager;
        }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                // Grab Components of entity
                ComponentPosition position = entity.GetComponent<ComponentPosition>();
                ComponentVelocity velocity = entity.GetComponent<ComponentVelocity>();

                HandleMotion(entity, position, velocity);
            }
        }

        private void HandleMotion(Entity e, ComponentPosition pos, ComponentVelocity vel)
        {
            float rad = e.GetComponent<ComponentCollisionSphere>().Radius;
            ComponentAudio audio = e.GetComponent<ComponentAudio>();

            switch (e.objectType) {
                case OBJECT_TYPE.BOUNCINGBALL:
                    physicsManager.BounceBall(pos, vel, audio, rad);
                    break;
                case OBJECT_TYPE.ROLLINGBALL:
                    physicsManager.RollBall(pos, vel, rad);
                    break;
                case OBJECT_TYPE.KEY:
                    physicsManager.Float(pos, vel);
                    break;
                default:
                    break;
            }
        }
    }
}