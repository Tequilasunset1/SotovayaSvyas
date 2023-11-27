namespace SotovayaSvyas.ViewModels.FilterViewModels
{
    public class SubscriberFilterViewModel
    {
        public string? SurnameFind { get; set; } = null!;
        public string? NameFind { get; set; } = null!;
        public string? LastnameFind { get; set; } = null!;

        public string? AddressFind { get; set; } = null!;

        public string? PassportDetailsFind { get; set; } = null!;
        public SubscriberFilterViewModel()
        {
            
        }
    }
}
