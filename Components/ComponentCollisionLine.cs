
using OpenGL_Game.Objects;
using OpenTK;
using System.Collections.Generic;

namespace OpenGL_Game.Components
{
    class ComponentCollisionLine : IComponent
    {
        List<WallBoundaries> boundaries;
        public ComponentCollisionLine(List<WallBoundaries> boundaries)
        {
            this.boundaries = boundaries;
        }

        public List<WallBoundaries> Boundaries
        {
            get { return boundaries; }
            set { boundaries = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_LINE_COLLISION; }
        }

        public void Close() { }
    }
}
