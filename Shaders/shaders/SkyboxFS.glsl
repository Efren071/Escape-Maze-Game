#version 330
out vec4 Color;

in vec3 vTexCoord;

uniform samplerCube skybox;

void main() 
{
	Color = texture(skybox, vTexCoord);
}