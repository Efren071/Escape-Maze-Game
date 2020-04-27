using OpenTK;

namespace OpenGL_Game.Components
{
    class ComponentLightSource : IComponent
    {
        Vector3 ambient, diffuse, specular, attenuation;
        int pLightIndex;
        
        public ComponentLightSource(Vector3 ambient, Vector3 diffuse, Vector3 specular,
            float constant, float linear, float quadratic)
        {
            this.ambient = ambient;
            this.diffuse = diffuse;
            this.specular = specular;
            this.attenuation = new Vector3(constant, linear, quadratic);

            // Error handler -- Number of Lights exceeded limit
            pLightIndex = Systems.SystemLighting.pLightIndex;
            Systems.SystemLighting.pLightIndex++;

            if (pLightIndex > 20)
            {
                throw new System.Exception("You have exceeded the amount of Point Lights you can make!");
            }
        }

        public ComponentLightSource(Vector3 ambient, Vector3 diffuse, Vector3 specular)
        {
            this.ambient = ambient;
            this.diffuse = diffuse;
            this.specular = specular;
            this.attenuation = new Vector3(10.0f, 5.0f, 20.0f);

            pLightIndex = Systems.SystemLighting.pLightIndex;
            Systems.SystemLighting.pLightIndex++;
            if (pLightIndex > 20)
            {
                // Error handler -- Number of Lights exceeded limit
                throw new System.Exception("You have exceeded the amount of Point Lights you can make!");
            }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_LIGHT_EMITTER; }
        }

        public Vector3 Ambient
        {
            get { return ambient; }
            set { this.ambient = value; }
        }

        public Vector3 Diffuse
        {
            get { return diffuse; }
            set { this.diffuse = value; }
        }

        public Vector3 Specular
        {
            get { return specular; }
            set { this.specular = value; }
        }

        public Vector3 Attenuation
        {
            get { return attenuation; }
            set { this.attenuation = value; }
        }

        public int LightIndex
        {
            get { return pLightIndex; }
        }

        public void Close()
        {
            Systems.SystemLighting.pLightIndex = 0;
            Systems.SystemRender.mShader.Delete();
        }
    }
}
