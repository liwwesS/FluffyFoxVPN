namespace FluffyFox.ViewModels
{
    public class RegionViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Code { get; set; }

        public bool? IsFavourite { get; set; }
        public bool IsPremium { get; set; }

        public int Ping { get; set; }
    }
}
