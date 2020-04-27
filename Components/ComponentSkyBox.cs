using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL_Game.Components
{
    class ComponentSkyBox : IComponent
    {
        public float[] vertices;
         
        public ComponentSkyBox()
        {
            vertices = new float[] {
                
            };
        }

        public float[] GetVertices
        {
            get { return vertices; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_SKYBOX; }
        }

        public void Close()
        {
        }
    }
}
