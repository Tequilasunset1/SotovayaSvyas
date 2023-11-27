namespace SotovayaSvyas.ViewModels
{
    public class PageViewModel
    {
        public int PageNumber {  get; set; }
        public int PageCount {  get; set; }
        public int PageSize {  get; set; }
        public bool HasNext
        {
            get
            {
                return PageNumber < PageCount;
            }
        }
        public bool HasPrevious
        {
            get
            {
                return PageNumber > 1;
            }
        }
        public PageViewModel(int pageNumber, int count, int pageSize)
        {
            PageNumber = pageNumber;
            PageCount = (int)Math.Ceiling((decimal)(count / pageSize));
            PageSize = pageSize;
        }
    }
}
