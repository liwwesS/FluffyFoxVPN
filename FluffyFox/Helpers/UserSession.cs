using DataAccess.Models;

namespace FluffyFox.Helpers
{
    public class UserSession
    {
        public UsersModel CurrentUser { get; set; }
        
        public string Region { get; set; }

        public bool IsVpnConnect { get; set; } = false;
    }
}
