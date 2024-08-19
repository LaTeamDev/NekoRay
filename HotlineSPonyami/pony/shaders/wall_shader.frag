#version 330

in vec2 fragTexCoord;

// Input uniform values
uniform sampler2D texture0;
uniform sampler2D uvMap;
uniform vec2 mapSize;
uniform int atlasWidth;
const int textureSize = 48;

// Output fragment color
out vec4 finalColor;

void main()
{
    vec4 texelColor = texture2D(uvMap, fragTexCoord);
    
    vec2 globalCoord = fragTexCoord;
    globalCoord.x *= mapSize.x;
    globalCoord.y *= mapSize.y;
    int cellX = int(globalCoord.x); //floor
    int cellY = int(globalCoord.y);
    vec2 localCellCoord = globalCoord - vec2(cellX, cellY);
    localCellCoord.x = clamp(localCellCoord.x, 0.001, 0.999);
    localCellCoord.y = clamp(localCellCoord.y, 0.001, 0.999);
    //localCellCoord.x /= float(atlasWidth / textureSize);
    //localCellCoord.x /= 48;
    //localCellCoord.x *= 32;
    vec2 cellCoord = localCellCoord * 32;
    if(cellCoord.x > 7 && cellCoord.y < (32-7)) discard;

    if(cellCoord.x <= 7 && cellCoord.y >= (32-7) && texelColor.w > 0)
    {
        finalColor = vec4(1, 0, 1, 1);
        return;
    }
    
    if(cellCoord.x <= 7 && texelColor.y > 0)
    {
        localCellCoord /= 48;
        localCellCoord *= 32;
        localCellCoord.x /= float(atlasWidth / textureSize);
        localCellCoord.x += (int(texelColor.y * 256)) * (1.0 / float(atlasWidth / textureSize));
        finalColor = texture2D(texture0, localCellCoord);
        return;
    }
    if(cellCoord.y >= (32-7) && texelColor.z > 0)
    {
        //localCellCoord.y = 1.0 - localCellCoord.y;
        localCellCoord *= 48;
        
        localCellCoord.y += 24;
        localCellCoord.x += 24;
        
        localCellCoord /= 48;
        
        localCellCoord /= 48;
        localCellCoord *= 32;
        localCellCoord.x /= float(atlasWidth / textureSize);
        localCellCoord.x += (int(texelColor.z * 256)) * (1.0 / float(atlasWidth / textureSize));
        finalColor = texture2D(texture0, localCellCoord);
        return;
    }
    discard;

    finalColor = vec4(localCellCoord, 0, 1);
    //finalColor = texture2D(texture0, localCellCoord);
}