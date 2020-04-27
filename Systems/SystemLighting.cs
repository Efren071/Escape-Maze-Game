using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using OpenTK.Graphics.OpenGL;

namespace OpenGL_Game.Systems
{
    class SystemLighting : ISystem
    {
        const ComponentTypes POINTLIGHT_MASK = (ComponentTypes.COMPONENT_LIGHT_EMITTER);
        const ComponentTypes SPOTLIGHT_MASK = (ComponentTypes.COMPONENT_SPOT_LIGHT);

        public static int pLightIndex, sLightIndex;   // used to increment the light index when we create a new light source;
        public SystemLighting()
        {
        }
        public string Name
        {
            get { return "System Light"; }
        }
        public void OnAction(Entity entity)
        { 
            // Check if the entity is POINT LIGHT OR SPOTLIGHT then process lighting appropriately
            // Spotlight
            if ((entity.Mask & POINTLIGHT_MASK) == POINTLIGHT_MASK)
            {
                ComponentPosition componentPosition = entity.GetComponent<ComponentPosition>();
                ComponentLightSource lightSourceComponent = entity.GetComponent<ComponentLightSource>(); 

                SetPointLight(lightSourceComponent, componentPosition);
            }

            if ((entity.Mask & SPOTLIGHT_MASK) == SPOTLIGHT_MASK)
            {
                ComponentPosition componentPosition = entity.GetComponent<ComponentPosition>();
                ComponentSpotLight spotLightComponent = entity.GetComponent<ComponentSpotLight>();      

                SetSpotLight(spotLightComponent, componentPosition);
            }
        }

        private void SetSpotLight(ComponentSpotLight spotLight, ComponentPosition ePosition)
        {
            // Grab ShaderID & Use program
            int index = spotLight.LightIndex;
            GL.UseProgram(SystemRender.mShader.ShaderProgramID);
            GL.Uniform3(GL.GetUniformLocation(SystemRender.mShader.ShaderProgramID, $"spotLight[{index}].position"), ePosition.Position);
            GL.Uniform3(GL.GetUniformLocation(SystemRender.mShader.ShaderProgramID, $"spotLight[{index}].direction"), spotLight.LightDirection);
            GL.Uniform3(GL.GetUniformLocation(SystemRender.mShader.ShaderProgramID, $"spotLight[{index}].ambient"), spotLight.Ambient);
            GL.Uniform3(GL.GetUniformLocation(SystemRender.mShader.ShaderProgramID, $"spotLight[{index}].diffuse"), spotLight.Diffuse);
            GL.Uniform3(GL.GetUniformLocation(SystemRender.mShader.ShaderProgramID, $"spotLight[{index}].specular"), spotLight.Specular);
            GL.Uniform1(GL.GetUniformLocation(SystemRender.mShader.ShaderProgramID, $"spotLight[{index}].cutOff"), spotLight.CutOff);
            GL.Uniform1(GL.GetUniformLocation(SystemRender.mShader.ShaderProgramID, $"spotLight[{index}].outerCutOff"), spotLight.OuterCutOff);

            GL.Uniform1(GL.GetUniformLocation(SystemRender.mShader.ShaderProgramID, $"pointLight[{index}].constant"), spotLight.Attenuation.X);
            GL.Uniform1(GL.GetUniformLocation(SystemRender.mShader.ShaderProgramID, $"pointLight[{index}].linear"), spotLight.Attenuation.Y);
            GL.Uniform1(GL.GetUniformLocation(SystemRender.mShader.ShaderProgramID, $"pointLight[{index}].quadratic"), spotLight.Attenuation.Z);

            GL.Uniform1(GL.GetUniformLocation(SystemRender.mShader.ShaderProgramID, "material.shininess"), 32.0f);

            GL.UseProgram(0);
        }

        private void SetPointLight(ComponentLightSource light, ComponentPosition ePosition)
        {
            // Grab ShaderID & Use program
            int index = light.LightIndex;
            GL.UseProgram(SystemRender.mShader.ShaderProgramID);

            GL.Uniform3(GL.GetUniformLocation(SystemRender.mShader.ShaderProgramID, $"pointLight[{index}].position"), ePosition.Position);
            GL.Uniform3(GL.GetUniformLocation(SystemRender.mShader.ShaderProgramID, $"pointLight[{index}].ambient"), light.Ambient);
            GL.Uniform3(GL.GetUniformLocation(SystemRender.mShader.ShaderProgramID, $"pointLight[{index}].diffuse"), light.Diffuse);
            GL.Uniform3(GL.GetUniformLocation(SystemRender.mShader.ShaderProgramID, $"pointLight[{index}].specular"), light.Specular);

            GL.Uniform1(GL.GetUniformLocation(SystemRender.mShader.ShaderProgramID, $"pointLight[{index}].constant"), light.Attenuation.X);
            GL.Uniform1(GL.GetUniformLocation(SystemRender.mShader.ShaderProgramID, $"pointLight[{index}].linear"), light.Attenuation.Y);
            GL.Uniform1(GL.GetUniformLocation(SystemRender.mShader.ShaderProgramID, $"pointLight[{index}].quadratic"), light.Attenuation.Z);

            GL.Uniform1(GL.GetUniformLocation(SystemRender.mShader.ShaderProgramID, "material.shininess"), 128.0f);

            GL.UseProgram(0);
        }
    }
}
