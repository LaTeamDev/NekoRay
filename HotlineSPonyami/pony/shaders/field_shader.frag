#version 330

in vec2 fragTexCoord;

// Input uniform values
uniform sampler2D texture0;
uniform sampler2D uvMap;
uniform vec2 mapSize;
uniform int atlasWidth;
const int textureSize = 32;

// Output fragment color
out vec4 finalColor;

void main()
{
    vec4 texelColor = texture2D(uvMap, fragTexCoord);
    if(texelColor.x == 0) discard;
    
    vec2 globalCoord = fragTexCoord;
    globalCoord.x *= mapSize.x;
    globalCoord.y *= mapSize.y;
    int cellX = int(globalCoord.x); //floor
    int cellY = int(globalCoord.y);
    vec2 localCellCoord = globalCoord - vec2(cellX, cellY);
    localCellCoord.x = clamp(localCellCoord.x, 0.001, 0.999);
    localCellCoord.y = clamp(localCellCoord.y, 0.001, 0.999);
    localCellCoord.x /= float(atlasWidth / textureSize);
    localCellCoord.x += (int(texelColor.x * 256)) * (1.0 / float(atlasWidth / textureSize));
    
    
    //finalColor = vec4(localCellCoord, 0, 1);
    finalColor = texture2D(texture0, localCellCoord);
}