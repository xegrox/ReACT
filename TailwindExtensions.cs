using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ReACT;

public static class TailwindExtensions
{
    public static void RunTailwind(this WebApplication app, string input, string output)
    {
        var os = OperatingSystem.IsWindows() ? "windows"
            : OperatingSystem.IsLinux() ? "linux"
            : OperatingSystem.IsMacOS() ? "macos"
            : "unknown";
        var arch = RuntimeInformation.OSArchitecture switch
        {
            Architecture.X64 => "x64",
            Architecture.Arm => "armv7",
            Architecture.Arm64 => "arm64",
            Architecture.X86 => "x86",
            Architecture.Wasm => "wasm",
            Architecture.S390x => "s390x",
            _ => "unknown"
        };
        var path = Path.Join(app.Environment.ContentRootPath, "Binaries", $"tailwindcss-{os}-{arch}");
        if (OperatingSystem.IsWindows()) path += ".exe";
        var unsupportedException = new PlatformNotSupportedException($"No tailwind standalone binary found for this platform at {path}");
        if (!File.Exists(path)) throw unsupportedException;

        var psStartInfo = new ProcessStartInfo(path)
        {
            Arguments = $"-i {input} -o {output} --watch",
            UseShellExecute = false,
            WorkingDirectory = app.Environment.ContentRootPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        var ps = Process.Start(psStartInfo);
        if (ps == null) throw new InvalidOperationException($"Failed to start tailwind standalone executable at {path}"); 
        ps.EnableRaisingEvents = true;
    }
}