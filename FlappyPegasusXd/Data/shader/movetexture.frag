#version 330

// Input vertex attributes (from vertex shader)
in vec2 fragTexCoord;
in vec4 fragColor;

// Input uniform values
uniform sampler2D texture0;
uniform vec4 colDiffuse;

// Output fragment color
out vec4 finalColor;

uniform float time;
uniform vec2 direction;
uniform float speed;

void main() {
    ivec2 s = textureSize(texture0, 0);
    vec2 p = fract(((fragTexCoord*s)-direction*time*speed)/s) ;
    //finalColor = vec4( p, 0f, 1f); 
    finalColor = texture(texture0, p);
    
    //finalColor = vec4(speed, 0, 0, 1);
}