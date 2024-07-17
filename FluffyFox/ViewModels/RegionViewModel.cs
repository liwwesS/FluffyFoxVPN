using System.Windows;

namespace FluffyFox.ViewModels
{
    public class RegionViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Code { get; set; }

        public int Ping { get; set; }
        public Visibility Visibility { get; set; }
    }
}
