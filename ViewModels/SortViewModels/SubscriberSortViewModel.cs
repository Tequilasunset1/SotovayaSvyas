using SotovayaSvyas.ViewModels.SortStates;

namespace SotovayaSvyas.ViewModels.SortViewModels
{
    public class SubscriberSortViewModel
    {
        public SubscriberSortState SurnameSort { get; set; }
        public SubscriberSortState NameSort { get; set; }
        public SubscriberSortState LastnameSort { get; set; }

        public SubscriberSortState AddressSort { get; set; }
        public SubscriberSortState PassportDetailsSort { get; set; }
        public SubscriberSortState CurrentState { get; set; }
        public SubscriberSortViewModel(SubscriberSortState sortOrder)
        {
            SurnameSort = sortOrder == SubscriberSortState.SurnameAsc ? SubscriberSortState.SurnameDesc : SubscriberSortState.SurnameAsc;
            NameSort = sortOrder == SubscriberSortState.NameAsc ? SubscriberSortState.NameDesc : SubscriberSortState.NameAsc;
            LastnameSort = sortOrder == SubscriberSortState.LastnameAsc ? SubscriberSortState.LastnameDesc : SubscriberSortState.LastnameAsc;
            AddressSort = sortOrder == SubscriberSortState.AddressAsc ? SubscriberSortState.AddressDesc : SubscriberSortState.AddressAsc;
            PassportDetailsSort = sortOrder == SubscriberSortState.PassportDetailsAsc ? SubscriberSortState.PassportDetailsDesc : SubscriberSortState.PassportDetailsAsc;
            CurrentState = sortOrder;
        }
    }
}
