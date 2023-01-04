using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace B_Skin_Api.Domain.Models
{
    public class PaginationModel
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }

        [JsonIgnore]
        public int? Offset { get; set; }

        public PaginationModel(int? page, int? pageSize, int? offSet)
        {
            Page = page.Value;
            PageSize = pageSize.Value < 1 ? 1 : pageSize.Value;
            Offset = page * pageSize;
        }
    }
}
