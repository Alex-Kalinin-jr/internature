/*
    public override void DirectionalLightAdjustShader(ref Shader shader, int i) {
      base.AdjustShader(ref shader, i);
      shader.SetUniform3($"dirlights[{i}].direction",
          new OpenTK.Mathematics.Vector3(Direction.X, Direction.Y, Direction.Z));
      shader.SetUniform3($"dirlights[{i}].color",
          new OpenTK.Mathematics.Vector3(Color.X, Color.Y, Color.Z));
      shader.SetUniform3($"dirlights[{i}].diffuse",
          new OpenTK.Mathematics.Vector3(Diffuse.X, Diffuse.Y, Diffuse.Z));
      shader.SetUniform3($"dirlights[{i}].specular",
          new OpenTK.Mathematics.Vector3(Specular.X, Specular.Y, Specular.Z));
    }
 
 
 
 
 */