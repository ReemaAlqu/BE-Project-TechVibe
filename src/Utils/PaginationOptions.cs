namespace src.Utils
{
    public class PaginationOptions
    {
        public int Limit { get; set; } = 2;
        public int Offset { get; set; } = 0;
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public SortOptions? Sort { get; set; } = new SortOptions();
        public FilterOptions? Filter { get; set; } = new FilterOptions();
        
        /**********************************************************************/
        public string? Search { get; set; } = null;
        public decimal? MinPrice { get; set; } = 0; 
        public decimal? MaxPrice { get; set; } = 10000;
    }
}
