#version 460 core

in vec2 passTextureCoord;
in vec3 passColor;

out vec4 out_Color;

uniform sampler2D textureSampler;;

void main(void)
{
	out_Color = vec4(passColor, texture(textureSampler, passTextureCoord).a);
//	out_Color = texture(textureSampler, passTextureCoord);
}