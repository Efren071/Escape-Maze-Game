using System;

namespace OpenGL_Game.Components
{
    [FlagsAttribute]
    enum ComponentTypes {
        COMPONENT_NONE     = 0,
	    COMPONENT_POSITION = 1 << 0,
        COMPONENT_GEOMETRY = 1 << 1,
        COMPONENT_ROTATION = 1 << 2,
        COMPONENT_TEXTURE  = 1 << 3,
        COMPONENT_VELOCITY = 1 << 4,
        COMPONENT_AUDIO    = 1 << 5,
        COMPONENT_INSCENE  = 1 << 6,
        COMPONENT_SPHERE_SPHERE = 1 << 7,
        COMPONENT_LINE_COLLISION = 1 << 8,
        COMPONENT_SKYBOX        = 1 << 9,
        COMPONENT_LIGHT_EMITTER = 1 << 10,
        COMPONENT_SPOT_LIGHT         = 1 << 11
    }

    interface IComponent
    {
        ComponentTypes ComponentType
        {
            get;
        }

        void Close();
    }
}
