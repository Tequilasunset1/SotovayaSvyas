using SotovayaSvyas.Models;
using SotovayaSvyas.ViewModels.FilterViewModels;
using SotovayaSvyas.ViewModels.SortViewModels;

namespace SotovayaSvyas.ViewModels
{
    public class TariffPlansViewModel
    {
        public IEnumerable<TariffPlan> TariffPlans { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public TariffPlanFilterViewModel Filter {  get; set; }
        public TariffPlanSortViewModel SortOrder { get; set; }
    }
}
