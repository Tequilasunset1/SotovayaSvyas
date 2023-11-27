namespace SotovayaSvyas.ViewModels.FilterViewModels
{
    public class TreatyFilterViewModel
    {
        public string? SubscriberFind { get; set; } = null!;

        public DateOnly?  DateConclusionFind { get; set; }

        public string? TariffFind { get; set; } = null!;

        public string? PhoneNumberFind { get; set; } = null!;

        public string? SurnameFind { get; set; } = null!;
        public string? NameFind { get; set; } = null!;
        public string? LastnameFind { get; set; } = null!;
        public TreatyFilterViewModel()
        {
            
        }
    }
}
