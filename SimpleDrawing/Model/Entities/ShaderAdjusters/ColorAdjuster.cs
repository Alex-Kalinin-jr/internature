
using ImGuiNET;

namespace SimpleDrawing.Model.Entities.ShaderAdjusters {
  public class ColorAdjuster {


    public static void AdjustShader(Color color, Shader shader, int pos) {
      if (color is Color) {

      }
    }

    public static void AdjustShader(DirectionalLightColor dL, ref Shader shader, int i) {
      if (shader.UniformLocations.ContainsKey("dirlights[{i}].direction")) {
        shader.SetUniform3($"dirlights[{i}].direction", dL.Direction);
      }
      if (shader.UniformLocations.ContainsKey("dirlights[{i}].color")) {
        shader.SetUniform3($"dirlights[{i}].color", dL.Ambient);
      }
      if (shader.UniformLocations.ContainsKey("dirlights[{i}].diffuse")) {
        shader.SetUniform3($"dirlights[{i}].diffuse", dL.Diffuse);
      }
      if (shader.UniformLocations.ContainsKey("dirlights[{i}].specular")) {
        shader.SetUniform3($"dirlights[{i}].specular", dL.Specular);
      }
    }

    public static void AdjustShader(PointLightColor color, ref Shader shader, int i) {
      shader.SetUniform3($"pointLights[{i}].color", color.Ambient);
      shader.SetUniform3($"pointLights[{i}].diffuse", color.Diffuse);
      shader.SetUniform3($"pointLights[{i}].specular", color.Specular);

      shader.SetFloat($"pointLights[{i}].constant", color.Constant);
      shader.SetFloat($"pointLights[{i}].linear", color.Linear);
      shader.SetFloat($"pointLights[{i}].quadratic", color.Quadratic);
    }

    public static void Adjustshader(FlashLightColor color, ref Shader shader, int i) {

      if (shader.UniformLocations.ContainsKey($"flashLights[{i}].direction")) {
        shader.SetUniform3($"flashLights[{i}].direction", color.Direction);
      }
      if (shader.UniformLocations.ContainsKey($"flashLights[{i}].color")) {
        shader.SetUniform3($"flashLights[{i}].color", color.Ambient);
      }
      if (shader.UniformLocations.ContainsKey($"flashLights[{i}].diffuse")) {
        shader.SetUniform3($"flashLights[{i}].diffuse", color.Diffuse);
      }
      if (shader.UniformLocations.ContainsKey($"flashLights[{i}].specular")) {
        shader.SetUniform3($"flashLights[{i}].specular", color.Specular);
      }
      if (shader.UniformLocations.ContainsKey($"flashLights[{i}].cutOff")) {
        shader.SetFloat($"flashLights[{i}].cutOff", color.CutOff);
      }
      if (shader.UniformLocations.ContainsKey($"flashLights[{i}].outerCutOff")) {
        shader.SetFloat($"flashLights[{i}].outerCutOff", color.OuterCutOff);
      }
      if (shader.UniformLocations.ContainsKey($"flashLights[{i}].constant")) {
        shader.SetFloat($"flashLights[{i}].constant", color.Constant);
      }
      if (shader.UniformLocations.ContainsKey($"flashLights[{i}].linear")) {
        shader.SetFloat($"flashLights[{i}].linear", color.Linear);
      }
      if (shader.UniformLocations.ContainsKey($"flashLights[{i}].quadratic")) {
        shader.SetFloat($"flashLights[{i}].quadratic", color.Quadratic);
      }
    }

    public static void AdjustShader(MaterialColor color, ref Shader shader, int pos) { }

  }

}
