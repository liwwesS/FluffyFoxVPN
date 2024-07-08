using System.Diagnostics;
using System.IO;
using System.Text;

namespace FluffyFox.Helpers;

public class VpnManager
{
    private static string FolderPath => string.Concat(Directory.GetCurrentDirectory(),"\\VPN");

    public static async Task Connect(string vpnName, string username, string password)
    {
        if (!Directory.Exists(FolderPath))
            Directory.CreateDirectory(FolderPath);

        var runningProcess = Process.GetProcessesByName("rasdial").ToList();
        if (runningProcess.Count != 0)
        {
            foreach (var item in runningProcess)
            {
                item.Kill();
                await item.WaitForExitAsync();
            }
        }

        var pbkContent = new StringBuilder()
            .AppendLine("[VPN]")
            .AppendLine("MEDIA=rastapi")
            .AppendLine("Port=VPN2-0")
            .AppendLine("Device=WAN Miniport (IKEv2)")
            .AppendLine("DEVICE=vpn")
            .AppendLine("PhoneNumber=" + vpnName)
            .ToString();

        await File.WriteAllTextAsync(FolderPath + "\\VpnConnection.pbk", pbkContent.ToString());
        
        var process = new Process();
        var startInfo = new ProcessStartInfo
        {
            FileName = "rasdial.exe",
            Arguments = $"\"VPN\" {username} {password} /phonebook:\"{FolderPath}\\VpnConnection.pbk\"",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        process.StartInfo = startInfo;
        
        await Task.Run(() => process.Start());
        await process.WaitForExitAsync();
    }

    public static async Task Disconnect()
    {
        var runningProcess = Process.GetProcessesByName("rasdial").ToList();
        if (runningProcess.Count != 0)
        {
            foreach (var item in runningProcess)
            {
                item.Kill();
                await item.WaitForExitAsync();
            }
        }

        using var process = new Process();
        var startInfo = new ProcessStartInfo
        {
            FileName = "rasdial.exe",
            Arguments = $"/d",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        process.StartInfo = startInfo;
        
        await Task.Run(() => process.Start());
        await process.WaitForExitAsync();
    }   
}