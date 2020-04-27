using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenGL_Game.Components;
using OpenGL_Game.Managers;
using OpenTK;

namespace OpenGL_Game.Objects
{
    public enum OBJECT_TYPE
    {
        PLAYER,
        ENEMY,
        KEY,
        PORTAL,
        ENVIRONMENT,
        FLOOR,
        ROLLINGBALL,
        BOUNCINGBALL,
        LIGHTEMITTER,
        SPOTLIGHT
    }
    class Entity
    {
        public List<IComponent> componentList = new List<IComponent>();
        public ComponentTypes mask;

        public string name;
        public OBJECT_TYPE objectType;
        public Entity(string name, OBJECT_TYPE objectType )
        {
            this.name = name;
            this.objectType = objectType;
        }
        
        //Adds a single component</summary>
        public void AddComponent(IComponent component)
        {
            Debug.Assert(component != null, "Component cannot be null");

            componentList.Add(component);
            mask |= component.ComponentType;
        }

        public void Delete()
        {
            mask &= ~ComponentTypes.COMPONENT_INSCENE;
        }

        public String Name
        {
            get { return name; }
        }

        public ComponentTypes Mask
        {
            get { return mask; }
        }

        public List<IComponent> Components
        {
            get { return componentList; }
        }

        // returns a Component <T> == General Type
        public T GetComponent<T>()
        {
            Type typeparam = typeof(T);

            foreach (IComponent component in componentList)
            {
                if (component.GetType() == typeparam)
                    return (T)Convert.ChangeType(component, typeparam);
            }
            return default(T);
        }

    }
}
