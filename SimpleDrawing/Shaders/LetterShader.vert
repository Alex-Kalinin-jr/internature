﻿#version 330



layout(location = 0) in vec3 aPosition;


void main(void)
{
    gl_Position = vec4(aPosition, 1.0);
}