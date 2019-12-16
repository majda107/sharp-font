#version 460 core

in vec2 position;
in vec2 textureCoord;

out vec2 passTextureCoord;
out vec3 passColor;

uniform mat4 projectionMatrix;
uniform mat4 modelMatrix;

void main(void)
{
//	gl_Position = projectionMatrix * vec4(position.x - 700, position.y, -800.0, 1.0);
	vec4 worldPosition = modelMatrix * vec4(position.x, position.y, 0, 1.0); 
	gl_Position = projectionMatrix * worldPosition;
	
	passTextureCoord = textureCoord;
	
	passColor = vec3(position.x, 0.4, 0.6);
}