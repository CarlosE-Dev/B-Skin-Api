using B_Skin_Api.Domain.Enums;
using System.Text.Json.Serialization;

namespace B_Skin_Api.Domain.Models
{
    public class TShirtFilterModel
    {
        /// <summary>
        /// The Page Number -> Example: Page 1
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// The maximum results number for each page
        /// example: if you have 10 results and enter PageSize 5, you'll have 2 pages with 5 records each
        /// example2: if you have 8 results and enter PageSize 3, you'll have 3 pages like -> page 1: 3 records, page 2: 3 records, page 3: 2 records
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// True if you want to ignore the parameters above (Page and PageSize) and get all the avaiable results
        /// </summary>
        public bool IgnorePagination { get; set; }

        /// <summary>
        /// Dont enter value for this property, the constructor will handle with this
        /// </summary>
        [JsonIgnore]
        public int? Offset { get; set; }

        /// <summary>
        /// Filter By Properties - 1 -> Brand , 2 -> Size
        /// You need to combine this value with the correspondent properties of the filter
        /// Example: If you enter 1 and did'nt enter a value for ProviderId, the filter will not work
        /// </summary>
        public EFilterTShirt? FilterType { get; set; }

        /// <summary>
        /// Filter Property, initial price, if you don't want to filter by price, enter null
        /// </summary>
        public decimal? InitialPrice { get; set; }

        /// <summary>
        /// Filter Property, final price, should be greater than 0, if you don't want to filter by price, enter null
        /// </summary>
        public decimal? FinalPrice { get; set; }

        /// <summary>
        /// Filter Property, Id for Provider, if you don't want to filter by Brand, enter null
        /// </summary>
        public long? ProviderId { get; set; }

        /// <summary>
        /// Filter by Size - 1 -> XS, 2 -> S, 3 -> M, 4 -> L, 5 -> XL
        /// Filter Property, Size, if you don't want to filter by Size, enter null
        /// </summary>
        public ESizeModel? Size { get; set; }

        /// <summary>
        /// Configure the order of the results - 1 -> By Highest Price, 2 -> By Lowest Price
        /// </summary>
        public EOrderBy? OrderBy { get; set; }

        public TShirtFilterModel(
            int? page, 
            int? pageSize, 
            int? offSet, 
            EFilterTShirt? filterType = null, 
            decimal? initialPrice = null, 
            decimal? finalPrice = null, 
            long? providerId = null, 
            ESizeModel? size = null,
            EOrderBy? orderBy = null,
            bool ignorePagination = false
            )
        {
            Page = page.Value < 1 ? 0 : page.Value;
            PageSize = pageSize.Value < 1 ? 1 : pageSize.Value;
            Offset = Page * PageSize;
            IgnorePagination = ignorePagination ? ignorePagination : false;
            FilterType = filterType.HasValue ? filterType.Value : null;
            InitialPrice = initialPrice.HasValue ? initialPrice.Value : null;
            FinalPrice = finalPrice.HasValue ? finalPrice.Value : null;
            ProviderId = providerId.HasValue ? providerId.Value : null;
            Size = size.HasValue ? size.Value : null;
            OrderBy = orderBy.HasValue ? orderBy.Value : null;
        }
    }
}
