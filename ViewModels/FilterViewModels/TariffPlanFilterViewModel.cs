namespace SotovayaSvyas.ViewModels.FilterViewModels
{
    public class TariffPlanFilterViewModel
    {
        public string? TypeNameFind { get; set; } = null!;

        public decimal? SubscriptionLocalFind { get; set; }

        public decimal? SubscriptionIntercityFind { get; set; }

        public decimal? SubscriptionInternationalFind { get; set; }

        public string? TypeTariffFind { get; set; }

        public int? PriceSmsFind { get; set; }
        public TariffPlanFilterViewModel()
        {
            
        }

        public TariffPlanFilterViewModel(string typeNameFind, decimal subscriptionLocalFind, decimal subscriptionIntercityFind, decimal subscriptionInternationalFind, string typeTariffFind, int priceSmsFind)
        {
            TypeNameFind = typeNameFind;
            SubscriptionLocalFind = subscriptionLocalFind;  
            SubscriptionIntercityFind = subscriptionIntercityFind;
            SubscriptionInternationalFind = subscriptionInternationalFind;
            TypeTariffFind = typeTariffFind;
            PriceSmsFind = priceSmsFind;
        }
    }
}
