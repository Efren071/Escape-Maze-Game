using OpenTK;

namespace OpenGL_Game.Components
{
    class ComponentVelocity : IComponent
    {
        public Vector3 velocity;
        public float mass;

        public ComponentVelocity(float x, float y, float z)
        {
            velocity = new Vector3(x, y, z);
        }

        public ComponentVelocity(Vector3 vel)
        {
            velocity = vel;
        }
        public ComponentVelocity(Vector3 vel, float pMass)
        {
            velocity = vel;
            mass = pMass;
        }

        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_VELOCITY; }
        }

        public void Close()
        {
            velocity = new Vector3(0, 0, 0);
        }
    }
}
