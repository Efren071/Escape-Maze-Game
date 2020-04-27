using OpenGL_Game.Managers;

namespace OpenGL_Game.Components
{
    class ComponentTexture : IComponent
    {
        int texture;
        string filepath;

        public ComponentTexture(string textureName)
        {
            texture = ResourceManager.LoadTexture(textureName);
            filepath = textureName;
        }

        public int Texture
        {
            get { return texture; }
        }

        public string TexturePath
        {
            get { return filepath; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_TEXTURE; }
        }

        public void Close()
        {
            texture = 0;
        }
    }
}
