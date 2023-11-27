using SotovayaSvyas.ViewModels.SortStates;

namespace SotovayaSvyas.ViewModels.SortViewModels
{
    public class TariffPlanSortViewModel
    {
        public TariffPlanSortState NameSort { get; set; }
        public TariffPlanSortState SubscriptionLocalSort { get; set; }
        public TariffPlanSortState SubscriptionIntercitySort { get; set; }
        public TariffPlanSortState SubscriptionInternationalSort { get; set; }
        public TariffPlanSortState PriceSmsSort{ get; set; }
        public TariffPlanSortState CurrentState { get; set; }
        public TariffPlanSortViewModel(TariffPlanSortState sortOrder)
        {

            NameSort = sortOrder == TariffPlanSortState.NameAsc ? TariffPlanSortState.NameDesc : TariffPlanSortState.NameAsc;
            SubscriptionLocalSort = sortOrder == TariffPlanSortState.SubscriptionLocalAsc ? TariffPlanSortState.SubscriptionLocalDesc : TariffPlanSortState.SubscriptionLocalAsc;
            SubscriptionIntercitySort = sortOrder == TariffPlanSortState.SubscriptionIntercityAsc ? TariffPlanSortState.SubscriptionIntercityDesc : TariffPlanSortState.SubscriptionIntercityAsc;
            SubscriptionInternationalSort = sortOrder == TariffPlanSortState.SubscriptionInternationalAsc ? TariffPlanSortState.SubscriptionInternationalDesc : TariffPlanSortState.SubscriptionInternationalAsc;
            PriceSmsSort = sortOrder == TariffPlanSortState.PriceSmsAsc ? TariffPlanSortState.PriceSmsDesc : TariffPlanSortState.PriceSmsAsc;
            CurrentState = sortOrder;
        }

    }
}
