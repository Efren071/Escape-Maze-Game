#version 330

layout (location = 0) in vec3 a_Position; 

uniform mat4 uView;
uniform mat4 uProjection;
uniform mat4 uModel;

out vec3 vTexCoord;


void main()
{

	vTexCoord = a_Position;
	vec4 pos = uView * uProjection * vec4(a_Position, 1.0);
	gl_Position = pos.xyww;;

}