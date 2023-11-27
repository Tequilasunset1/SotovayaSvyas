using Microsoft.Data.SqlClient;
using SotovayaSvyas.ViewModels.SortStates;

namespace SotovayaSvyas.ViewModels.SortViewModels
{
    public class ServicesProvidedSortViewModel
    {
        public ServicesProvidedSortState TimeSort { get; set; }
        public ServicesProvidedSortState QuantitySmsSort { get; set; }
        public ServicesProvidedSortState DataVolumeSort { get; set; }
        public ServicesProvidedSortState SubscriberSort { get; set; }
        public ServicesProvidedSortState CurrentState { get; set; }
        public ServicesProvidedSortViewModel(ServicesProvidedSortState sortOrder)
        {
            TimeSort = sortOrder == ServicesProvidedSortState.TimeAsc ? ServicesProvidedSortState.TimeDesc : ServicesProvidedSortState.TimeAsc;
            QuantitySmsSort = sortOrder == ServicesProvidedSortState.QuantitySmsAsc ? ServicesProvidedSortState.QuantitySmsDesc : ServicesProvidedSortState.QuantitySmsAsc;
            DataVolumeSort = sortOrder == ServicesProvidedSortState.DataVolumeAsc ? ServicesProvidedSortState.DataVolumeDesc : ServicesProvidedSortState.DataVolumeAsc;
            SubscriberSort = sortOrder == ServicesProvidedSortState.SubscriberAsc ? ServicesProvidedSortState.SubscriberDesc : ServicesProvidedSortState.SubscriberAsc;
            CurrentState = sortOrder;
        }
    }
}
