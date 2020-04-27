using System.Collections.Generic;
using OpenGL_Game.Systems;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Managers
{
    class SystemManager
    {
        List<ISystem> systemList = new List<ISystem>();

        public SystemManager()
        {
        }

        public void ActionSystems(EntityManager entityManager)
        {
            List<Entity> entityList = entityManager.Entities;
            foreach(ISystem system in systemList)
            {
                foreach(Entity entity in entityList)
                {
                    // Only update the entities if they are in the scene
                    if ((entity.Mask & Components.ComponentTypes.COMPONENT_INSCENE) == Components.ComponentTypes.COMPONENT_INSCENE)
                    {
                        system.OnAction(entity);
                    }
                }
            }
        }

        public void AddSystem(ISystem system)
        {
            ISystem result = FindSystem(system.Name);
            //Debug.Assert(result != null, "System '" + system.Name + "' already exists");
            systemList.Add(system);
        }

        private ISystem FindSystem(string name)
        {
            return systemList.Find(delegate(ISystem system)
            {
                return system.Name == name;
            }
            );
        }
    }
}
