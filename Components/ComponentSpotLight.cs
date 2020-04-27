using OpenGL_Game.Components;
using OpenTK;
using System;
using System.Collections.Generic;

namespace OpenGL_Game.Components
{
    class ComponentSpotLight : IComponent
    {
        Vector3 lightDir, ambient, diffuse, specular;
        float cutOff, outerCutOff;
        int sLightIndex;

        Vector3 attenuation;

        public ComponentSpotLight(Vector3 lightDirection, Vector3 ambient, Vector3 diffuse, Vector3 specular, float cutOff, float outerCutOff)
        {
            this.lightDir = lightDirection;
            this.ambient = ambient;
            this.diffuse = diffuse;
            this.specular = specular;
            this.cutOff = (cutOff);
            this.outerCutOff = (outerCutOff); 
            this.attenuation = new Vector3(10.0f, 5.0f, 20.0f);

            sLightIndex = Systems.SystemLighting.sLightIndex;
            Systems.SystemLighting.sLightIndex++;    // increases the index of spotlight
            if (sLightIndex > 1)
            {
                // Error handler -- Number of Spot Lights exceeded limit
                throw new System.Exception("You have exceeded the amount of spotlights you can make!");
            }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_SPOT_LIGHT;}
        }
        public Vector3 LightDirection
        {
            get { return lightDir; }
            set { lightDir = value; }
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
        public float CutOff
        {
            get { return cutOff; }
            set { cutOff = value; }
        }
        public float OuterCutOff
        {
            get { return outerCutOff; }
            set { outerCutOff = value; }
        }

        public int LightIndex
        {
            get { return sLightIndex; }
            set { this.sLightIndex = value; }
        }

        public Vector3 Attenuation
        {
            get { return attenuation; }
            set { this.attenuation = value; }
        }
        public void Close() { }
    }
}
