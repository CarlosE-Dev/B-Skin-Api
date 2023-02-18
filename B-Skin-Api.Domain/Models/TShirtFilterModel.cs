using B_Skin_Api.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace B_Skin_Api.Domain.Models
{
    public class TShirtFilterModel
    {
        /// <summary>
        /// Provider Id (Filter by Brand)
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "The value for field {0} must be bigger than 0")]
        public long? ProviderId { get; set; }

        /// <summary>
        /// 1 -> XS, 2 -> S, 3 -> M, 4 -> L, 5 -> XL
        /// </summary>
        [Range(1, 5, ErrorMessage = "The value for field {0} must be {1} to {2}")]
        public ESizeModel? Size { get; set; }

        /// <summary>
        /// Results Prices starting at:
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "The value for field {0} must be bigger than 0")]
        public decimal? InitialPrice { get; set; }

        /// <summary>
        /// Results Max Price:
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "The value for field {0} must be bigger than 0")]
        public decimal? FinalPrice { get; set; }

        /// <summary>
        /// F / M / U
        /// </summary>
        [StringLength(1, ErrorMessage = "The length of the field {0} must be {1} character", MinimumLength = 1)]
        public string Gender { get; set; }

        /// <summary>
        /// 1 -> By Highest Price, 2 -> By Lowest Price, 3 -> By Name
        /// </summary>
        [Range(1, 3, ErrorMessage = "The value for field {0} must be {1} to {2}")]
        public EOrderBy? OrderBy { get; set; }

        public TShirtFilterModel(
            long? providerId = null, 
            ESizeModel? size = null,
            decimal? initialPrice = null, 
            decimal? finalPrice = null, 
            EOrderBy? orderBy = null,
            string gender = null
            )
        {
            InitialPrice = initialPrice.HasValue ? initialPrice.Value : null;
            FinalPrice = finalPrice.HasValue ? finalPrice.Value : null;
            ProviderId = providerId.HasValue ? providerId.Value : null;
            Size = size.HasValue ? size.Value : null;
            OrderBy = orderBy.HasValue ? orderBy.Value : null;
            Gender = gender == "" ? gender : null;
        }
    }
}
