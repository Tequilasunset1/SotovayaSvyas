using SotovayaSvyas.Models;
using SotovayaSvyas.ViewModels.FilterViewModels;
using SotovayaSvyas.ViewModels.SortViewModels;

namespace SotovayaSvyas.ViewModels
{
    public class TreatiesViewModel
    {
        public IEnumerable<Treaty> Treaties { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public TreatyFilterViewModel Filter { get; set;}
        public TreatySortViewModel Sort { get; set;}
    }
}
