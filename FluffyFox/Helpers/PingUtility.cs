using System.Net.NetworkInformation;
using Timer = System.Timers.Timer;

namespace FluffyFox.Helpers;

public class PingUtility
{
    private readonly Timer _timer;
    private readonly string _hostNameOrAddress;

    public const string Germany = "DE20.vpnbook.com";
    public const string France = "FR200.vpnbook.com";
    public const string Canada = "CA149.vpnbook.com";
    public const string Russia = "rus1.freevpn4you.online";
    
    public const string UsernameVpnBook = "vpnbook";
    public const string UsernameVpn4You = "freevpn4you";
    public const string PasswordVpnBook = "n8rfzew";
    public const string PasswordVpn4You = "8729591";
    
    public event EventHandler<int> PingUpdated;

    public PingUtility(string hostNameOrAddress)
    {
        _hostNameOrAddress = hostNameOrAddress;
        _timer = new Timer();
        _timer.Interval = TimeSpan.FromSeconds(15).TotalMilliseconds;
        _timer.AutoReset = true;
        _timer.Elapsed += async (sender, e) => await UpdatePing();

        _ = UpdatePing();
    }

    public void Start()
    {
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
    }

    private async Task UpdatePing()
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
}