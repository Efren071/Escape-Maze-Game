using OpenGL_Game.Scenes;
using OpenGL_Game.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace OpenGL_Game.Objects
{
    class SkyBox
    {
        public ShaderUtility skyShader;
        int uniform_vPos;
        int uniform_model;
        int uniform_view;
        int uniform_Projection;
        int uniform_mSamplerCube;

        int VAO;
        int VBO;
        int textID;

        List<string> faces = new List<string>();
        private string path = System.Environment.CurrentDirectory;
        int cubeMapTexture;

        float[] SkyBoxVerts =
            {
            // Skybox Positions
            -1.0f,  1.0f, -1.0f,
        -1.0f, -1.0f, -1.0f,
         1.0f, -1.0f, -1.0f,
         1.0f, -1.0f, -1.0f,
         1.0f,  1.0f, -1.0f,
        -1.0f,  1.0f, -1.0f,

        -1.0f, -1.0f,  1.0f,
        -1.0f, -1.0f, -1.0f,
        -1.0f,  1.0f, -1.0f,
        -1.0f,  1.0f, -1.0f,
        -1.0f,  1.0f,  1.0f,
        -1.0f, -1.0f,  1.0f,

         1.0f, -1.0f, -1.0f,
         1.0f, -1.0f,  1.0f,
         1.0f,  1.0f,  1.0f,
         1.0f,  1.0f,  1.0f,
         1.0f,  1.0f, -1.0f,
         1.0f, -1.0f, -1.0f,

        -1.0f, -1.0f,  1.0f,
        -1.0f,  1.0f,  1.0f,
         1.0f,  1.0f,  1.0f,
         1.0f,  1.0f,  1.0f,
         1.0f, -1.0f,  1.0f,
        -1.0f, -1.0f,  1.0f,

        -1.0f,  1.0f, -1.0f,
         1.0f,  1.0f, -1.0f,
         1.0f,  1.0f,  1.0f,
         1.0f,  1.0f,  1.0f,
        -1.0f,  1.0f,  1.0f,
        -1.0f,  1.0f, -1.0f,

        -1.0f, -1.0f, -1.0f,
        -1.0f, -1.0f,  1.0f,
         1.0f, -1.0f, -1.0f,
         1.0f, -1.0f, -1.0f,
        -1.0f, -1.0f,  1.0f,
         1.0f, -1.0f,  1.0f
            };
        public SkyBox()
        {
            skyShader = new ShaderUtility("Shaders/shaders/SkyboxVS.glsl", "Shaders/shaders/SkyboxFS.glsl");

            GL.UseProgram(skyShader.ShaderProgramID);

            uniform_vPos = GL.GetUniformLocation(skyShader.ShaderProgramID, "a_Position");
            uniform_model = GL.GetUniformLocation(skyShader.ShaderProgramID, "uModel");
            uniform_view = GL.GetUniformLocation(skyShader.ShaderProgramID, "uView");
            uniform_Projection = GL.GetUniformLocation(skyShader.ShaderProgramID, "uProjection");
            uniform_mSamplerCube = GL.GetUniformLocation(skyShader.ShaderProgramID, "skybox");

            GL.GenVertexArrays(1, out VAO);
            GL.GenBuffers(1, out VBO);

            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(SkyBoxVerts.Length * 4), SkyBoxVerts, BufferUsageHint.StaticDraw);

            // position attribute
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * 4, 0);

            GL.BindVertexArray(0);

            Loadfiles();
            cubeMapTexture = LoadTextures(faces);
        }

        private int LoadTextures(List<string> faces)
        {
            GL.GenTextures(1, out textID);
            GL.BindTexture(TextureTarget.TextureCubeMap, textID);

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            //GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            //GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            //GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);

            // Loading textures
            for (int i = 0; i < faces.Count; i++)
            {
                string filename = faces[i];
                Bitmap bmp = new Bitmap(filename);

                BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgb, bmp_data.Width, bmp_data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.UnsignedByte, bmp_data.Scan0);

                bmp.UnlockBits(bmp_data);
            }

            return textID;
        }

        private void Loadfiles()
        {
            path = path.Substring(0, path.IndexOf("bin"));
            path = path.Replace("\\", "/");

            faces.Add(path.Insert(path.Length, "Textures/Sky/right.jpg"));
            faces.Add(path.Insert(path.Length, "Textures/Sky/left.jpg"));
            faces.Add(path.Insert(path.Length, "Textures/Sky/top.jpg"));
            faces.Add(path.Insert(path.Length, "Textures/Sky/bottom.jpg"));
            faces.Add(path.Insert(path.Length, "Textures/Sky/front.jpg"));
            faces.Add(path.Insert(path.Length, "Textures/Sky/back.jpg"));
        }

        public void RenderSkyBox()
        {
            GL.UseProgram(skyShader.ShaderProgramID);
            GL.DepthMask(false);
            GL.DepthFunc(DepthFunction.Lequal); // set depth function so depth test passes when value is equal to 1 as is set in the cubemap shader

            Matrix4 mView = GameScene.gameInstance.camera.view;
            mView = mView.ClearTranslation(); // keep the cube map centered on the player by removing the translation from the view matrix
            GL.UniformMatrix4(uniform_view, false, ref mView);
            //Projection
            Matrix4 mProjection = GameScene.gameInstance.camera.projection;
            GL.UniformMatrix4(uniform_Projection, false, ref mProjection);

            //// Draw skybox cube
            GL.BindVertexArray(VAO);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.TextureCubeMap, cubeMapTexture);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 36);
            GL.BindVertexArray(0);
            GL.DepthFunc(DepthFunction.Less);
            GL.DepthMask(true);
            GL.UseProgram(0);
        }
    }
}