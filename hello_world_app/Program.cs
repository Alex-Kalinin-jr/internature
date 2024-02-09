using ImGuiNET;


class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        ImGui.GetIO();
        ImGui.CreateContext();
        // ImGui.NewFrame();
        Console.WriteLine("hello");
        //ImGui.ShowDemoWindow();
    }

}