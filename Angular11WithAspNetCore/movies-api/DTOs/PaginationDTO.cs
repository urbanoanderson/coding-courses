namespace MoviesAPI.DTOs
{
    public class PaginationDTO
    {
        private const int MAX_RECORDS_PER_PAGE = 50;

        private int recordsPerPage = 10;

        public int RecordsPerPage
        {
            get
            {
                return recordsPerPage;
            }
            set
            {
                recordsPerPage = (value > MAX_RECORDS_PER_PAGE) ? MAX_RECORDS_PER_PAGE : value;
            }
        }

        public int Page { get; set; } = 1;
    }
}