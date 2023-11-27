namespace SotovayaSvyas.ViewModels.FilterViewModels
{
    public class ServicesProvidedFilterViewModel
    {
        public TimeOnly? TimeFind { get; set; }
        public int? QuantitySmsFind { get; set; }
        public int? DataVolumeFind { get; set; }
        public string? SubscriberFind {  get; set; } 
        public ServicesProvidedFilterViewModel()
        {
            
        }
        public ServicesProvidedFilterViewModel(TimeOnly timeFind, int quantitySmsFind, int dataVolumeFind)
        {
            TimeFind = timeFind;
            QuantitySmsFind = quantitySmsFind;
            DataVolumeFind = dataVolumeFind;
        }
    }
}
