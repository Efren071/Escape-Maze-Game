using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenGL_Game.Objects;
using OpenGL_Game.Components;

namespace OpenGL_Game.Components
{
    class ComponentCollisionSphere : IComponent
    {
        float radius;

        public ComponentCollisionSphere(float rad)
        {
            radius = rad;
        }

        public float Radius
        {
            get { return radius; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_SPHERE_SPHERE; }
        }

        public void Close()
        {
            radius = 0;
        }
    }
}
