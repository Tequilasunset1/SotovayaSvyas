using SotovayaSvyas.ViewModels.SortStates;

namespace SotovayaSvyas.ViewModels.SortViewModels
{
    public class TypeTariffSortViewModel
    {
        public TypeTariffSortState TariffNameSort {  get; set; }
        public TypeTariffSortState CurrentState { get; set; }
        public TypeTariffSortViewModel( TypeTariffSortState sortOrder)
        {
            TariffNameSort = sortOrder == TypeTariffSortState.TariffNameAsc ? TypeTariffSortState.TariffNameDesc : TypeTariffSortState.TariffNameAsc;
            CurrentState = sortOrder;
        }
    }
}
