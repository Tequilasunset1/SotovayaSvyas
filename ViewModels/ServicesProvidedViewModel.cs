using SotovayaSvyas.Models;
using SotovayaSvyas.ViewModels.FilterViewModels;
using SotovayaSvyas.ViewModels.SortViewModels;

namespace SotovayaSvyas.ViewModels
{
    public class ServicesProvidedViewModel
    {
        public IEnumerable<ServicesProvided> ServicesProvideds {  get; set; }
        public ServicesProvidedFilterViewModel Filter {  get; set; }
        public PageViewModel PageViewModel { get; set; }
        public ServicesProvidedSortViewModel SortOrder {  get; set; }
    }
}
