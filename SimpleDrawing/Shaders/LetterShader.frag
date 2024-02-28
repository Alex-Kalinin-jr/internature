#version 330

out vec4 outputColor;

in vec2 texCoord;

uniform sampler2D texture0;
uniform vec3 color;

void main() {
    vec4 texColor = texture(texture0, texCoord);
    
    float alpha = texColor.r < 0.2 && texColor.g < 0.2 && texColor.b < 0.2 ? 0.0 : 1.0;
    
    outputColor = vec4(color, alpha);
}