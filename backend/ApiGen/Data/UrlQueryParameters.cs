namespace ApiGen.Data
{
    public class UrlQueryParameters
    {
        const int maxPageSize = 100;
        private int _pageSize = 1;
        public int offset { get; set; } = 1;
        public string condition { get; set; }
        public int limit
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
        public bool IncludeCount { get; set; } = false;
        public int id { get; set; }
    }
}
