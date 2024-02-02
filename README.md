# internature
Repository for the process of internature.

## conclusion

- Базовое задание можно написать в Debian
- основное задание нужно реализовывать в Windows

## understanding

- У меня вопрос по структуре заданий. Правильно я понимаю, что базовое задание (включает 4 задачи) пишется на стеке OpenGL, Imgui.NET; а основное задание (включает 5 задач) пишется на SharpDX, WinForms?
Из файла заданий не очень понятно.
- Да, всё верно.<br><br>
- i want to make gui app on c# and on debian. can i do this?
- Yes, you can create a GUI app using C# on Debian. One way to do this is by using the Mono framework, which is compatible with C# and runs on Debian and other Linux distributions. You can use the GTK# toolkit for building cross-platform GUI applications in C#.
- what is OpenTK?
- OpenTK is a cross-platform, open-source library that provides C# bindings for OpenGL, OpenAL, and OpenCL. It allows developers to create high-performance, cross-platform graphics, audio, and parallel computing applications. OpenTK is commonly used for game development and other graphics-intensive applications.
- can i use OpenTK developing on debian using C#?
- Yes, you can use OpenTK for developing graphics applications in C# on Debian. OpenTK is compatible with the Mono framework, which allows you to develop and run C# applications on Debian and other Linux distributions.
- what is ImGui?
- Dear ImGui (ImGui) is a bloat-free graphical user interface library for C++. It outputs vertex buffers that you can render in your own engine. It is fast, portable, renderer agnostic and self-contained (no external dependencies).
- can i use ImGui on Debian?
- Yes, you can use ImGui on Debian. ImGui is designed to be portable and can be used on various platforms, including Debian and other Linux distributions. As long as you have a compatible C++ development environment set up on Debian, you should be able to use ImGui for your graphical user interface needs.
- but i am going to develop on C#.
- I see, since ImGui is a C++ library, you won't be able to directly use it in a C# application. However, you can consider using other GUI libraries that are compatible with C# and Debian, such as GTK# or WinForms with Mono.
- what is WPF?
- WPF, or Windows Presentation Foundation, is a graphical subsystem for rendering user interfaces in Windows-based applications. It is a part of the .NET framework and provides a unified programming model for building rich, modern desktop applications. WPF applications can be created using XAML for defining the layout and C# for the application logic.
- can i use SharpDX while writing my c# app. i have Debian OS installed and i am going to write app on it?
- SharpDX is a managed wrapper for the DirectX API, and it is primarily targeted for Windows platforms. Since you are using Debian OS, which is a Linux distribution, you may encounter compatibility issues. It's recommended to use cross-platform libraries like OpenGL or Vulkan for graphics programming on Linux.
- and answer to this question regarding to WinForms
- For GUI programming in C# on Debian, you can use the cross-platform GTK# library or consider using other cross-platform UI frameworks such as AvaloniaUI. These libraries are compatible with C# and can be used to create GUI applications that can run on Debian and other operating systems.
- and will they run on windows?
- Yes, both GTK# and AvaloniaUI applications can run on Windows in addition to Debian and other operating systems. They are designed to be cross-platform, allowing you to create GUI applications that can be deployed on multiple platforms including Windows.
- ok, during conversation i have read some info from you, give a more detailed description about: Mono framework, AvaloniaUI and GTK#.
- ***The Mono framework is an open-source implementation of Microsoft's .NET Framework, allowing developers to build and run cross-platform applications using C#. It includes a C# compiler, a Common Language Runtime (CLR), and a set of class libraries. Mono is compatible with various operating systems, including Linux, macOS, and Windows. AvaloniaUI is a cross-platform XAML-based UI framework for .NET. It is designed to run on Windows, Linux, and macOS, and provides a way to create modern, native user interfaces using C# and XAML. AvaloniaUI aims to be a fully-featured, extensible platform for building desktop applications.GTK# is a set of .NET bindings for the GTK+ toolkit and assorted GNOME libraries. It allows developers to create cross-platform graphical user interfaces in C# using the GTK+ toolkit, which is commonly used in Linux and Unix systems. GTK# applications can run on Windows, Linux, and macOS, making it a versatile option for cross-platform development.***
- ok, if you would get a task to write small application on C# + OpenGL + shaders and this app should work on both Linux and Windows, what stack would be your choice?
- ***For a small application that needs to work on both Linux and Windows, using C# with the OpenTK library for OpenGL and shaders would be a good choice. OpenTK provides C# bindings for OpenGL, allowing you to create cross-platform graphics applications. You can use the same codebase for both Linux and Windows, making it a convenient option for your requirements.***
- did you take into account that development process - is on debian machine?
- ***Yes, I did take that into account. Since the development process is on a Debian machine, using OpenTK with C# would still be a suitable choice for developing the application, as OpenTK is compatible with Debian and other Linux distributions, as well as Windows.***

