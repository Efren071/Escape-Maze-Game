using System.Collections.Generic;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;
using OpenGL_Game.Managers;


namespace OpenGL_Game.Systems
{
    class SystemCollisionSphere : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_SPHERE_SPHERE);

        // Set camera variables
        Camera camera;
        CollisionManager collisionManager;

        public SystemCollisionSphere(CollisionManager manager, Camera camera)
        {
            this.camera = camera;
            this.collisionManager = manager;
        }

        public string Name
        {
            get { return "SystemCameraSphere"; }
        }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                // Position
                IComponent positionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                ComponentPosition position = (ComponentPosition)positionComponent;

                // Collision
                IComponent collisionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_SPHERE_SPHERE;
                });
                ComponentCollisionSphere sphereColl = (ComponentCollisionSphere)collisionComponent;

                SphereCollision(entity, position, sphereColl);
            }
        }

        // Collision Method
        private void SphereCollision(Entity entity,
                                     ComponentPosition position,
                                     ComponentCollisionSphere collision)
        {
            if ((position.Position - camera.cameraPosition).Length < collision.Radius + camera.cameraRadius && entity.name != "Player")
            {
                collisionManager.CollisionBetweenCamera(entity, COLLISIONTYPE.SPHERE_SPHERE);
                collisionManager.ProcessCollisions(entity);
            }
        }
    }
}
