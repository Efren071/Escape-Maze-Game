using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Components;
using OpenGL_Game.OBJLoader;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;
using OpenGL_Game.Shaders;

namespace OpenGL_Game.Systems
{
    class SystemRender : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_GEOMETRY);

        public static ShaderUtility mShader;

        protected int uniform_stex;
        protected int uniform_model;
        protected int uniform_viewProjection;

        public SystemRender()
        {
            mShader = new ShaderUtility("shaders/shaders/MainVS.glsl", "shaders/shaders/MainFS.glsl");

            GL.UseProgram(mShader.ShaderProgramID);
            uniform_stex = GL.GetUniformLocation(mShader.ShaderProgramID, "s_texture");
            uniform_model = GL.GetUniformLocation(mShader.ShaderProgramID, "uModel");
            uniform_viewProjection = GL.GetUniformLocation(mShader.ShaderProgramID, "uViewProjection");
        }

        public string Name
        {
            get { return "SystemRender"; }
        }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                // Get entity components
                ComponentPosition positionComponent = entity.GetComponent<ComponentPosition>();
                Matrix4 model = Matrix4.CreateTranslation(positionComponent.Position);          // Create translation matrix for the position of the entity

                ComponentGeometry geometryComponent = entity.GetComponent<ComponentGeometry>();
                Geometry geometry = geometryComponent.Geometry;

                Draw(model, geometry);
            }
        }

        public void Draw(Matrix4 model, Geometry geometry)
        {
            GL.UseProgram(mShader.ShaderProgramID);

            GL.Uniform1(uniform_stex, 0);
            GL.ActiveTexture(TextureUnit.Texture0);

            // Model
            GL.UniformMatrix4(uniform_model, false, ref model);

            Matrix4 modelViewProjection = model * GameScene.gameInstance.camera.view * GameScene.gameInstance.camera.projection;
            GL.UniformMatrix4(uniform_viewProjection, false, ref modelViewProjection);

            geometry.Render();

            GL.UseProgram(0);
        }
    }
}