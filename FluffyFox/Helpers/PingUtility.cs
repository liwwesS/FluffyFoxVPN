using System.Net.NetworkInformation;
using Timer = System.Timers.Timer;

namespace FluffyFox.Helpers;

public class PingUtility
{
    private readonly Timer _timer;
    private readonly string _hostNameOrAddress;

    public static readonly Dictionary<string, string> RegionHosts = new()
    {
        { "DE", "DE20.vpnbook.com" },
        { "FR", "FR200.vpnbook.com" },
        { "CA", "CA149.vpnbook.com" },
        { "PL", "PL134.vpnbook.com" }
    };

    public const string UsernameVpnBook = "vpnbook";
    public const string PasswordVpnBook = "dnx97sa";
    
    public event EventHandler<int> PingUpdated;

    public PingUtility(string hostNameOrAddress)
    {
        _hostNameOrAddress = hostNameOrAddress;
        _timer = new Timer
        {
            Interval = TimeSpan.FromSeconds(15).TotalMilliseconds,
            AutoReset = true
        };
        _timer.Elapsed += async (sender, e) => await UpdatePingAsync();

        _ = UpdatePingAsync();
    }

    public void Start() => _timer.Start();

    public void Stop() => _timer.Stop();

    private async Task UpdatePingAsync()
    {
        var ping = await GetPingAsync(_hostNameOrAddress);
        PingUpdated?.Invoke(this, ping);
    }

    private static async Task<int> GetPingAsync(string hostNameOrAddress)
    {
        try
        {
            using var ping = new Ping();
            var reply = await ping.SendPingAsync(hostNameOrAddress);
            if (reply.Status == IPStatus.Success)
            {
                return (int)reply.RoundtripTime;
            }
        }
        catch (PingException)
        {
        }

        return -1; 
    }

    public static string? GetHostByRegion(string region)
    {
        return RegionHosts.TryGetValue(region, out var host) ? host : null;
    }
}