using OpenGL_Game.Components;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Systems
{
    class SystemCollisionLine : ISystem
    {
        const ComponentTypes Mask = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_LINE_COLLISION);

        public Camera camera;
        CollisionManager collisionManager;
        public string Name
        {
            get { return "System Collision Line"; }
        }

        public SystemCollisionLine(CollisionManager manager, Camera camera)
        {
            this.camera = camera;
            this.collisionManager = manager;
        }


        public void OnAction(Entity entity)
        {
            if ((entity.mask& Mask) == Mask)
            {
                ComponentCollisionLine lineCollisionComponent = entity.GetComponent<ComponentCollisionLine>();

                Collision(entity, lineCollisionComponent);
            }
        }

        private void Collision(Entity entity, ComponentCollisionLine walls)
        {
            // loop through each boundary line
            foreach (var boundary in walls.Boundaries)
            {
                // Get rounded whole number of camera position
                float playerX = (float)Math.Round(camera.cameraPosition.X, 1);
                float playerZ = (float)Math.Round(camera.cameraPosition.Z, 1);

                // if player/camera position exceeds wall bounds, process wall collision
                if (boundary.point1.X >= playerX && boundary.point2.X <= playerX
                && boundary.point1.Z >= playerZ && boundary.point2.Z <= playerZ)
                {
                    collisionManager.ProcessCollisions(entity);
                }
            }
        }
    }
}
