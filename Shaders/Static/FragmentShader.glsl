#version 460 core

in vec2 passTextureCoord;
in vec3 passColor;

out vec4 out_Color;

uniform sampler2D textureSampler;

float width = 0.475;
float edge = 0.015;

void main(void)
{
	float dist = 1 - texture(textureSampler, passTextureCoord).a;

	float alpha = 1 - smoothstep(width, width + edge, dist);

	out_Color = vec4(passColor, alpha);

//	out_Color = vec4(passColor, texture(textureSampler, passTextureCoord).a);
//	out_Color = texture(textureSampler, passTextureCoord);
}