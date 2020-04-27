using OpenTK;
using OpenGL_Game.Scenes;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Components
{
    class ComponentRotation : IComponent
    {
        Vector3 rotation;

        public ComponentRotation(float x, float y, float z)
        {
            this.rotation.X = MathHelper.DegreesToRadians(x);
            this.rotation.Y = MathHelper.DegreesToRadians(y);
            this.rotation.Z = MathHelper.DegreesToRadians(z);
        }

        public ComponentRotation(Vector3 rot)
        {
            this.rotation.X = MathHelper.DegreesToRadians(rot.X);
            this.rotation.Y = MathHelper.DegreesToRadians(rot.Y);
            this.rotation.Z = MathHelper.DegreesToRadians(rot.Z);
        }

        public Vector3 Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_POSITION; }
        }

        public void Close()
        {
            rotation = new Vector3(0, 0, 0);
        }
    }
}
