using System.Collections.Generic;
using OpenGL_Game.Objects;
using System.Diagnostics;
using OpenGL_Game.Components;

namespace OpenGL_Game.Managers
{
    class EntityManager
    {
        public List<Entity> entityList;

        public EntityManager()
        {
            entityList = new List<Entity>();
        }

        public void AddEntity(Entity entity)
        {
            Entity result = FindEntity(entity.Name);
            Debug.Assert(result == null, "Entity '" + entity.Name + "' already exists");
            entityList.Add(entity);
        }

        private Entity FindEntity(string name)
        {
            return entityList.Find(delegate(Entity e)
            {
                return e.Name == name;
            }
            );
        }

        public void Close()
        {
            foreach (Entity entity in entityList)
            {
                foreach (IComponent component in entity.componentList)
                {
                    component.Close();
                }
                entity.Delete();
            }
        }

        public List<Entity> Entities
        {
            get { return entityList; }
        }


    }
}
