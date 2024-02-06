class Program
{
    static void Main()
    {
        System.Console.WriteLine("Initializing ImGui.NET...");
        System.Console.WriteLine("ImGui.NET initialized. Running ImGui demo...");
        System.Console.WriteLine("ImGui demo complete.");
        test_lib.Class1 class1 = new test_lib.Class1();
        int a;
        int b;
        class1.GetMembers(out a, out b);
        System.Console.WriteLine($"this is a test {a} and test {b}");
    }
}