#version 330

// Input vertex attributes
in vec3 vertexPosition;
in vec2 vertexTexCoord;
in vec3 vertexNormal;
in vec4 vertexColor;

out vec2 fragTexCoord;

// Input uniform values
uniform mat4 mvp;

void main()
{
    fragTexCoord = vertexTexCoord;
    // Calculate final vertex position
    gl_Position = mvp*vec4(vertexPosition, 1.0);
}