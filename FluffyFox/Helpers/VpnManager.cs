using System.Diagnostics;
using System.IO;
using System.Text;

namespace FluffyFox.Helpers;

public class VpnManager
{
    private static string FolderPath => string.Concat(Directory.GetCurrentDirectory(),
        "\\VPN");
    public void Connect(string vpnName, string username, string password)
    {
        if (!Directory.Exists(FolderPath))
            Directory.CreateDirectory(FolderPath);
        
        var sb = new StringBuilder();
        sb.AppendLine("[VPN]");
        sb.AppendLine("MEDIA=rastapi");
        sb.AppendLine("Port=VPN2-0");
        sb.AppendLine("Device=WAN Miniport (IKEv2)");
        sb.AppendLine("DEVICE=vpn");
        sb.AppendLine("PhoneNumber=" + vpnName);
        
        File.WriteAllText(FolderPath + "\\VpnConnection.pbk", sb.ToString());
        
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
        process.Start();
        process.WaitForExit();
    }

    public void Disconnect()
    {
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
        process.Start();
        process.WaitForExit();
    }   
}