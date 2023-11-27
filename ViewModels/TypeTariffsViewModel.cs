using SotovayaSvyas.Models;
using SotovayaSvyas.ViewModels.SortViewModels;

namespace SotovayaSvyas.ViewModels
{
    public class TypeTariffsViewModel
    {
        public IEnumerable<TypeTariff> TypeTariffs { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public TypeTariffSortViewModel SortOrder { get; set; }
        public string NameFind { get; set; } = "";
    }
}
