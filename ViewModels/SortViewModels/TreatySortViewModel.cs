using Microsoft.Data.SqlClient;
using SotovayaSvyas.ViewModels.SortStates;

namespace SotovayaSvyas.ViewModels.SortViewModels
{
    public class TreatySortViewModel
    {
        public TreatySortState SubscriberSort { get; set; }
        public TreatySortState DateConclusionSort { get; set; }
        public TreatySortState TariffSort { get; set; }
        public TreatySortState SurnameSort { get; set; }
        public TreatySortState NameSort { get; set; }
        public TreatySortState LastnameSort { get; set; }
        public TreatySortState CurrentState { get; set; }
        public TreatySortViewModel(TreatySortState sortOrder)
        {
            SubscriberSort = sortOrder == TreatySortState.SubscriberAsc ? TreatySortState.SubscriberDesc : TreatySortState.SubscriberAsc;
            DateConclusionSort = sortOrder == TreatySortState.DateConclusionAsc ? TreatySortState.DateConclusionDesc : TreatySortState.DateConclusionAsc;
            TariffSort = sortOrder == TreatySortState.TariffAsc ? TreatySortState.TariffDesc : TreatySortState.TariffAsc;
            SurnameSort = sortOrder == TreatySortState.SurnameAsc ? TreatySortState.SurnameDesc : TreatySortState.SurnameAsc;
            NameSort = sortOrder == TreatySortState.NameAsc ? TreatySortState.NameDesc : TreatySortState.NameAsc;
            LastnameSort = sortOrder == TreatySortState.LastnameAsc ? TreatySortState.LastnameDesc : TreatySortState.LastnameAsc;
            CurrentState = sortOrder;
        }
    }
}
