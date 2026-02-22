namespace DellyBelly.Application.DTOs
{
    /// <summary>
    /// Generic paginated response envelope used by all paginated endpoints.
    /// </summary>
    public class PagedResult<T>
    {
        /// <summary>Items for the current page.</summary>
        public IEnumerable<T> Items { get; set; } = [];

        /// <summary>Current page number (1-based).</summary>
        public int Page { get; set; }

        /// <summary>Number of items requested per page.</summary>
        public int PageSize { get; set; }

        /// <summary>Total number of items that match the query (across all pages).</summary>
        public int TotalCount { get; set; }

        /// <summary>Total number of pages.</summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        /// <summary>Whether there is a page before the current one.</summary>
        public bool HasPreviousPage => Page > 1;

        /// <summary>Whether there is a page after the current one.</summary>
        public bool HasNextPage => Page < TotalPages;
    }
}
