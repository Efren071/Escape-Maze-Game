using OpenGL_Game.Managers;
using OpenGL_Game.OBJLoader;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;
using System;

namespace OpenGL_Game.Components
{
    class ComponentGeometry : IComponent
    {
        Geometry geometry;

        public ComponentGeometry(string geometryName)
        {
            this.geometry = ResourceManager.LoadGeometry(geometryName);
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_GEOMETRY; }
        }

        public Geometry Geometry
        {
            get { return geometry; }
            set { this.geometry = value; }
        }

        public void Close() { geometry = null; }
    }
}
