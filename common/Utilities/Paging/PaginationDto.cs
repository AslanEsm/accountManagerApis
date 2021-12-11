using common.Data;


namespace common.Utilities.Paging
{
    public class PaginationDto
    {
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get { return _pageSize; }
            set => _pageSize = (value > Constants.MaxPageSize) ? Constants.MaxPageSize : value;
        }
    }
}
