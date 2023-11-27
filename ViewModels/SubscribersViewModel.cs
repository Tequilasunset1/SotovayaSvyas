using SotovayaSvyas.Models;
using SotovayaSvyas.ViewModels.FilterViewModels;
using SotovayaSvyas.ViewModels.SortViewModels;

namespace SotovayaSvyas.ViewModels
{
    public class SubscribersViewModel
    {
        public IEnumerable<Subscriber> Subscribers {  get; set; }
        public SubscriberFilterViewModel Filter {  get; set; }
        public PageViewModel PageViewModel { get; set; }
        public SubscriberSortViewModel SortOrder {  get; set; }
    }
}
