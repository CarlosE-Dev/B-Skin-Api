using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace B_Skin_Api.Domain.Models
{
    public class PaginationModel
    {
        public int? Page { get; set; }

        public int? PageSize { get; set; }
        public bool IgnorePagination { get; set; }

        [JsonIgnore]
        public int? Offset { get; set; }

        public PaginationModel(int? page, int? pageSize, int? offSet, bool ignorePagination = false)
        {
            Page = page.Value < 1 ? 0 : page.Value;
            PageSize = pageSize.Value < 1 ? 1 : pageSize.Value;
            Offset = Page * PageSize;
            IgnorePagination = ignorePagination ? ignorePagination : false;
        }
    }
}
