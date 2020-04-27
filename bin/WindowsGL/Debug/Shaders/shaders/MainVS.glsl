#version 330

layout (location = 0) in vec3 a_Position;
layout (location = 1) in vec2 a_TexCoord;
layout (location = 2) in vec3 a_Normal;

uniform mat4 uModel;
uniform mat4 uViewProjection;

out vec2 vTexCoord;
out vec3 vNormal;
out vec3 vFragPos;

void main()
{
	gl_Position = uViewProjection * vec4(a_Position, 1.0);
	vFragPos = vec3(uModel * vec4(a_Position, 1.0));
	vNormal = a_Normal;
	vTexCoord = a_TexCoord;
}