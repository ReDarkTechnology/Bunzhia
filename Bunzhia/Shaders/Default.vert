#version 330 core
layout (location = 0) in vec3 aPosition; // Vertex coordinates
layout (location = 1) in vec2 aTexCoord; // Texture coordinates

out vec2 texCoord;

// Uniform variables
// Matrix = Matrix4x4
uniform mat4 model; 
uniform mat4 view;
uniform mat4 projection;

void main()
{
	// Coordinates, multiplication for matrix has to be in this order
	gl_Position = vec4(aPosition, 1.0) * model * view * projection;
	texCoord = aTexCoord;
}