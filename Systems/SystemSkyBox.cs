using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Components;
using OpenGL_Game.OBJLoader;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;
using System.Drawing;
using System.Drawing.Imaging;

namespace OpenGL_Game.Systems
{
    class SystemSkyBox : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_TEXTURE | ComponentTypes.COMPONENT_SKYBOX);

        protected int pgmID;
        protected int vsID;
        protected int fsID;
        protected int uniform_mProjection;
        protected int uniform_mView;
        protected int uniform_mSamplerCube;

        List<string> boxFaces = new List<string> { };
        protected string filePath = System.Environment.CurrentDirectory; // gets the current directory
        
        int skyboxVAO, skyboxVBO;
        int textureID;

        public string Name
        {
            get { return "System Sky Box"; }
        }

        public SystemSkyBox(Entity entity)
        {
            ComponentSkyBox sky = entity.GetComponent<ComponentSkyBox>();
            ComponentTexture cubemap = entity.GetComponent<ComponentTexture>();
            SetupBoxFaces();
            CreateShaders();
            GenerateDataBuffers(sky);
            LoadCubeMapTexture(cubemap);
        }

        public void OnAction(Entity entity)
        {
            if((entity.Mask & MASK) == MASK)
            {
            }
        }


        /// <summary>
        /// Loads Shaders
        /// </summary>
        private void CreateShaders()
        {
            pgmID = GL.CreateProgram();
            LoadShader("Shaders/Skybox/skybox_FS.glsl", ShaderType.FragmentShader, pgmID, out fsID);
            LoadShader("Shaders/Skybox/skybox_VS.glsl", ShaderType.VertexShader, pgmID, out vsID);
            GL.LinkProgram(pgmID);
            Console.WriteLine(GL.GetProgramInfoLog(pgmID));

            uniform_mProjection = GL.GetUniformLocation(pgmID, "projection");
            uniform_mView = GL.GetUniformLocation(pgmID, "view");
            uniform_mSamplerCube = GL.GetUniformLocation(pgmID, "skybox");
        }

        /// <summary>
        /// Generates data buffers and binds data to vertices
        /// </summary>
        /// <param name="sky"></param>
        public void GenerateDataBuffers(ComponentSkyBox sky)
        {
            GL.GenVertexArrays(1, out skyboxVAO);
            GL.GenBuffers(1, out skyboxVBO);
            // binders
            GL.BindVertexArray(skyboxVAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, skyboxVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(sky.GetVertices.Length * sizeof(float)), sky.GetVertices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            GL.BindVertexArray(0);
        }

        /// <summary>
        /// Loads cubemap texture as TextureTarget == CubeMap
        /// </summary>
        /// <param name="cubemap"></param>
        private void LoadCubeMapTexture(ComponentTexture cubemap)
        {
            GL.GenTextures(1, out textureID);
            GL.BindTexture(TextureTarget.TextureCubeMap, textureID);

            // Setting texture parameters

            // We will not upload mipmaps, so disable mipmapping (otherwise the texture will not appear).
            // We can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
            // mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            // Loading textures
            for (int i = 0; i < boxFaces.Count; i++)
            {
                string filename = boxFaces[i];
                Bitmap bmp = new Bitmap(filename);

                BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgb, bmp_data.Width, bmp_data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.UnsignedByte, bmp_data.Scan0);

                bmp.UnlockBits(bmp_data);
            }
        }

        /// <summary>
        /// Parses shader file and attaches to program
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="type"></param>
        /// <param name="program"></param>
        /// <param name="address"></param>
        private void LoadShader(String filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }

        private void SetupBoxFaces()
        {
            
            filePath = filePath.Substring(0, filePath.IndexOf("bin"));
            filePath = filePath.Replace("\\", "/");

            boxFaces.Add(filePath.Insert(filePath.Length,"Textures/Sky/front.jpg"));
            boxFaces.Add(filePath.Insert(filePath.Length, "Textures/Sky/back.jpg"));
            boxFaces.Add(filePath.Insert(filePath.Length, "Textures/Sky/left.jpg"));
            boxFaces.Add(filePath.Insert(filePath.Length, "Textures/Sky/right.jpg"));
            boxFaces.Add(filePath.Insert(filePath.Length, "Textures/Sky/top.jpg"));
            boxFaces.Add(filePath.Insert(filePath.Length, "Textures/Sky/bottom.jpg"));
        }

        //public void Render()
        //{
        //    GL.UseProgram(pgmID);

        //    GL.DepthFunc(DepthFunction.Lequal); // set depth function so depth test passes when value is equal to 1 as is set in the cubemap shader

        //    // Binding sampler cube
        //    GL.Uniform1(uniform_mSamplerCube, 0); // possibly incorrect methid for uniforms of type samplerCube
        //    GL.ActiveTexture(TextureUnit.Texture0);
        //    GL.BindTexture(TextureTarget.TextureCubeMap, textureID);
        //    GL.Enable(EnableCap.TextureCubeMap);
        //    //GL.Enable(EnableCap.Texture2D);

        //    // Setting the view and projection matrices
        //    // View
        //    Matrix4 mView = GameScene.gameInstance.camera.getViewMatrix();
        //    mView = mView.ClearTranslation(); // keep the cube map centered on the player by removing the translation from the view matrix
        //    GL.UniformMatrix4(uniform_mView, false, ref mView);
        //    //Projection
        //    Matrix4 mProjection = GameScene.gameInstance.projection;
        //    GL.UniformMatrix4(uniform_mProjection, false, ref mProjection);

        //    GL.BindVertexArray(pgmID);
        //    GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

        //    GL.DepthFunc(DepthFunction.Less);
        //    GL.BindVertexArray(0);
        //    GL.UseProgram(0);
        //}
    }
}
