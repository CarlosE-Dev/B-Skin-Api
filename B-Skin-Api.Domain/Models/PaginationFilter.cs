using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace B_Skin_Api.Domain.Models
{
    public class PaginationFilter
    {
        /// <summary>
        /// The Page Number -> Example: Page 1
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "The value for field {0} must be bigger than 0")]
        public int? Page { get; set; }

        /// <summary>
        /// The maximum results number for each page
        /// example: if you have 10 results and enter PageSize 5, you'll have 2 pages with 5 records each
        /// example2: if you have 8 results and enter PageSize 3, you'll have 3 pages like -> page 1: 3 records, page 2: 3 records, page 3: 2 records
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "The value for field {0} must be bigger than 0")]
        public int? PageSize { get; set; }

        /// <summary>
        /// True if you want to ignore the parameters above (Page and PageSize) and get all the avaiable results
        /// </summary>
        [DefaultValue(false)]
        public bool IgnorePagination { get; set; }

        public PaginationFilter(int? page, int? pageSize, bool ignorePagination = false)
        {
            Page = page.Value < 1 ? 0 : page.Value;
            PageSize = pageSize.Value < 1 ? 1 : pageSize.Value;
            IgnorePagination = ignorePagination ? ignorePagination : false;
        }
    }
}
