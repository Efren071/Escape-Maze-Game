using OpenTK;
using OpenGL_Game.Scenes;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Components
{
    class ComponentPosition : IComponent
    {
        public Vector3 position;

        public ComponentPosition(float x, float y, float z)
        {
            position = new Vector3(x, y, z);
        }

        public ComponentPosition(Vector3 pos)
        {
            position = pos;
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_POSITION; }
        }

        public void Close()
        {
            position = new Vector3(0, 0, 0);
        }
    }
}
