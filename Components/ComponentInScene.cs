﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Components
{
    class ComponentInScene :IComponent
    {
        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_INSCENE; }
        }

        public void Close()
        {
        }
    }
}
