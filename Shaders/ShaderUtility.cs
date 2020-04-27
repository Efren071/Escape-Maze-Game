using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace OpenGL_Game.Shaders
{
    public class ShaderUtility
    {
        public int ShaderProgramID { get; set; }
        public int VertexShaderID { get; set; }
        public int FragmentShaderID { get; set; }

        // Finds shader files and compiles them
        public ShaderUtility(string vertexShader, string fragShader)
        {
            StreamReader reader;
            VertexShaderID = GL.CreateShader(ShaderType.VertexShader);
            reader = new StreamReader(vertexShader);
            GL.ShaderSource(VertexShaderID, reader.ReadToEnd());
            reader.Close();
            GL.CompileShader(VertexShaderID);

            int result;
            GL.GetShader(VertexShaderID, ShaderParameter.CompileStatus, out result);
            if (result == 0)
            {
                throw new Exception("Failed to compile vertex shader!" + GL.GetShaderInfoLog(VertexShaderID));
            }

            FragmentShaderID = GL.CreateShader(ShaderType.FragmentShader);
            reader = new StreamReader(fragShader);
            GL.ShaderSource(FragmentShaderID, reader.ReadToEnd());
            reader.Close();
            GL.CompileShader(FragmentShaderID);

            GL.GetShader(FragmentShaderID, ShaderParameter.CompileStatus, out result);
            if (result == 0)
            {
                throw new Exception("Failed to compile fragment shader!" + GL.GetShaderInfoLog(FragmentShaderID));
            }

            // Create shader program and sets
            ShaderProgramID = GL.CreateProgram();
            GL.AttachShader(ShaderProgramID, VertexShaderID);
            GL.AttachShader(ShaderProgramID, FragmentShaderID);
            GL.LinkProgram(ShaderProgramID);
        }

        public void Delete()
        {
            GL.DetachShader(ShaderProgramID, VertexShaderID);
            GL.DetachShader(ShaderProgramID, FragmentShaderID);
            GL.DeleteShader(VertexShaderID);
            GL.DeleteShader(FragmentShaderID);
            GL.DeleteProgram(ShaderProgramID);
        }
    }
}
